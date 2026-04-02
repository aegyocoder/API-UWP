using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Hackathon_Project_DavidCaballero.Utilities;
using Hackathon_Project_DavidCaballero.Models;
using System.Net.Http;

namespace Hackathon_Project_DavidCaballero.Data
{
    public class ChallengeRepository : IChallengeRepository
    {
        private readonly HttpClient client = new HttpClient();

        public ChallengeRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Challenge>> GetChallenges()
        {
            HttpResponseMessage response = await client.GetAsync("api/Challenge");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Challenge>>();
            }
            else
            {
                string msg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(msg) ? "Could not access the list of Challenges." : msg);
            }
        }

        public async Task<Challenge> GetChallenge(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"api/Challenge/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Challenge>();
            }
            else
            {
                string msg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(msg) ? "Could not access that Challenge." : msg);
            }
        }
    }
}
