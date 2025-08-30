using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DiceSystem2D
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public sealed class Dice2DUI : MonoBehaviour
    {
        [Header("Sprites")]
        [SerializeField] private Sprite[] faceSprites = new Sprite[6];
        [SerializeField] private Sprite[] spinSprites = Array.Empty<Sprite>();

        [Header("Timing & Motion")]
        [Min(0.2f)] [SerializeField] private float rollDuration = 0.9f;
        [SerializeField] private float jumpHeight = 40f;  
        [SerializeField] private float spinFPS = 18f;

        [Header("Randomness")]
        [SerializeField] private int fixedSeed = -1;

        [Header("Output")]
        public UnityEvent<int> OnRolledUnityEvent;
        public event Action<int> OnRolled;

        public bool IsRolling { get; private set; }
        public int CurrentValue { get; private set; }

        Image _image;
        RectTransform _rt;
        Vector2 _baseAnchored;
        Coroutine _rollRoutine;
        System.Random _rng;

        void Awake()
        {
            _image = GetComponent<Image>();
            _rt = GetComponent<RectTransform>();
            _baseAnchored = _rt.anchoredPosition;
            _rng = (fixedSeed >= 0) ? new System.Random(fixedSeed) : new System.Random();

            if (faceSprites == null || faceSprites.Length != 6) Debug.LogError("[Dice2DUI] Assign exactly 6 face sprites.", this);
            if (spinSprites == null || spinSprites.Length == 0) Debug.LogWarning("[Dice2DUI] No spin sprites assigned.", this);

            if (faceSprites != null && faceSprites.Length == 6 && faceSprites[0] != null)
            {
                _image.sprite = faceSprites[0];
                CurrentValue = 1;
            }
        }

        void OnValidate()
        {
            if (faceSprites == null || faceSprites.Length != 6)
            {
                var arr = new Sprite[6];
                if (faceSprites != null)
                    for (int i = 0; i < Mathf.Min(6, faceSprites.Length); i++) arr[i] = faceSprites[i];
                faceSprites = arr;
            }
            rollDuration = Mathf.Max(0.2f, rollDuration);
            spinFPS = Mathf.Max(1f, spinFPS);
            jumpHeight = Mathf.Max(0f, jumpHeight);
        }

        public bool Roll(Action<int> callback = null, bool allowInterrupt = false)
        {
            if (IsRolling)
            {
                if (!allowInterrupt) return false;
                if (_rollRoutine != null) StopCoroutine(_rollRoutine);
                _rollRoutine = null; IsRolling = false; _rt.anchoredPosition = _baseAnchored;
            }
            _rollRoutine = StartCoroutine(RollRoutine(callback));
            return true;
        }

        public void RollFromButton() => Roll();

        IEnumerator RollRoutine(Action<int> callback)
        {
            IsRolling = true;
            _baseAnchored = _rt.anchoredPosition;

            float elapsed = 0f;
            float spinInterval = 1f / spinFPS;
            float spinAccum = 0f;
            int spinIndex = 0;
            bool hasSpin = spinSprites != null && spinSprites.Length > 0;
            int finalValue = ((_rng.Next() % 6) + 1);

            while (elapsed < rollDuration)
            {
                float dt = Time.unscaledDeltaTime;
                elapsed += dt;
                spinAccum += dt;

                if (hasSpin && spinAccum >= spinInterval)
                {
                    spinAccum -= spinInterval;
                    spinIndex = (spinIndex + 1) % spinSprites.Length;
                    _image.sprite = spinSprites[spinIndex];
                }

                float t = Mathf.Clamp01(elapsed / rollDuration);
                float arc = 4f * t * (1f - t);
                float yOffset = arc * jumpHeight;

                var p = _baseAnchored; p.y += yOffset;
                _rt.anchoredPosition = p;

                yield return null;
            }

            _rt.anchoredPosition = _baseAnchored;

            var sprite = (faceSprites != null && faceSprites.Length == 6) ? faceSprites[finalValue - 1] : null;
            if (sprite != null) _image.sprite = sprite;
            CurrentValue = finalValue;

            OnRolled?.Invoke(finalValue);
            OnRolledUnityEvent?.Invoke(finalValue);
            callback?.Invoke(finalValue);

            _rollRoutine = null;
            IsRolling = false;
        }

        public void SetFaceInstant(int value)
        {
            value = Mathf.Clamp(value, 1, 6);
            if (faceSprites != null && faceSprites.Length == 6 && faceSprites[value - 1] != null)
            {
                _image.sprite = faceSprites[value - 1];
                CurrentValue = value;
            }
        }

        public void Reseed(int seed = -1)
        {
            fixedSeed = seed;
            _rng = (seed >= 0) ? new System.Random(seed) : new System.Random();
        }
    }
}
