using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hackathon_Project_DavidCaballero.Models;

namespace Hackathon_Project_DavidCaballero.Data
{
    public interface IChallengeRepository
    {
        Task<List<Challenge>> GetChallenges();
        Task<Challenge> GetChallenge(int id);
    }
}
