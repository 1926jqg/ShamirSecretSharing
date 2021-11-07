using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace ShamirSecretSharing.FiniteFieldIntegers
{
    public struct FiniteFieldInteger : IEquatable<FiniteFieldInteger>
    {
        public int Value { get; }
        public int Mod { get; }

        public FiniteFieldInteger(int value, int mod)
        {
            Value = value % mod;
            Mod = mod;
        }

        public FiniteFieldInteger GetAdditiveIdentity()
        {
            return new FiniteFieldInteger(0, Mod);
        }

        public FiniteFieldInteger GetMultiplicativeIdentity()
        {
            return new FiniteFieldInteger(1, Mod);
        }

        public FiniteFieldInteger GetMultiplicativeInverse()
        {
            var inverse = (int)BigInteger.ModPow(Value, Mod - 2, Mod);
            return new FiniteFieldInteger(inverse, Mod);
        }

        public FiniteFieldInteger GetAdditiveInverse()
        {
            var inverse = Mod - Value;
            return new FiniteFieldInteger(inverse, Mod);
        }

        public bool IsCompatible(FiniteFieldInteger other)
        {
            return Mod == other.Mod;
        }

        public override string ToString()
        {
            return $"{Value} (mod {Mod})";
        }

        public bool Equals([AllowNull] FiniteFieldInteger other)
        {
            if (!IsCompatible(other))
                throw new FiniteFieldCompatibilityException(this, other);
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if(obj is FiniteFieldInteger other)
            {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Mod);
        }

        public static FiniteFieldInteger operator +(FiniteFieldInteger a, FiniteFieldInteger b)
        {
            if (!a.IsCompatible(b))
                throw new FiniteFieldCompatibilityException(a, b);
            return new FiniteFieldInteger(a.Value + b.Value, a.Mod);
        }
        public static FiniteFieldInteger operator -(FiniteFieldInteger a, FiniteFieldInteger b)
        {
            return a + b.GetAdditiveInverse();
        }
        public static FiniteFieldInteger operator *(FiniteFieldInteger a, FiniteFieldInteger b)
        {
            if (!a.IsCompatible(b))
                throw new FiniteFieldCompatibilityException(a, b);
            return new FiniteFieldInteger(a.Value * b.Value, a.Mod);
        }
        public static FiniteFieldInteger operator /(FiniteFieldInteger a, FiniteFieldInteger b)
        {        
            return a * b.GetMultiplicativeInverse();
        }
    }
}
