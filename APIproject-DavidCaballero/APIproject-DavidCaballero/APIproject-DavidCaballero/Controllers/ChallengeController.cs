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
    public class ChallengeController : ControllerBase
    {
        private readonly HackathonContext _context;

        public ChallengeController(HackathonContext context)
        {
            _context = context;
        }

        // GET: api/Challenge
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChallengeDTO>>> GetChallenges()
        {
            var challengeDTO = await _context.Challenges
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new ChallengeDTO
                {
                    ID = c.ID,
                    Code = c.Code,
                    Name = c.Name,
                    RowVersion = c.RowVersion,
                    //Members = c.Members?.Select(cMember => new MemberDTO
                    //{
                    //    ID = cMember.ID,
                    //    FirstName = cMember.FirstName,
                    //    MiddleName = cMember.MiddleName,
                    //    LastName = cMember.LastName,
                    //    MemberCode = cMember.MemberCode,
                    //    DOB = cMember.DOB,
                    //    SkillRating = cMember.SkillRating,
                    //    YearsExperience = cMember.YearsExperience,
                    //    Category = cMember.Category,
                    //    Organization = cMember.Organization,
                    //    RowVersion = cMember.RowVersion,
                    //    RegionID = cMember.RegionID,
                    //    Region = cMember.Region != null ? new RegionDTO
                    //    {
                    //        ID = cMember.Region.ID,
                    //        Code = cMember.Region.Code,
                    //        Name = cMember.Region.Name,
                    //        RowVersion = cMember.Region.RowVersion,
                    //        Members = cMember.Region.Members /* Stop recursive mapping */
                    //    } : null,
                    //    ChallengeID = cMember.ChallengeID,
                    //    Challenge = cMember.Challenge != null ? new ChallengeDTO
                    //    {
                    //        ID = cMember.Challenge.ID,
                    //        Code = cMember.Challenge.Code,
                    //        Name = cMember.Challenge.Name,
                    //        RowVersion = cMember.Challenge.RowVersion,
                    //        Members = cMember.Challenge.Members /* Stop recursive mapping */
                    //    } : null
                    //}).ToList()
                }).ToListAsync();

            if (challengeDTO.Count() > 0)
            {
                return challengeDTO;
            }
            else
            {
                return NotFound(new { Message = "No challenges found." });
            }

        }

        // GET: api/Challenge/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChallengeDTO>> GetChallenge(int id)
        {
            var challengeDTO = await _context.Challenges
               .AsNoTracking()
               .Where(c => c.ID == id)
               .Select(c => new ChallengeDTO
               {
                   ID = c.ID,
                   Code = c.Code,
                   Name = c.Name,
                   RowVersion = c.RowVersion,
                   //Members = c.Members?.Select(cMember => new MemberDTO
                   //{
                   //    ID = cMember.ID,
                   //    FirstName = cMember.FirstName,
                   //    MiddleName = cMember.MiddleName,
                   //    LastName = cMember.LastName,
                   //    MemberCode = cMember.MemberCode,
                   //    DOB = cMember.DOB,
                   //    SkillRating = cMember.SkillRating,
                   //    YearsExperience = cMember.YearsExperience,
                   //    Category = cMember.Category,
                   //    Organization = cMember.Organization,
                   //    RowVersion = cMember.RowVersion,
                   //    RegionID = cMember.RegionID,
                   //    Region = cMember.Region != null ? new RegionDTO
                   //    {
                   //        ID = cMember.Region.ID,
                   //        Code = cMember.Region.Code,
                   //        Name = cMember.Region.Name,
                   //        RowVersion = cMember.Region.RowVersion,
                   //        Members = cMember.Region.Members /* Stop recursive mapping */
                   //    } : null,
                   //    ChallengeID = cMember.ChallengeID,
                   //    Challenge = cMember.Challenge != null ? new ChallengeDTO
                   //    {
                   //        ID = cMember.Challenge.ID,
                   //        Code = cMember.Challenge.Code,
                   //        Name = cMember.Challenge.Name,
                   //        RowVersion = cMember.Challenge.RowVersion,
                   //        Members = cMember.Challenge.Members /* Stop recursive mapping */
                   //    } : null
                   //}).ToList()
               })
               .FirstOrDefaultAsync();

            if (challengeDTO == null)
            {
                return NotFound(new { Message = $"The challenge {id} was not found." });
            }
            else
            {
                return challengeDTO;
            }
        }

        // GET: api/Challenge/inc
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<ChallengeDTO>>> GetChallengesInc()
        {
            var data = await _context.Challenges
                .AsNoTracking()
                .Include(c => c.Members)
                .OrderBy(c => c.Name)
                .Select(c => new ChallengeDTO
                {
                    ID = c.ID,
                    Code = c.Code,
                    Name = c.Name,
                    RowVersion = c.RowVersion,
                    Members = c.Members.Select(m => new MemberDTO
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
                }).ToListAsync();
            if (!data.Any())
            {
                return NotFound("No challenges found.");
            }
            else
            {
                return data;
            }
        }
        [HttpGet("inc/{id}")]
        public async Task<ActionResult<ChallengeDTO>> GetChallengeInc(int id)
        {
            var challengeDTO = await _context.Challenges
                .AsNoTracking()
                .Include(c => c.Members)
                .Where(c => c.ID == id)
                .Select(c => new ChallengeDTO
                {
                    ID = c.ID,
                    Code = c.Code,
                    Name = c.Name,
                    RowVersion = c.RowVersion,
                    Members = c.Members.Select(cMember => new MemberDTO
                    {
                        ID = cMember.ID,
                        FirstName = cMember.FirstName,
                        MiddleName = cMember.MiddleName,
                        LastName = cMember.LastName,
                        MemberCode = cMember.MemberCode,
                        DOB = cMember.DOB,
                        SkillRating = cMember.SkillRating,
                        YearsExperience = cMember.YearsExperience,
                        Category = cMember.Category,
                        Organization = cMember.Organization,
                        RowVersion = cMember.RowVersion,
                        //RegionID = cMember.RegionID,
                        //Region = cMember.Region != null ? new RegionDTO
                        //{
                        //    ID = cMember.Region.ID,
                        //    Code = cMember.Region.Code,
                        //    Name = cMember.Region.Name,
                        //    RowVersion = cMember.Region.RowVersion,
                        //    Members = cMember.Region.Members /* Stop recursive mapping */
                        //} : null,
                        //ChallengeID = cMember.ChallengeID,
                        //Challenge = cMember.Challenge != null ? new ChallengeDTO
                        //{
                        //    ID = cMember.Challenge.ID,
                        //    Code = cMember.Challenge.Code,
                        //    Name = cMember.Challenge.Name,
                        //    RowVersion = cMember.Challenge.RowVersion,
                        //    Members = cMember.Challenge.Members /* Stop recursive mapping */
                        //} : null
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (challengeDTO == null)
                return NotFound($"Challenge with id {id} not found.");

            return challengeDTO;
        }

        //// PUT: api/Challenge/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutChallenge(int id, Challenge challenge)
        //{
        //    if (id != challenge.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(challenge).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ChallengeExists(id))
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

        //// POST: api/Challenge
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Challenge>> PostChallenge(Challenge challenge)
        //{
        //    _context.Challenges.Add(challenge);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetChallenge", new { id = challenge.ID }, challenge);
        //}

        //// DELETE: api/Challenge/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteChallenge(int id)
        //{
        //    var challenge = await _context.Challenges.FindAsync(id);
        //    if (challenge == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Challenges.Remove(challenge);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool ChallengeExists(int id)
        //{
        //    return _context.Challenges.Any(e => e.ID == id);
        //}
    }
}
