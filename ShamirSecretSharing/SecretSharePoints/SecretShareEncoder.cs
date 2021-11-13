using ShamirSecretSharing.FiniteFieldIntegers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShamirSecretSharing.SecretSharePoints
{
    public class SecretShareEncoder
    {
        private readonly Random _random;

        public SecretShareEncoder()
        {
            _random = new Random();
        }

        public IEnumerable<SecretSharePoint> Encode(int secret, int quorum, int mod, IEnumerable<int> inputs)
        {
            var coefficients = new FiniteFieldInteger[quorum];
            coefficients[0] = new FiniteFieldInteger(secret, mod);
            for (int i = 1; i < quorum; i++)
            {
                coefficients[i] = new FiniteFieldInteger(_random.Next(1, mod), mod);
            }
            foreach (var input in inputs)
            {
                yield return SolveFunction(new FiniteFieldInteger(input, mod), coefficients, quorum);
            }
        }

        public IEnumerable<SecretSharePoint> Encode(int secret, int quorum, int mod, int shares)
        {            
            var inputs = Enumerable
                .Range(1, mod)
                .OrderBy(o => _random.Next())
                .Take(shares);
            return Encode(secret, quorum, mod, inputs);
        }

        private static SecretSharePoint SolveFunction(FiniteFieldInteger input, FiniteFieldInteger[] coefficients, int quorum)
        {
            var output = input.GetAdditiveIdentity();
            for(int i = 0; i < quorum; i++)
            {
                output += coefficients[i] * input.Pow(i);
            }
            return new SecretSharePoint(input, output, quorum);
        }

        public IEnumerable<SecretSharePoint> Encode(int secret, int quorum, int mod)
        {
            return Encode(secret, quorum, mod, quorum);
        }
    }
}
