using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class DiceModificator : MonoBehaviour, IDiceModificator
{
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _nameText;

    private CanvasGroup _canvasGroup;

    private Transform _valueTextDuplicate;
    
    private IDiceModificatorData _data;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(IDiceModificatorData modificatorData)
    {
        _data = modificatorData;

        _valueText.text = (_data.Value > 0 ? "+" : "") + _data.Value;
        _valueTextDuplicate = Instantiate(_valueText, _valueText.transform.parent).transform;
        _image.sprite = _data.Sprite;
        _nameText.text = _data.Name;
    }

    public void ApplyTo(IDice dice)
    {
        dice.ModifyResult(_data.Value);

        DOTween.Sequence()
            .Append(_valueTextDuplicate.DOScale(3 * _valueTextDuplicate.localScale, 0.3f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine))
            .Append(_valueTextDuplicate.DOMove(dice.WorldPosition, 0.3f).SetEase(Ease.OutQuad))
            .Join(_valueTextDuplicate.DOScale(1.5f * _valueTextDuplicate.localScale, 0.3f))
            .AppendCallback(() => dice.PlayModificationAnimationInDirection(dice.WorldPosition - transform.position))
            .Append(_canvasGroup.DOFade(0.67f, 0.3f));
    }
}
