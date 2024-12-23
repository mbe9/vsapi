using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vintagestory.API.MathTools
{
    // Since C# does not support proper templates or const generics, this is essentially
    // the only way to implement this sort of struct
    //
    // Should you need Dual numbers with different amount of differentials, just copy the
    // whole structure and change the Count constant to the desired value
    public unsafe struct DualNum6
    {
        const int Count = 6;

        public double value;
        public fixed double diffs[Count];

        // DualNum() {}

        public DualNum6(double value)
        {
            this.value = value;

            for (int i = 0; i < Count; i++)
            {
                this.diffs[i] = 0;
            }
        }

        public DualNum6(double value, double[] diffs)
        {
            this.value = value;

            for (int i = 0; i < Count && i < diffs.Length; i++)
            {
                this.diffs[i] = diffs[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 Max(DualNum6 a, DualNum6 b)
        {
            if (a.value > b.value)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 Min(DualNum6 a, DualNum6 b)
        {
            if (a.value < b.value)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 Max(DualNum6 a, DualNum6 b, DualNum6 c)
        {
            if ((a.value > b.value) && (a.value > c.value))
            {
                return a;
            }
            else if ((b.value > a.value) && (b.value > c.value))
            {
                return b;
            }
            else
            {
                return c;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 Min(DualNum6 a, DualNum6 b, DualNum6 c)
        {
            if ((a.value < b.value) && (a.value < c.value))
            {
                return a;
            }
            else if ((b.value < a.value) && (b.value < c.value))
            {
                return b;
            }
            else
            {
                return c;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DualNum6 Abs()
        {
            double sign = Math.Sign(value);

            DualNum6 res;
            res.value = value * sign;

            for (int i = 0; i < Count; i++)
            {
                res.diffs[i] = diffs[i] * sign;
            }

            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DualNum6 Sqrt()
        {
            double sqrt = Math.Sqrt(value);
            double inv_sqrt = 1.0 / Math.Sqrt(value);

            DualNum6 res;
            res.value = sqrt;

            for (int i = 0; i < Count; i++)
            {
                res.diffs[i] = diffs[i] * inv_sqrt * 0.5;
            }

            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DualNum6 Pow(double power)
        {
            double pow = Math.Pow(value, power - 1.0);

            DualNum6 res;
            res.value = pow * value;

            for (int i = 0; i < Count; i++)
            {
                res.diffs[i] = diffs[i] * power * pow;
            }

            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 operator +(DualNum6 left, DualNum6 right)
        {
            DualNum6 res;
            res.value = left.value + right.value;

            for (int i = 0; i < Count; i++)
            {
                res.diffs[i] = left.diffs[i] + right.diffs[i];
            }

            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 operator +(DualNum6 left, double right)
        {
            DualNum6 res = left;
            res.value += right;

            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 operator +(double left, DualNum6 right)
        {
            DualNum6 res = right;
            res.value += left;

            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 operator -(DualNum6 left, DualNum6 right)
        {
            DualNum6 res;
            res.value = left.value - right.value;

            for (int i = 0; i < Count; i++)
            {
                res.diffs[i] = left.diffs[i] - right.diffs[i];
            }

            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 operator -(DualNum6 left, double right)
        {
            DualNum6 res = left;
            res.value -= right;

            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 operator -(double left, DualNum6 right)
        {
            DualNum6 res;
            res.value = left - right.value;

            for (int i = 0; i < Count; i++)
            {
                res.diffs[i] = -right.diffs[i];
            }

            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 operator *(DualNum6 left, DualNum6 right)
        {
            DualNum6 res;
            res.value = left.value * right.value;

            for (int i = 0; i < Count; i++)
            {
                res.diffs[i] = left.diffs[i] * right.value + left.value * right.diffs[i];
            }

            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 operator *(DualNum6 left, double right)
        {
            DualNum6 res;
            res.value = left.value * right;

            for (int i = 0; i < Count; i++)
            {
                res.diffs[i] = left.diffs[i] * right;
            }

            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 operator /(DualNum6 left, DualNum6 right)
        {
            DualNum6 res;
            res.value = left.value / right.value;

            for (int i = 0; i < Count; i++)
            {
                res.diffs[i] = ((left.diffs[i] * right.value) - (left.value * right.diffs[i])) / (right.value * right.value);
            }

            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 operator /(DualNum6 left, double right)
        {
            DualNum6 res;
            res.value = left.value / right;

            for (int i = 0; i < Count; i++)
            {
                res.diffs[i] = left.diffs[i] / right;
            }

            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DualNum6 operator -(DualNum6 num)
        {
            DualNum6 res;
            res.value = -num.value;

            for (int i = 0; i < Count; i++)
            {
                res.diffs[i] = -num.diffs[i];
            }

            return res;
        }

        public double this[int index]
        {
            get { return diffs[index]; }
            set { diffs[index] = value; }
        }

        public static bool operator <(DualNum6 left, double right)
        {
            return left.value < right;
        }

        public static bool operator >(DualNum6 left, double right)
        {
            return left.value > right;
        }

        public static bool operator ==(DualNum6 left, double right)
        {
            return left.value == right;
        }

        public static bool operator !=(DualNum6 left, double right)
        {
            return left.value != right;
        }


        public static bool operator <(DualNum6 left, DualNum6 right)
        {
            return left.value < right.value;
        }

        public static bool operator >(DualNum6 left, DualNum6 right)
        {
            return left.value > right.value;
        }

        public static bool operator ==(DualNum6 left, DualNum6 right)
        {
            return left.value == right.value;
        }

        public static bool operator !=(DualNum6 left, DualNum6 right)
        {
            return left.value != right.value;
        }
    }
}
