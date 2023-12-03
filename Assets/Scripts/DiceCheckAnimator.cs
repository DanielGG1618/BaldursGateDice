using UnityEngine;

[RequireComponent (typeof(Animator))]
public class DiceCheckAnimator : MonoBehaviour, IDiceCheckAnimator
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Show()
    {
        _animator.SetTrigger("Show");
    }

    public void Hide()
    {
        _animator.SetTrigger("Hide");
    }

    public void OnRoll()
    {
        _animator.SetTrigger("Roll");
    }

    public void OnCriticalFailure()
    {
        _animator.SetTrigger("CriticalFailure");
    }

    public void OnFailure()
    {
        _animator.SetTrigger("Failure");
    }

    public void OnSuccess()
    {
        _animator.SetTrigger("Success");
    }

    public void OnCriticalSuccess()
    {
        _animator.SetTrigger("CriticalSuccess");
    }
}