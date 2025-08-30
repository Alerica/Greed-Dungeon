using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace DiceSystem2D
{
    public sealed class DiceManager2DUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform diceContainer;   //  horizontalLayoutGroup
        [SerializeField] private Dice2DUI dicePrefabUI;         // Image + Dice2DUI
        [SerializeField] private Button addDiceButton;          // optional (testing)
        [SerializeField] private Button rollButton;             // optional (testing)
        [SerializeField] private TMP_Text totalText;            // optional (Testing)

        [Header("Limits")]
        [SerializeField] private int hardMaxDice = 20;

        [Header("Wave")]
        [SerializeField] private float rollWaveDelay = 0.18f;   // delay between dice 

        [Header("Events")]
        public UnityEvent<int> OnTotalRolled;

        readonly List<Dice2DUI> diceList = new List<Dice2DUI>();
        Coroutine _activeRoutine;

        void Awake()
        {
            if (addDiceButton != null) addDiceButton.onClick.AddListener(() => AddDice(1));
            if (rollButton != null) rollButton.onClick.AddListener(() => RollAll());
        }

        // ---------- PUBLIC PROGRAMMATIC API ----------
        public int DiceCount => diceList.Count;

        public int AddDice(int count)
        {
            if (diceContainer == null || dicePrefabUI == null || count <= 0) return 0;

            int added = 0;
            for (int i = 0; i < count; i++)
            {
                if (diceList.Count >= hardMaxDice) break;
                var d = Instantiate(dicePrefabUI, diceContainer);
                diceList.Add(d);
                added++;
            }
            return added;
        }

        public void ClearAllDice()
        {
            if (_activeRoutine != null) { StopCoroutine(_activeRoutine); _activeRoutine = null; }
            foreach (var d in diceList) if (d) Destroy(d.gameObject);
            diceList.Clear();
            UpdateTotalText(0);
        }

        public void RollAll(Action<int> onTotal = null)
        {
            if (_activeRoutine != null) { StopCoroutine(_activeRoutine); _activeRoutine = null; }
            if (diceList.Count == 0) { onTotal?.Invoke(0); OnTotalRolled?.Invoke(0); UpdateTotalText(0); return; }
            _activeRoutine = StartCoroutine(RollWaveRoutine(onTotal, destroyAfter:false, destroyDelay:0f));
        }

        public void AddAndRoll(int count, Action<int> onTotal = null, bool destroyAfter = true, float destroyDelay = 0f)
        {
            int actuallyAdded = AddDice(count);
            if (actuallyAdded == 0 && diceList.Count == 0)
            {
                onTotal?.Invoke(0); OnTotalRolled?.Invoke(0); UpdateTotalText(0);
                return;
            }
            if (_activeRoutine != null) { StopCoroutine(_activeRoutine); _activeRoutine = null; }
            _activeRoutine = StartCoroutine(RollWaveRoutine(onTotal, destroyAfter, destroyDelay));
        }
        // --------------------------------------------

        IEnumerator RollWaveRoutine(Action<int> onTotal, bool destroyAfter, float destroyDelay)
        {
            int total = 0;
            int finished = 0;

            foreach (var dice in diceList)
            {
                dice.Roll(result =>
                {
                    total += result;
                    finished++;
                    if (finished == diceList.Count)
                    {
                        UpdateTotalText(total);
                        onTotal?.Invoke(total);
                        OnTotalRolled?.Invoke(total);
                    }
                });
                yield return new WaitForSecondsRealtime(rollWaveDelay);
            }

            // wait until all dice finished (paranoid in case events missed)
            while (finished < diceList.Count) yield return null;

            if (destroyAfter)
            {
                if (destroyDelay > 0f) yield return new WaitForSecondsRealtime(destroyDelay);
                ClearAllDice();
            }

            _activeRoutine = null;
        }

        void UpdateTotalText(int total)
        {
            if (totalText != null) totalText.text = $"Total: {total}";
        }
    }
}
