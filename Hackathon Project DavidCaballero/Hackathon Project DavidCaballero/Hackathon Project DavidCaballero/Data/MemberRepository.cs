using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Hackathon_Project_DavidCaballero.Models;
using Hackathon_Project_DavidCaballero.Utilities;

namespace Hackathon_Project_DavidCaballero.Data
{
    public class MemberRepository : IMemberRepository
    {
        private readonly HttpClient client = new HttpClient();

        public MemberRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Member>> GetMembers()
        {
            HttpResponseMessage response = await client.GetAsync("api/Member");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Member>>();
            }
            else
            {
                string msg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(msg) ? "Could not access the list of Members." : msg);
            }
        }

        public async Task<Member> GetMember(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"api/Member/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Member>();
            }
            else
            {
                string msg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(msg) ? "Could not access that Member." : msg);
            }
        }

        public async Task<List<Member>> GetMembersByRegion(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"api/Member/ByRegion/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Member>>();
            }
            else
            {
                string msg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(msg) ? "Could not access members for that Region." : msg);
            }
        }

        public async Task<List<Member>> GetMembersByChallenge(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"api/Member/ByChallenge/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Member>>();
            }
            else
            {
                string msg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(msg) ? "Could not access members for that Challenge." : msg);
            }
        }

        public async Task<Member> AddMember(Member member)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/Member", member);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Member>();
            }
            else
            {
                string msg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(msg) ? "Could not add the Member." : msg);
            }
        }

        public async Task UpdateMember(Member member)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/Member/{member.ID}", member);
            if (response.IsSuccessStatusCode)
            {
                return; // NoContent expected
            }
            else
            {
                string msg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(msg) ? "Could not update the Member." : msg);
            }
        }

        public async Task DeleteMember(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"api/Member/{id}");
            if (response.IsSuccessStatusCode)
            {
                return; // NoContent expected
            }
            else
            {
                string msg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(msg) ? "Could not delete the Member." : msg);
            }
        }
    }
}
