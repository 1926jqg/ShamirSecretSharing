using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShamirSecretSharing.FiniteFieldIntegers
{
    public static class FiniteFieldIntegerExtensions
    {
        public static FiniteFieldInteger GetProduct(this IEnumerable<FiniteFieldInteger> integers)
        {
            if (!integers.Any())
                throw new Exception("Cannot get Finite Field Integer product of an empty list.");
            var exemplar = integers.First();
            return integers.Aggregate(exemplar.GetMultiplicativeIdentity(), (x, y) => x * y);
        }
    }
}
