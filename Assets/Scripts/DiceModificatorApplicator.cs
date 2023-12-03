using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceModificatorApplicator : MonoBehaviour, IDiceModificatorApplicator
{
    [SerializeField] private DiceModificator _modificatorPrefab;

    private List<IDiceModificator> _modificators = new();

    private bool _isStopped = false;
    private int _currentModificatorIndex = 0;

    public int TotalBonus { get; private set; }

    public event Action LastModificationApplied;

    public void Init(IEnumerable<IDiceModificatorData> modificatorDatas)
    {
        _isStopped = false;
        _currentModificatorIndex = 0;

        _modificators.Clear();
        foreach (Transform oldModificator in transform)
        {
            Destroy(oldModificator.gameObject);
        }

        TotalBonus = 0;

        foreach (IDiceModificatorData modificatorData in modificatorDatas)
        {
            IDiceModificator modificator = Instantiate(_modificatorPrefab, transform);
            modificator.Init(modificatorData);
            _modificators.Add(modificator);

            TotalBonus += modificatorData.Value;
        }
    }

    public void ApplyTo(IDice dice)
    {
        _currentModificatorIndex = 0;

        ApplyNextModificatorTo(dice);
        
        dice.ModificationAnimationEnded += () => ApplyNextModificatorTo(dice);
    }

    private void ApplyNextModificatorTo(IDice dice)
    {
        if (_isStopped)
        {
            return;
        }

        if (_currentModificatorIndex >= _modificators.Count) 
        {
            LastModificationApplied?.Invoke();
            return;
        }

        IDiceModificator nextModificator = _modificators[_currentModificatorIndex];
        _currentModificatorIndex++;

        nextModificator.ApplyTo(dice);
    }

    public void Stop()
    {
        _isStopped = true;
    }
}
