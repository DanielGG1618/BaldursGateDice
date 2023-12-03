using System.Collections.Generic;

public interface IDiceModificatorApplicator
{
    public int TotalBonus { get; }

    public event System.Action LastModificationApplied;

    public void Init(IEnumerable<IDiceModificatorData> modificatorDatas);

    public void ApplyTo(IDice dice);

    public void Stop();
}
