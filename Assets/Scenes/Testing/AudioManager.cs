using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAudio
{
    [Serializable]
    public struct SoundEntry
    {
        public string key;
        public AudioClip clip;
        [Range(0f,1f)] public float volume;
        [Range(0.1f,3f)] public float pitchMin;
        [Range(0.1f,3f)] public float pitchMax;
    }

    [DisallowMultipleComponent]
    public sealed class AudioManager : MonoBehaviour
    {
        public static AudioManager I { get; private set; }

        [Header("Libraries (assign in Inspector)")]
        [SerializeField] private SoundEntry[] sfxLibrary;
        [SerializeField] private SoundEntry[] musicLibrary;

        [Header("SFX Pool")]
        [SerializeField] private int initialSfxSources = 8;
        [SerializeField] private int maxSfxSources = 24;

        [Header("Music")]
        [SerializeField] private float musicCrossfade = 0.8f; // seconds

        [Header("Volumes")]
        [Range(0f,1f)] public float masterVolume = 1f;
        [Range(0f,1f)] public float musicVolume  = 1f;
        [Range(0f,1f)] public float sfxVolume    = 1f;

        [Header("Optional")]
        [SerializeField] private string startMusicKey = ""; // leave empty to start silent

        readonly Dictionary<string, SoundEntry> _sfx = new();
        readonly Dictionary<string, SoundEntry> _music = new();

        readonly List<AudioSource> _sfxPool = new();
        int _sfxRing;

        AudioSource _musicA, _musicB;
        bool _useA = true;
        Coroutine _xfadeCo;

        void Awake()
        {
            if (I && I != this) { Destroy(gameObject); return; }
            I = this;
            DontDestroyOnLoad(gameObject);
            BuildMaps();
            BuildSfxPool(initialSfxSources);
            BuildMusicSources();
            ApplyMasterVolume();
            if (!string.IsNullOrEmpty(startMusicKey)) PlayMusic(startMusicKey, true, musicCrossfade);
        }

        void OnValidate()
        {
            initialSfxSources = Mathf.Max(1, initialSfxSources);
            maxSfxSources     = Mathf.Max(initialSfxSources, maxSfxSources);
            musicCrossfade    = Mathf.Max(0f, musicCrossfade);
        }

        void BuildMaps()
        {
            _sfx.Clear(); _music.Clear();
            if (sfxLibrary != null)
                foreach (var e in sfxLibrary)
                    if (e.clip && !string.IsNullOrEmpty(e.key)) _sfx[e.key] = Normalize(e);
            if (musicLibrary != null)
                foreach (var e in musicLibrary)
                    if (e.clip && !string.IsNullOrEmpty(e.key)) _music[e.key] = Normalize(e);
        }

        static SoundEntry Normalize(SoundEntry e)
        {
            if (e.volume <= 0f) e.volume = 1f;
            if (e.pitchMin <= 0f) e.pitchMin = 1f;
            if (e.pitchMax <= 0f) e.pitchMax = 1f;
            if (e.pitchMax < e.pitchMin) (e.pitchMin, e.pitchMax) = (e.pitchMax, e.pitchMin);
            return e;
        }

        void BuildSfxPool(int count)
        {
            for (int i = 0; i < count; i++)
                _sfxPool.Add(MakeChildSource("SFX_" + i, spatial2D:true));
        }

        void BuildMusicSources()
        {
            _musicA = MakeChildSource("MusicA", spatial2D:true);
            _musicB = MakeChildSource("MusicB", spatial2D:true);
            _musicA.loop = true; _musicB.loop = true;
            _musicA.playOnAwake = false; _musicB.playOnAwake = false;
            _musicA.volume = 0f; _musicB.volume = 0f;
        }

        AudioSource MakeChildSource(string name, bool spatial2D)
        {
            var go = new GameObject(name);
            go.transform.SetParent(transform, false);
            var src = go.AddComponent<AudioSource>();
            src.spatialBlend = spatial2D ? 0f : 1f;
            src.playOnAwake = false;
            src.bypassListenerEffects = false;
            src.bypassReverbZones = true;
            return src;
        }

        AudioSource GetFreeSfxSource()
        {
            for (int i = 0; i < _sfxPool.Count; i++)
            {
                var src = _sfxPool[i];
                if (!src.isPlaying) return src;
            }
            if (_sfxPool.Count < maxSfxSources)
            {
                var src = MakeChildSource("SFX_" + _sfxPool.Count, spatial2D:true);
                _sfxPool.Add(src);
                return src;
            }
            // reuse round-robin
            _sfxRing = (_sfxRing + 1) % _sfxPool.Count;
            return _sfxPool[_sfxRing];
        }

        public void UnlockAudio()
        {
#if UNITY_WEBGL
            // Calling Play() once on a muted source after a user gesture helps unlock the browser audio context.
            var src = GetFreeSfxSource();
            src.volume = 0f;
            src.clip = null;
            src.Play();
            src.Stop();
#endif
        }

        public void PlaySFX(string key, float volumeScale = 1f)
        {
            if (!_sfx.TryGetValue(key, out var e) || e.clip == null) { Debug.LogWarning($"[Audio] SFX key '{key}' not found."); return; }
            var src = GetFreeSfxSource();
            src.pitch  = UnityEngine.Random.Range(e.pitchMin, e.pitchMax);
            float v = Mathf.Clamp01(e.volume * sfxVolume * masterVolume * volumeScale);
            src.PlayOneShot(e.clip, v);
        }

        public void PlayMusic(string key, bool loop = true, float crossfade = -1f)
        {
            if (!_music.TryGetValue(key, out var e) || e.clip == null) { Debug.LogWarning($"[Audio] Music key '{key}' not found."); return; }
            var next = _useA ? _musicB : _musicA;
            var cur  = _useA ? _musicA : _musicB;

            next.clip = e.clip;
            next.loop = loop;
            next.pitch = 1f;
            next.volume = 0f;
            next.Play();

            if (_xfadeCo != null) StopCoroutine(_xfadeCo);
            _xfadeCo = StartCoroutine(XFade(cur, next, (crossfade < 0f ? musicCrossfade : crossfade), e.volume));
            _useA = !_useA;
        }

        public void StopMusic(float fadeOut = 0.3f)
        {
            var cur = _useA ? _musicA : _musicB;
            if (_xfadeCo != null) StopCoroutine(_xfadeCo);
            _xfadeCo = StartCoroutine(XFade(cur, null, fadeOut, 1f));
        }

        IEnumerator XFade(AudioSource from, AudioSource to, float duration, float targetEntryVol)
        {
            duration = Mathf.Max(0f, duration);
            float t = 0f;
            float fromStart = from ? from.volume : 0f;
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                float a = duration > 0f ? t / duration : 1f;
                float musicScale = musicVolume * masterVolume;

                if (to)   to.volume   = targetEntryVol * musicScale * a;
                if (from) from.volume = fromStart      * musicScale * (1f - a);
                yield return null;
            }
            float m = musicVolume * masterVolume;
            if (to)   to.volume   = targetEntryVol * m;
            if (from) { from.volume = 0f; from.Stop(); }
            _xfadeCo = null;
        }

        public void SetMasterVolume(float v) { masterVolume = Mathf.Clamp01(v); ApplyMasterVolume(); }
        public void SetMusicVolume (float v) { musicVolume  = Mathf.Clamp01(v); }
        public void SetSfxVolume   (float v) { sfxVolume    = Mathf.Clamp01(v); }

        void ApplyMasterVolume() => AudioListener.volume = masterVolume;

        // Inspector-friendly wrappers
        public void PlaySFX_ByKey(string key)  => PlaySFX(key);
        public void PlayMusic_ByKey(string key)=> PlayMusic(key);
    }
}
