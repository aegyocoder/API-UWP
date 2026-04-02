using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Hackathon_Project_DavidCaballero.Models;
using Hackathon_Project_DavidCaballero.Utilities;
using Newtonsoft.Json;

namespace Hackathon_Project_DavidCaballero.Data
{
    public class RegionRepository : IRegionRepository
    {
        private readonly HttpClient client = new HttpClient();

        public RegionRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Region>> GetRegions()
        {
            HttpResponseMessage response = await client.GetAsync("api/region");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Region>>();
            }
            else
            {
                // Pass API error info
                string msg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(msg) ? "Could not access the list of Regions." : msg);
            }
        }

        public async Task<Region> GetRegion(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"api/region/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Region>();
            }
            else
            {
                string msg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(msg) ? "Could not access that Region." : msg);
            }
        }
    }
}
