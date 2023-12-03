using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ResultAnimator : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnSuccess() => _animator.SetTrigger("Success");
    public void OnFailure() => _animator.SetTrigger("Failure");
    public void OnCriticalSuccess() => _animator.SetTrigger("CriticalSuccess");

    public void Hide() => _animator.SetTrigger("Hide");
}
