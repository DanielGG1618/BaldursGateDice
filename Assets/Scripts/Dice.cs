using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dice : MonoBehaviour, IDice
{
    protected abstract int MAX_VALUE { get; }

    [SerializeField] private Vector2 _bounds;

    private bool _rolled = false;
    private int _currentFace;

    private IDictionary<int, Quaternion> _rotaionsForFaces;

    public Vector3 WorldPosition => transform.position;
    public bool HasCriticalySucceed => _currentFace == MAX_VALUE;
    public bool HasCriticalyFailed => _currentFace == 1;

    public event Action MouseDown;
    public event Action Rolled;
    public event Action<int> ResultChanged;
    public event Action ModificationAnimationEnded;

    private void Awake()
    {
        _rotaionsForFaces = GetQuaternions();

        _currentFace = MAX_VALUE;
        TeleportToFace(_currentFace);
    }

    private void OnMouseDown()
    {
        MouseDown?.Invoke();
    }

    public void RollInDirection(Vector2 direction, int numberOfBumps)
    {
        if (_rolled) return;

        _rolled = true;

        StartCoroutine(RollInDirectionCoroutine(direction, numberOfBumps));
    }

    private IEnumerator RollInDirectionCoroutine(Vector2 initialDirection, int numberOfBumps)
    {
        _currentFace = UnityEngine.Random.Range(1, MAX_VALUE + 1);
        ResultChanged?.Invoke(_currentFace);

        float bumpVelocityLoseCoefficient = 0.7f;

        float rotationSpeed = 1000;
        Vector2 velocity = 2500 * initialDirection.normalized;
        Vector2 rotationAxis = new(velocity.y, -velocity.x);

        for (int i = 1; i < numberOfBumps; i++)
            yield return RollTillBump();

        yield return RollBackToOrigin();

        OnRolled();

        //------------------------\\

        IEnumerator RollTillBump()
        {
            while (true)
            {
                Vector2 newPosition = (Vector2)transform.localPosition + velocity * Time.deltaTime;

                bool bumped = false;

                if (Mathf.Abs(newPosition.x) > _bounds.x)
                {
                    bumped = true;

                    newPosition.x = (int)Mathf.Sign(newPosition.x) * _bounds.x;

                    velocity = bumpVelocityLoseCoefficient * new Vector2(-velocity.x, velocity.y);
                    rotationAxis = new(velocity.y, -velocity.x);
                }

                if (Mathf.Abs(newPosition.y) > _bounds.y)
                {
                    bumped = true;

                    newPosition.y = (int)Mathf.Sign(newPosition.y) * _bounds.y;

                    velocity = bumpVelocityLoseCoefficient * new Vector2(velocity.x, -velocity.y);
                    rotationAxis = new(velocity.y, -velocity.x);
                }

                transform.localPosition = newPosition;
                transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.World);

                yield return null;

                if (bumped) break;
            }
        }

        IEnumerator RollBackToOrigin()
        {
            float previousFrameSqrDistanceToOrigin = transform.localPosition.sqrMagnitude;

            velocity = -bumpVelocityLoseCoefficient * velocity.magnitude * transform.localPosition.normalized;
            rotationAxis = new(velocity.y, -velocity.x);

            while (true)
            {
                transform.localPosition += (Vector3)(velocity * Time.deltaTime);
                transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.World);

                float currentSqrDistanceToOrigin = transform.localPosition.sqrMagnitude;

                if (currentSqrDistanceToOrigin < previousFrameSqrDistanceToOrigin)
                {
                    previousFrameSqrDistanceToOrigin = currentSqrDistanceToOrigin;
                    yield return null;
                    continue;
                }

                float rotationDuration = 0.15f;
                yield return transform.DORotateQuaternion(_rotaionsForFaces[_currentFace], rotationDuration).SetEase(Ease.OutSine).WaitForCompletion();
                transform.localPosition = Vector3.zero;

                break;
            }
        }
    }

    private void OnRolled()
    {
        Rolled?.Invoke();
    }

    public void ModifyResult(int modification)
    {
        _currentFace += modification;
        _currentFace = Mathf.Clamp(_currentFace, 1, MAX_VALUE);

        ResultChanged?.Invoke(_currentFace);
    }

    public void PlayModificationAnimationInDirection(Vector2 direction)
    {
        float duration = 0.4f;

        float highlightDuration = duration / 3;
        float pauseDuration = duration / 6;
        float dehighlightDuration = duration - pauseDuration - highlightDuration;

        Vector2 initialPosition = transform.localPosition;
        Vector2 highlightPosition = initialPosition + 25 * direction.normalized;

        Vector3 initialScale = transform.localScale;
        Vector3 highlightScale = 1.1f * initialScale;

        DOTween.Sequence()
            .Append(transform.DOLocalMove(highlightPosition, highlightDuration).SetEase(Ease.OutCubic))
            .Join(transform.DOScale(highlightScale, highlightDuration).SetEase(Ease.OutCubic))
            .Join(transform.DORotateQuaternion(_rotaionsForFaces[_currentFace], 0.8f * highlightDuration).SetEase(Ease.OutSine))
            .AppendInterval(pauseDuration)
            .Append(transform.DOLocalMove(initialPosition, dehighlightDuration).SetEase(Ease.InCubic))
            .Join(transform.DOScale(initialScale, dehighlightDuration).SetEase(Ease.InCubic))
            .OnComplete(() => {
                ModificationAnimationEnded?.Invoke();
            });
    }

    public void RotateToFace(int face, float duration)
    {
        transform.DORotateQuaternion(_rotaionsForFaces[face], duration).SetEase(Ease.OutBack);
    }
    
    public void TeleportToFace(int face)
    {
        transform.rotation = _rotaionsForFaces[face];
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    protected abstract IDictionary<int, Quaternion> GetQuaternions();
}
