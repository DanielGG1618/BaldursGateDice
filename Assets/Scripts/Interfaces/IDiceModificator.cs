public interface IDiceModificator
{
    public void Init(IDiceModificatorData modificatorData);

    public void ApplyTo(IDice dice);
}
