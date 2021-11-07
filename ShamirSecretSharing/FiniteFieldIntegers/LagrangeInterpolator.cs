using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShamirSecretSharing.FiniteFieldIntegers
{
    public static class LagrangeInterpolator
    {
        /// <summary>
        /// Uses the "Lagrange Interpolation" method detailed here (https://en.wikipedia.org/wiki/Shamir%27s_Secret_Sharing#Python_example)
        /// </summary>
        /// <param name="xVals"></param>
        /// <param name="yVals"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static FiniteFieldInteger SimpleInterpolate(IEnumerable<FiniteFieldInteger> xVals, IEnumerable<FiniteFieldInteger> yVals, int size)
        {
            var numerators = new FiniteFieldInteger[size];
            var denominators = new FiniteFieldInteger[size];

            int index = 0;

            foreach (var xVal in xVals)
            {
                var others = xVals.ToList();
                others.RemoveAt(index);
                numerators[index] = others.Select(o => o.GetAdditiveIdentity() - o).GetProduct();
                denominators[index] = others.Select(o => xVal - o).GetProduct();
                if (++index >= size)
                    break;
            }
            var denominator = denominators.GetProduct();
            var numerator = denominator.GetAdditiveIdentity();
            index = 0;
            foreach (var yVal in yVals)
            {
                numerator += numerators[index] * denominator * yVal / denominators[index];
                if (++index >= size)
                    break;
            }

            return numerator / denominator;
        }

        /// <summary>
        /// An interpolation algorithm that better shows the O(n²) runtime of the algorithm
        /// </summary>
        /// <param name="xVals"></param>
        /// <param name="yVals"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static FiniteFieldInteger StreamLinedInterpolate(FiniteFieldInteger[] xVals, FiniteFieldInteger[] yVals, int size)
        {
            var numerators = new FiniteFieldInteger[size];
            var denominators = new FiniteFieldInteger[size];

            for(int i = 0; i < size; i++)
            {
                var currentX = xVals[i];
                denominators[i] = currentX.GetMultiplicativeIdentity();
                numerators[i] = currentX.GetMultiplicativeIdentity();
                for(int j = 0; j < size; j++)
                {
                    if (i == j)
                        continue;
                    var otherX = xVals[j];
                    denominators[i] *= currentX - otherX;
                    numerators[i] *= otherX.GetAdditiveInverse();
                }
                numerators[i] *= yVals[i] / denominators[i];
            }

            var denominator = denominators.GetProduct();
            var numerator = denominator.GetAdditiveIdentity();
            for(int i = 0; i < size; i++)
            {
                numerator += numerators[i] * denominator;
            }

            return numerator / denominator;
        }

        /// <summary>
        /// A more efficient interpolation algorithm that can calculate denominators in O(n log n) and numerators in O(n)
        /// </summary>
        /// <param name="xVals"></param>
        /// <param name="yVals"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static FiniteFieldInteger EfficientInterpolate(FiniteFieldInteger[] xVals, FiniteFieldInteger[] yVals, int size)
        {
            var numerators = new FiniteFieldInteger[size];
            var denominators = new FiniteFieldInteger[size];
            FiniteFieldInteger tempNumerator = xVals[0].GetMultiplicativeIdentity();

            for (int i = 0; i < size; i++)
            {
                var currentX = xVals[i];
                for (int j = i; j < size; j++)
                {
                    var otherX = xVals[j];
                    if (i == 0) 
                    {
                        denominators[j] = currentX.GetMultiplicativeIdentity();
                        tempNumerator *= otherX.GetAdditiveInverse();
                    }
                    if (i == j)
                        continue;                    
                    var difference = currentX - otherX;
                    denominators[i] *= difference;
                    denominators[j] *= difference.GetAdditiveInverse();
                }
                numerators[i] = tempNumerator / currentX.GetAdditiveInverse();
            }

            var denominator = denominators.GetProduct();
            var numerator = denominator.GetAdditiveIdentity();
            for (int i = 0; i < size; i++)
            {
                numerator += numerators[i] * denominator * yVals[i] / denominators[i];
            }

            return numerator / denominator;
        }
    }
}
