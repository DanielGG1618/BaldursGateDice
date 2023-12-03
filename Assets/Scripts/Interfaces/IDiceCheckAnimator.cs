public interface IDiceCheckAnimator
{
    public void Show();
    public void Hide();

    public void OnRoll();

    public void OnCriticalFailure();
    public void OnFailure();
    public void OnSuccess();
    public void OnCriticalSuccess();
}