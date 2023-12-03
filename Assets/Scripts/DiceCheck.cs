using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(IDiceCheckAnimator))]
public class DiceCheck : MonoBehaviour, IDiceCheck
{
    [SerializeField] private Camera _uiCamera;
    [Space]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _subtitleText;
    [SerializeField] private TextMeshProUGUI _difficultyValueText;
    [SerializeField] private TextMeshProUGUI _totalBonusText;
    [Space]
    [SerializeField] private Transform _diceHolder;
    [SerializeField] private DiceModificatorApplicator _modificatorsApplicator;

    private IDiceCheckAnimator _animator;

    private IDice _dice;

    private int _difficulty;

    private bool _initialized = false;

    public DiceResult Result { get; private set; } = DiceResult.Failure;

    private void Awake()
    {
        _animator = GetComponent<IDiceCheckAnimator>();
    }

    public void Init(string title, string subtitle, int difficulty, Dice dicePrefab, IEnumerable<IDiceModificatorData> modificators)
    {
        if (_initialized) ResetSettings();

        _titleText.text = title;
        _subtitleText.text = subtitle;

        _difficulty = difficulty;
        _difficultyValueText.text = _difficulty.ToString();

        _dice = Instantiate(dicePrefab, _diceHolder.position, Quaternion.identity, _diceHolder);
        _dice.MouseDown += RollDice;
        _dice.Rolled += OnDiceRolled;
        _dice.ResultChanged += UpdateResult;
        _dice.ModificationAnimationEnded += CheckForCriticalSituations;

        _modificatorsApplicator.Init(modificators);
        _modificatorsApplicator.LastModificationApplied += ConcludeResults;

        _totalBonusText.text = "Total Bonus " + (_modificatorsApplicator.TotalBonus > 0 ? "+" : "") + _modificatorsApplicator.TotalBonus;

        _initialized = true;
    }

    public void Show()
    {
        _animator.Show();
    }

    public void Hide()
    {
        _animator.Hide();
        ResetSettings();
    }

    private void ResetSettings()
    {
        Result = DiceResult.Failure;
        ResetDice();
    }

    private void OnDestroy()
    {
        ResetDice();
    }

    private void ResetDice()
    {
        if (_dice != null)
        {
            _dice.MouseDown -= RollDice;
            _dice.Rolled -= OnDiceRolled;
            _dice.ResultChanged -= UpdateResult;
            _dice.ModificationAnimationEnded -= CheckForCriticalSituations;

            _dice.Destroy();

            _dice = null;
        }
    }

    private void RollDice()
    {
        Vector2 direction = _dice.WorldPosition - _uiCamera.ScreenToWorldPoint(Input.mousePosition);
        int numberOfBumps = 5;

        _dice.RollInDirection(direction, numberOfBumps);

        _animator.OnRoll();
    }

    private void OnDiceRolled()
    {
        CheckForCriticalSituations();

        _modificatorsApplicator.ApplyTo(_dice);
    }

    private void UpdateResult(int result)
    {
        if (_dice.HasCriticalyFailed)
        {
            Result = DiceResult.CriticalFailure;
            return;
        }
        if (_dice.HasCriticalySucceed)
        {
            Result = DiceResult.CriticalSuccess;
            return;
        }
        if (result > _difficulty) { 
            Result = DiceResult.Success;
            return;
        }

        Result = DiceResult.Failure;
    }

    private void CheckForCriticalSituations()
    {
        if (Result is DiceResult.CriticalFailure or DiceResult.CriticalSuccess)
        {
            ConcludeResults();
        }
    }

    private void ConcludeResults()
    {
        _modificatorsApplicator.Stop();

        switch (Result)
        {
            case DiceResult.CriticalSuccess:
                _animator.OnCriticalSuccess();
                return;
            case DiceResult.CriticalFailure:
                _animator.OnCriticalFailure();
                return;
            case DiceResult.Success:
                _animator.OnSuccess();
                return;
            case DiceResult.Failure:
                Result = DiceResult.Failure;
                _animator.OnFailure();
                break;
        }
    }
}
