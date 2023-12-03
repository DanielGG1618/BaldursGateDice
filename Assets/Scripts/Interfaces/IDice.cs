public interface IDice
{
    public UnityEngine.Vector3 WorldPosition { get; }

    public bool HasCriticalySucceed { get; }
    public bool HasCriticalyFailed { get; }

    public event System.Action MouseDown;
    public event System.Action Rolled;
    public event System.Action<int> ResultChanged;
    public event System.Action ModificationAnimationEnded;

    public void RollInDirection(UnityEngine.Vector2 direction, int numberOfBumps);

    public void ModifyResult(int modification);

    public void PlayModificationAnimationInDirection(UnityEngine.Vector2 direction);

    public void RotateToFace(int face, float duration);

    public void TeleportToFace(int face);

    public void Destroy();
}
