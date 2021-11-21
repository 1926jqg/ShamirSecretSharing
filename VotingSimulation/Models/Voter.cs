using ShamirSecretSharing.SecretSharePoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VotingSimulation.Models
{
    public class Voter
    {
        private string _signature;
        private int _vote;

        public Voter(string signature, int vote)
        {
            _signature = signature;
            _vote = vote;
        }

        public void Vote(SecretShareEncoder encoder, IEnumerable<VotingCenter> votingCenters)
        {
            var exemplar = votingCenters.First();
            var secrets = encoder.Encode(_vote, exemplar.Quorum, exemplar.Mod, votingCenters.Select(vc => vc.Coefficient))
                .GroupBy(s => s.Input)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault());
            foreach(var votingCenter in votingCenters)
            {
                votingCenter.Vote(secrets[votingCenter.Coefficient], _signature);
            }
        }

    }
}
