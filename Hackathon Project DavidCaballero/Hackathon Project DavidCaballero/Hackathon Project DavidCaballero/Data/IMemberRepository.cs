using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hackathon_Project_DavidCaballero.Models;

namespace Hackathon_Project_DavidCaballero.Data
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetMembers();
        Task<Member> GetMember(int id);

        Task<List<Member>> GetMembersByRegion(int id);
        Task<List<Member>> GetMembersByChallenge(int id);

        Task<Member> AddMember(Member member);
        Task UpdateMember(Member member);
        Task DeleteMember(int id);
    }
}
