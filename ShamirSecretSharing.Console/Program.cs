using ShamirSecretSharing.SecretSharePoints;
using System;
using System.Collections.Generic;

namespace ShamirSecretSharing.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var secrets1 = new List<SecretSharePoint>
            {
                new SecretSharePoint(1, 3, 7, 31),
                new SecretSharePoint(2, 14, 7, 31),
                new SecretSharePoint(3, 28, 7, 31),
                new SecretSharePoint(5, 13, 7, 31),
                new SecretSharePoint(7, 25, 7, 31),
                new SecretSharePoint(9, 28, 7, 31),
                new SecretSharePoint(10, 31, 7, 31),
            };

            var result1 = secrets1.Solve();

            var secrets2 = new List<SecretSharePoint>
            {
                new SecretSharePoint(43, 91, 3, 97),
                new SecretSharePoint(67, 7, 3, 97),
                new SecretSharePoint(96, 33, 3, 97),
            };

            var result2 = secrets2.Solve();
        }
    }
}
