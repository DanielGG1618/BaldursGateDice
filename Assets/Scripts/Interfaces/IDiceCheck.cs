using System.Collections.Generic;

public interface IDiceCheck
{
    public DiceResult Result { get; }

    public void Init(string title, string subtitle, int difficulty, Dice dicePrefab, IEnumerable<IDiceModificatorData> modificators);

    public void Show();

    public void Hide();
}
