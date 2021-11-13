using ShamirSecretSharing.SecretSharePoints;
using System;
using System.Collections.Generic;
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

            var secrets5 = encoder.Encode(12352, 500, 51627977).ToList();
            var result5 = secrets5.Solve();

            HomomorphicDemonstration();


        }

        public static void HomomorphicDemonstration()
        {
            var encoder = new SecretShareEncoder();
            var mod = 51627977;
            var quorum = 3;
            var inputs = new List<int>
            {
                12548,
                23568,
                78956
            };

            var secret1 = 456;
            var secret2 = 8974;
            var secret3 = 1234568;

            var encoded1 = encoder.Encode(secret1, quorum, mod, inputs);
            var encoded2 = encoder.Encode(secret2, quorum, mod, inputs);
            var encoded3 = encoder.Encode(secret3, quorum, mod, inputs);

            var decodedSum = encoded1
                .Zip(encoded2)
                .Select(x => x.First + x.Second)
                .Zip(encoded3)
                .Select(x => x.First + x.Second)
                .Solve();

            var secretsSum = secret1 + secret2 + secret3;
        }
    }
}
