using UnityEngine;

[CreateAssetMenu]
public class DiceModificatorData : ScriptableObject, IDiceModificatorData
{
    [SerializeField] private int _value;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;

    public int Value => _value;
    public Sprite Sprite => _sprite;
    public string Name => _name;
}
