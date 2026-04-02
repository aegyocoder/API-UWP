using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIproject_DavidCaballero.Data;
using APIproject_DavidCaballero.Models;

namespace APIproject_DavidCaballero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly HackathonContext _context;

        public RegionController(HackathonContext context)
        {
            _context = context;
        }

        // GET: api/Region
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegionDTO>>> GetRegions()
        {
            var regionDTO = await _context.Regions
                .AsNoTracking()
                .OrderBy(r => r.Name)
                .Select(r => new RegionDTO
                {
                    ID = r.ID,
                    Code = r.Code,
                    Name = r.Name,
                    RowVersion = r.RowVersion,
                    Members = null
                })
                .ToListAsync();
            if (regionDTO.Count() > 0)
            {
                return regionDTO;
            }
            else
            {
                return NotFound(new { Message = "Error: No region found." });
            }
        }

        // GET: api/Region/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RegionDTO>> GetRegion(int id)
        {
            var regionDTO = await _context.Regions
                .AsNoTracking()
                .Where(r => r.ID == id)
                .Select(r => new RegionDTO
                {
                    ID = r.ID,
                    Code = r.Code,
                    Name = r.Name,
                    RowVersion = r.RowVersion,
                    Members = null
                })
                .FirstOrDefaultAsync();

            if (regionDTO == null)
            {
                return NotFound(new { Message = $"Error: Region with id {id} not found." });
            }
            else
            {
                return regionDTO;
            }
        }



        // GET: api/Region/inc
        //AI
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<RegionDTO>>> GetRegionsInc()
        {
            var regionDTO = await _context.Regions
                .AsNoTracking()
                .Include(r => r.Members)
                .OrderBy(r => r.Name)
                .Select(r => new RegionDTO
                {
                    ID = r.ID,
                    Code = r.Code,
                    Name = r.Name,
                    RowVersion = r.RowVersion,
                    Members = r.Members.Select(m => new MemberDTO
                    {
                        ID = m.ID,
                        FirstName = m.FirstName,
                        MiddleName = m.MiddleName,
                        LastName = m.LastName,
                        MemberCode = m.MemberCode,
                        DOB = m.DOB,
                        SkillRating = m.SkillRating,
                        YearsExperience = m.YearsExperience,
                        Category = m.Category,
                        Organization = m.Organization,
                        RegionID = m.RegionID,
                        ChallengeID = m.ChallengeID,
                        RowVersion = m.RowVersion
                    }).ToList()
                })
                .ToListAsync();

            if (!regionDTO.Any())
            {
                return NotFound(new { Message = "No regions found." });
            }
            else
            {
                return regionDTO;
            }
 
        }

        // GET: api/Region/inc/5
        //AI
        // Includes related Members
        [HttpGet("inc/{id}")]
        public async Task<ActionResult<RegionDTO>> GetRegionInc(int id)
        {
            var regionDTO = await _context.Regions
                .AsNoTracking()
                .Include(r => r.Members)
                .Where(r => r.ID == id)
                .Select(r => new RegionDTO
                {
                    ID = r.ID,
                    Code = r.Code,
                    Name = r.Name,
                    RowVersion = r.RowVersion,
                    Members = r.Members.Select(m => new MemberDTO
                    {
                        ID = m.ID,
                        FirstName = m.FirstName,
                        MiddleName = m.MiddleName,
                        LastName = m.LastName,
                        MemberCode = m.MemberCode,
                        DOB = m.DOB,
                        SkillRating = m.SkillRating,
                        YearsExperience = m.YearsExperience,
                        Category = m.Category,
                        Organization = m.Organization,
                        RegionID = m.RegionID,
                        ChallengeID = m.ChallengeID,
                        RowVersion = m.RowVersion
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (regionDTO == null)
            {
                return NotFound(new { Message = $"Region with id {id} not found." });
            }
            else
            {
                return regionDTO;
            }
        }


        //// PUT: api/Region/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutRegion(int id, Region region)
        //{
        //    if (id != region.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(region).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!RegionExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Region
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Region>> PostRegion(Region region)
        //{
        //    _context.Regions.Add(region);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetRegion", new { id = region.ID }, region);
        //}

        //// DELETE: api/Region/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteRegion(int id)
        //{
        //    var region = await _context.Regions.FindAsync(id);
        //    if (region == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Regions.Remove(region);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool RegionExists(int id)
        //{
        //    return _context.Regions.Any(e => e.ID == id);
        //}
    }
}
