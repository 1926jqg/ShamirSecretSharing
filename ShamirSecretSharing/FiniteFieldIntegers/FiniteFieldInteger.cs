using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace ShamirSecretSharing.FiniteFieldIntegers
{
    public struct FiniteFieldInteger : IEquatable<FiniteFieldInteger>
    {
        private readonly BigInteger _value;

        public int Value 
        { 
            get
            {
                return (int)_value;
            }
        }
        public int Mod { get; }

        private FiniteFieldInteger(BigInteger value, int mod)
        {
            _value = value % mod;
            Mod = mod;
        }

        public FiniteFieldInteger(int value, int mod)
            : this(new BigInteger(value), mod) { }

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
            var inverse = (int)BigInteger.ModPow(_value, Mod - 2, Mod);
            return new FiniteFieldInteger(inverse, Mod);
        }

        public FiniteFieldInteger GetAdditiveInverse()
        {
            var inverse = Mod - _value;
            return new FiniteFieldInteger(inverse, Mod);
        }

        public bool IsCompatible(FiniteFieldInteger other)
        {
            return Mod == other.Mod;
        }

        public FiniteFieldInteger Pow(int exponent)
        {
            return new FiniteFieldInteger((int)BigInteger.ModPow(_value, exponent, Mod), Mod);
        }

        public override string ToString()
        {
            return $"{_value} (mod {Mod})";
        }

        public bool Equals([AllowNull] FiniteFieldInteger other)
        {
            if (!IsCompatible(other))
                throw new FiniteFieldCompatibilityException(this, other);
            return _value == other._value;
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
            return HashCode.Combine(_value, Mod);
        }

        public static FiniteFieldInteger operator +(FiniteFieldInteger a, FiniteFieldInteger b)
        {
            if (!a.IsCompatible(b))
                throw new FiniteFieldCompatibilityException(a, b);
            return new FiniteFieldInteger(a._value + b._value, a.Mod);
        }
        public static FiniteFieldInteger operator -(FiniteFieldInteger a, FiniteFieldInteger b)
        {
            return a + b.GetAdditiveInverse();
        }
        public static FiniteFieldInteger operator *(FiniteFieldInteger a, FiniteFieldInteger b)
        {
            if (!a.IsCompatible(b))
                throw new FiniteFieldCompatibilityException(a, b);
            return new FiniteFieldInteger(a._value * b._value, a.Mod);
        }
        public static FiniteFieldInteger operator /(FiniteFieldInteger a, FiniteFieldInteger b)
        {        
            return a * b.GetMultiplicativeInverse();
        }

        public static bool operator ==(FiniteFieldInteger a, FiniteFieldInteger b)
        {
            if (!a.IsCompatible(b))
                throw new FiniteFieldCompatibilityException(a, b);
            return a.Value == b.Value;
        }

        public static bool operator !=(FiniteFieldInteger a, FiniteFieldInteger b)
        {
            if (!a.IsCompatible(b))
                throw new FiniteFieldCompatibilityException(a, b);
            return a.Value != b.Value;
        }
    }
}
