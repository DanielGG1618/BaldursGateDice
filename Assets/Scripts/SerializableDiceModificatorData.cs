using System;
using UnityEngine;

[Serializable]
public class SerializableDiceModificatorData : IDiceModificatorData
{
    public int Value => _value;

    public Sprite Sprite => _sprite;

    public string Name => _name;

    [SerializeField] private int _value = 0;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;
}