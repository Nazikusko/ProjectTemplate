using System;
using UnityEngine;

namespace Unitls
{
    public static class FloatUtils
    {
        public const float Epsilon = 0.001f;

        public static bool AlmostEqualAbs(float a, float b, float absEpsilon = Epsilon)
        {
            return Mathf.Abs(a - b) <= absEpsilon;
        }

        public static bool AlmostEqualRelative(float a, float b, float relEpsilon = 1e-5f, float absEpsilon = 1e-8f)
        {
            float diff = Mathf.Abs(a - b);
            if (diff <= absEpsilon) return true; // очень близко к нулю

            float largest = Mathf.Max(Mathf.Abs(a), Mathf.Abs(b));
            return diff <= largest * relEpsilon;
        }


        public static bool AlmostEqualULP(float a, float b, int maxUlps = 4)
        {
            // Быстрое равенство и NaN/Infinity проверки
            if (a == b) return true;
            if (float.IsNaN(a) || float.IsNaN(b)) return false;
            if (float.IsInfinity(a) || float.IsInfinity(b)) return false;

            // Представим биты как int
            int ia = BitConverter.ToInt32(BitConverter.GetBytes(a), 0);
            int ib = BitConverter.ToInt32(BitConverter.GetBytes(b), 0);

            // Сделать порядок корректным для отрицательных чисел
            if (ia < 0) ia = int.MinValue - ia;
            if (ib < 0) ib = int.MinValue - ib;

            int intDiff = Math.Abs(ia - ib);
            return intDiff <= maxUlps;
        }

        public static bool EqualsWithPrecision(float a, float b, float relEpsilon = 1e-5f, float absEpsilon = 1e-8f)
        {
            return AlmostEqualRelative(a, b, relEpsilon, absEpsilon);
        }
    }
}