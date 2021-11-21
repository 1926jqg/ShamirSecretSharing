using ShamirSecretSharing.SecretSharePoints;
using System.Collections.Generic;
using System.Linq;

namespace VotingSimulation.Models
{
    public class VotingCenter
    {
        private Dictionary<string, SecretSharePoint> _votes = new Dictionary<string, SecretSharePoint>();
        public int Mod { get; }
        public int Coefficient { get; }
        public int Quorum { get; }

        public int Votes { get => _votes.Count; }

        public VotingCenter(int mod, int coefficient, int quorum)
        {
            Mod = mod;
            Coefficient = coefficient;
            Quorum = quorum;
        }
        
        public VoteResponse Vote(SecretSharePoint vote, string signature)
        {
            if (vote.GetModulus() != Mod || vote.Quorum != Quorum || vote.Input != Coefficient)
                return VoteResponse.IncorrectCenter;
            if (_votes.ContainsKey(signature))
                return VoteResponse.VoteAlreadyCounted;
            _votes.Add(signature, vote);
            return VoteResponse.Success;
        }

        public SecretSharePoint GetResult()
        {
            SecretSharePoint total = default;
            bool first = true;
            foreach(var vote in _votes.Values)
            {
                if (first)
                {
                    total = vote;
                    first = false;
                    continue;
                }
                total += vote;
            }
            return total;
        }
    }
}
