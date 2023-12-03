using System.Collections.Generic;
using UnityEngine;

public class DiceD20 : Dice
{
    protected override int MAX_VALUE => 20;

    protected override IDictionary<int, Quaternion> GetQuaternions()
    {
        const float PI_OVER_5 = Mathf.PI / 5;
        float innerRadius = 1 / (2 * Mathf.Sin(PI_OVER_5));
        float innerRadiusSqr = innerRadius * innerRadius;

        float h3 = 1 / (2 * Mathf.Sqrt(1 - innerRadiusSqr));
        float h2 = 1 / (2 * Mathf.Sqrt(1 - innerRadiusSqr)) -
            Mathf.Sqrt(1 - innerRadiusSqr);
        float h1 = Mathf.Sqrt(1 - innerRadiusSqr)
            - 1 / (2 * Mathf.Sqrt(1 - innerRadiusSqr));
        float h0 = -1 / (2 * Mathf.Sqrt(1 - innerRadiusSqr));

        Vector3[] lowLevelPoints = new Vector3[5];
        Vector3[] highLevelPoints = new Vector3[5];

        for (int i = 0; i < 5; i++)
        {
            lowLevelPoints[i] = new Vector3(innerRadius * Mathf.Cos((1 + 2 * i) * PI_OVER_5), h1,
                                            innerRadius * Mathf.Sin((1 + 2 * i) * PI_OVER_5));
            highLevelPoints[i] = new Vector3(innerRadius * Mathf.Cos((0 + 2 * i) * PI_OVER_5), h2,
                                             innerRadius * Mathf.Sin((0 + 2 * i) * PI_OVER_5));
        }

        Vector3 A = new(0, h0, 0);
        Vector3 B = lowLevelPoints[0];
        Vector3 C = lowLevelPoints[1];
        Vector3 D = lowLevelPoints[2];
        Vector3 E = lowLevelPoints[3];
        Vector3 F = lowLevelPoints[4];
        Vector3 G = highLevelPoints[0];
        Vector3 H = highLevelPoints[1];
        Vector3 I = highLevelPoints[2];
        Vector3 J = highLevelPoints[3];
        Vector3 K = highLevelPoints[4];
        Vector3 L = new(0, h3, 0);

        Vector3[] normals = new Vector3[21];
        Vector3[] axes = new Vector3[21];

        normals[1] = new Plane(H, C, B).normal;
        axes[1] = B - H;
        normals[2] = new Plane(K, F, E).normal;
        axes[2] = E - K;
        normals[3] = new Plane(H, L, I).normal;
        axes[3] = I - H;
        normals[4] = new Plane(E, A, D).normal;
        axes[4] = D - E;
        normals[5] = new Plane(A, F, B).normal;
        axes[5] = B - A;
        normals[6] = new Plane(D, I, J).normal;
        axes[6] = J - D;
        normals[7] = new Plane(B, G, H).normal;
        axes[7] = H - B;
        normals[8] = new Plane(J, L, K).normal;
        axes[8] = K - J;
        normals[9] = new Plane(I, D, C).normal;
        axes[9] = C - I;
        normals[10] = new Plane(L, G, K).normal;
        axes[10] = K - L;
        normals[11] = new Plane(C, D, A).normal;
        axes[11] = A - C;
        normals[12] = new Plane(K, G, F).normal;
        axes[12] = F - K;
        normals[13] = new Plane(C, A, B).normal;
        axes[13] = B - C;
        normals[14] = new Plane(E, D, J).normal;
        axes[14] = J - E;
        normals[15] = new Plane(B, F, G).normal;
        axes[15] = G - B;
        normals[16] = new Plane(J, I, L).normal;
        axes[16] = L - J;
        normals[17] = new Plane(G, L, H).normal;
        axes[17] = H - G;
        normals[18] = new Plane(F, A, E).normal;
        axes[18] = E - F;
        normals[19] = new Plane(H, I, C).normal;
        axes[19] = C - H;
        normals[20] = new Plane(K, E, J).normal;
        axes[20] = J - K;

        IDictionary<int, Quaternion> answer = new Dictionary<int, Quaternion>();

        for (int i = 1; i <= 20; i++)
        {
            Quaternion mainRotation = Quaternion.FromToRotation(normals[i], Vector3.back);
            Vector3 rotatedAxis = mainRotation * axes[i];
            Quaternion additionalRotation = Quaternion.FromToRotation(rotatedAxis, Vector3.right);

            answer[i] = additionalRotation * mainRotation;
        }

        return answer;
    }
}
