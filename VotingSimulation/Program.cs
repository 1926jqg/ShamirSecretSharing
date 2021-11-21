using ShamirSecretSharing.SecretSharePoints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VotingSimulation.Models;

namespace VotingSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            for(int i = 10; i <= 10000000; i *= 10)
            {
                Console.WriteLine($"Simulating an election with {i} voters.");
                Console.WriteLine("---------");
                var watch = Stopwatch.StartNew();
                SimulateElection(GenerateElectorate(i, random), TwoPartyElection(), new SecretShareEncoder(random));
                watch.Stop();
                Console.WriteLine("---------");
                Console.WriteLine("");
                Console.WriteLine($"Election with {i} voters took {watch.ElapsedMilliseconds}ms");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            
        }

        static void SimulateElection(List<Voter> electorate, List<VotingCenter> centers, SecretShareEncoder encoder)
        {                               
            foreach (var voter in electorate)
            {
                voter.Vote(encoder, centers);
            }

            var secret = centers.Select(c => c.GetResult()).Solve();

            var electionValid = ValidateElection(centers, electorate.Count, secret);
            if (!electionValid)
            {
                Console.WriteLine("There were iregularities in the election. We will need to do a recount.");
                return;
            }
                

            var candidateAVotes = secret;
            var candidateBVotes = Math.Abs(secret - electorate.Count);

            Console.WriteLine($"{electorate.Count} people voted");
            Console.WriteLine($"Candidate A recieved {candidateAVotes} votes");
            Console.WriteLine($"Candidate B recieved {candidateBVotes} votes");
        }

        static bool ValidateElection(List<VotingCenter> centers, int electorateSize, int countedVote)
        {
            return countedVote <= electorateSize && centers.Any(c => c.Votes == electorateSize);
        }

        static List<Voter> GenerateElectorate(int size, Random random)
        {
            return Enumerable
               .Range(0, size)
               .Select(i => new Voter(i.ToString(), random.Next(2)))
               .ToList();
        }

        static List<VotingCenter> TwoPartyElection()
        {
            var partyA = new VotingCenter(51627977, 3, 2);
            var partyB = new VotingCenter(51627977, 7, 2);

            return new List<VotingCenter> { partyA, partyB };
        }
    }
}
