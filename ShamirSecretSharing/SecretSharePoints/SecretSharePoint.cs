using ShamirSecretSharing.FiniteFieldIntegers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamirSecretSharing.SecretSharePoints
{
    public struct SecretSharePoint
    {        
        internal FiniteFieldInteger X { get; }
        internal FiniteFieldInteger Y { get; }
        public int Quorum { get; }

        public SecretSharePoint(int x, int y, int quorum, int mod)
        {
            X = new FiniteFieldInteger(x, mod);
            Y = new FiniteFieldInteger(y, mod);
            Quorum = quorum;
        }

        public SecretSharePoint(FiniteFieldInteger x, FiniteFieldInteger y, int quorum)
        {
            if (!x.IsCompatible(y))
                throw new FiniteFieldCompatibilityException(x, y);
            X = x;
            Y = y;
            Quorum = quorum;
        }

        public bool IsCompatible(SecretSharePoint other)
        {
            return X.IsCompatible(other.X) && Quorum == other.Quorum;
        }

        public bool CanBeAdded(SecretSharePoint other)
        {
            return IsCompatible(other) && X == other.X;
        }

        public int GetModulus()
        {
            return X.Mod;
        }

        public override string ToString()
        {
            return $"f({X}) = {Y}";
        }

        public static SecretSharePoint operator +(SecretSharePoint a, SecretSharePoint b)
        {
            if (!a.CanBeAdded(b))
                throw new Exception($"The points \"{a}\" and \"{b}\" cannot be added");
            return new SecretSharePoint(a.X,a.Y + b.Y, a.Quorum);
        }
    }
}
