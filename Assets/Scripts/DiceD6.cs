using System.Collections.Generic;
using UnityEngine;

public class DiceD6 : Dice
{
    protected override int MAX_VALUE => 6;

    protected override IDictionary<int, Quaternion> GetQuaternions()
    {
        IDictionary<int, Quaternion> answer = new Dictionary<int, Quaternion>
        {
            [1] = Quaternion.Euler(180, 0, 0),
            [2] = Quaternion.Euler(90, 180, 0),
            [3] = Quaternion.Euler(90, 0, 90),
            [4] = Quaternion.Euler(0, 90, 0),
            [5] = Quaternion.Euler(90, 0, 0),
            [6] = Quaternion.identity
        };

        return answer;
    }
}
