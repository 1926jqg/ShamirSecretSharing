using ShamirSecretSharing.FiniteFieldIntegers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShamirSecretSharing.SecretSharePoints
{
    public static class SecretSharePointExtensions
    {
        public static int Solve(this IEnumerable<SecretSharePoint> points)
        {
            if (points == null || !points.Any())
                throw new Exception("The list of points was empty, could not solve");
            if (!points.AreCompatible())
                throw new Exception("Not all secrets had the same mod and quorum");

            var exemplar = points.FirstOrDefault();
            var quorum = exemplar.Quorum;

            var xVals = points.Select(p => p.X);
            var yVals = points.Select(p => p.Y);

            var result = LagrangeInterpolator.EfficientInterpolate(xVals.ToArray(), yVals.ToArray(), quorum);

            return result.Value;
        }

        public static bool AreCompatible(this IEnumerable<SecretSharePoint> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            using (var enumerator = enumerable.GetEnumerator())
            {
                
                if (!enumerator.MoveNext())
                    return false;

                var toCompare = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    if (!toCompare.IsCompatible(enumerator.Current))
                    {
                        return false;
                    }
                }
            }
            return true;
        }        
    }
}
