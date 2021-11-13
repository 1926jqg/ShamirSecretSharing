using ShamirSecretSharing.SecretSharePoints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

            var encoder = new SecretShareEncoder();

            var secrets3 = encoder.Encode(15, 3, 3251).ToList();
            var result3 = secrets3.Solve();

            var secrets4 = encoder.Encode(314, 4, 2633, 8).ToList();
            var results4a = secrets4.Take(4).Solve();
            var results4b = secrets4.Skip(4).Take(4).Solve();

            var encodeWatch = Stopwatch.StartNew();
            var secrets5 = encoder.Encode(12345678, 1000000, 1660573643).ToList();
            encodeWatch.Stop();
            var decodeWatch = Stopwatch.StartNew();
            var result5 = secrets5.Solve();
            decodeWatch.Stop();
        }
    }
}
