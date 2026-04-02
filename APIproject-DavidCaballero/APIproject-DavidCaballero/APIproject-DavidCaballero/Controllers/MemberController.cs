using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIproject_DavidCaballero.Data;
using APIproject_DavidCaballero.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIproject_DavidCaballero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly HackathonContext _context;

        public MemberController(HackathonContext context)
        {
            _context = context;
        }

        // GET: api/Member
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembers()
        {
            var memberDTO = await _context.Members
                .AsNoTracking()
                .OrderBy(m => m.LastName).ThenBy(m => m.FirstName)
                .Select(m => new MemberDTO
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
                    RowVersion = m.RowVersion,
                    //RegionID = m.RegionID,
                    //Region = m.Region != null ? new RegionDTO
                    //{
                    //    ID = m.Region.ID,
                    //    Code = m.Region.Code,
                    //    Name = m.Region.Name,
                    //    RowVersion = m.Region.RowVersion,
                    //    Members = m.Region.Members?.Select(mRegionMember => new MemberDTO
                    //    {
                    //        ID = mRegionMember.ID,
                    //        FirstName = mRegionMember.FirstName,
                    //        MiddleName = mRegionMember.MiddleName,
                    //        LastName = mRegionMember.LastName,
                    //        MemberCode = mRegionMember.MemberCode,
                    //        DOB = mRegionMember.DOB,
                    //        SkillRating = mRegionMember.SkillRating,
                    //        YearsExperience = mRegionMember.YearsExperience,
                    //        Category = mRegionMember.Category,
                    //        Organization = mRegionMember.Organization,
                    //        RowVersion = mRegionMember.RowVersion,
                    //        RegionID = mRegionMember.RegionID,
                    //        Region = mRegionMember.Region /* Stop recursive mapping */,
                    //        ChallengeID = mRegionMember.ChallengeID,
                    //        Challenge = mRegionMember.Challenge != null ? new ChallengeDTO
                    //        {
                    //            ID = mRegionMember.Challenge.ID,
                    //            Code = mRegionMember.Challenge.Code,
                    //            Name = mRegionMember.Challenge.Name,
                    //            RowVersion = mRegionMember.Challenge.RowVersion,
                    //            Members = mRegionMember.Challenge.Members /* Stop recursive mapping */
                    //        } : null
                    //    }).ToList()
                    //} : null,
                    //ChallengeID = m.ChallengeID,
                    //Challenge = m.Challenge != null ? new ChallengeDTO
                    //{
                    //    ID = m.Challenge.ID,
                    //    Code = m.Challenge.Code,
                    //    Name = m.Challenge.Name,
                    //    RowVersion = m.Challenge.RowVersion,
                    //    Members = m.Challenge.Members?.Select(mChallengeMember => new MemberDTO
                    //    {
                    //        ID = mChallengeMember.ID,
                    //        FirstName = mChallengeMember.FirstName,
                    //        MiddleName = mChallengeMember.MiddleName,
                    //        LastName = mChallengeMember.LastName,
                    //        MemberCode = mChallengeMember.MemberCode,
                    //        DOB = mChallengeMember.DOB,
                    //        SkillRating = mChallengeMember.SkillRating,
                    //        YearsExperience = mChallengeMember.YearsExperience,
                    //        Category = mChallengeMember.Category,
                    //        Organization = mChallengeMember.Organization,
                    //        RowVersion = mChallengeMember.RowVersion,
                    //        RegionID = mChallengeMember.RegionID,
                    //        Region = mChallengeMember.Region != null ? new RegionDTO
                    //        {
                    //            ID = mChallengeMember.Region.ID,
                    //            Code = mChallengeMember.Region.Code,
                    //            Name = mChallengeMember.Region.Name,
                    //            RowVersion = mChallengeMember.Region.RowVersion,
                    //            Members = mChallengeMember.Region.Members /* Stop recursive mapping */
                    //        } : null,
                    //        ChallengeID = mChallengeMember.ChallengeID,
                    //        Challenge = mChallengeMember.Challenge /* Stop recursive mapping */
                    //    }).ToList()
                    //} : null
                }).ToListAsync();
            if (memberDTO.Count() > 0)
            {
                return memberDTO;
            }
            else
            {
                return NotFound(new { Message = "No members found." });
            }
        }

        // GET: api/Member/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDTO>> GetMember(int id)
        {
            var memberDTO = await _context.Members
                .AsNoTracking()
                .Where(m => m.ID == id)
                .Select(m => new MemberDTO
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
                })
                .FirstOrDefaultAsync();

            if (memberDTO == null)
            {
                return NotFound(new { Message = $"The member with the number {id} was not found." });
            }
            else
            {
                return memberDTO;
            }
        }


        // GET: api/Member/ByRegion/2
        [HttpGet("ByRegion/{id:int}")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembersByRegion(int id)
        {
            var memberDTO = await _context.Members
                .AsNoTracking()
                .Where(m => m.RegionID == id)
                .OrderBy(m => m.LastName).ThenBy(m => m.FirstName)
                .Select(m => new MemberDTO
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
                    RowVersion = m.RowVersion,
                    RegionID = m.RegionID,
                    ChallengeID = m.ChallengeID,
                    //Region = m.Region != null ? new RegionDTO
                    //{
                    //    ID = m.Region.ID,
                    //    Code = m.Region.Code,
                    //    Name = m.Region.Name,
                    //    RowVersion = m.Region.RowVersion,
                    //    Members = m.Region.Members?.Select(mRegionMember => new MemberDTO
                    //    {
                    //        ID = mRegionMember.ID,
                    //        FirstName = mRegionMember.FirstName,
                    //        MiddleName = mRegionMember.MiddleName,
                    //        LastName = mRegionMember.LastName,
                    //        MemberCode = mRegionMember.MemberCode,
                    //        DOB = mRegionMember.DOB,
                    //        SkillRating = mRegionMember.SkillRating,
                    //        YearsExperience = mRegionMember.YearsExperience,
                    //        Category = mRegionMember.Category,
                    //        Organization = mRegionMember.Organization,
                    //        RowVersion = mRegionMember.RowVersion,
                    //        RegionID = mRegionMember.RegionID,
                    //        Region = mRegionMember.Region /* Stop recursive mapping */,
                    //        ChallengeID = mRegionMember.ChallengeID,
                    //        Challenge = mRegionMember.Challenge != null ? new ChallengeDTO
                    //        {
                    //            ID = mRegionMember.Challenge.ID,
                    //            Code = mRegionMember.Challenge.Code,
                    //            Name = mRegionMember.Challenge.Name,
                    //            RowVersion = mRegionMember.Challenge.RowVersion,
                    //            Members = mRegionMember.Challenge.Members /* Stop recursive mapping */
                    //        } : null
                    //    }).ToList()
                    //} : null,
                    //ChallengeID = m.ChallengeID,
                    //Challenge = m.Challenge != null ? new ChallengeDTO
                    //{
                    //    ID = m.Challenge.ID,
                    //    Code = m.Challenge.Code,
                    //    Name = m.Challenge.Name,
                    //    RowVersion = m.Challenge.RowVersion,
                    //    Members = m.Challenge.Members?.Select(mChallengeMember => new MemberDTO
                    //    {
                    //        ID = mChallengeMember.ID,
                    //        FirstName = mChallengeMember.FirstName,
                    //        MiddleName = mChallengeMember.MiddleName,
                    //        LastName = mChallengeMember.LastName,
                    //        MemberCode = mChallengeMember.MemberCode,
                    //        DOB = mChallengeMember.DOB,
                    //        SkillRating = mChallengeMember.SkillRating,
                    //        YearsExperience = mChallengeMember.YearsExperience,
                    //        Category = mChallengeMember.Category,
                    //        Organization = mChallengeMember.Organization,
                    //        RowVersion = mChallengeMember.RowVersion,
                    //        RegionID = mChallengeMember.RegionID,
                    //        Region = mChallengeMember.Region != null ? new RegionDTO
                    //        {
                    //            ID = mChallengeMember.Region.ID,
                    //            Code = mChallengeMember.Region.Code,
                    //            Name = mChallengeMember.Region.Name,
                    //            RowVersion = mChallengeMember.Region.RowVersion,
                    //            Members = mChallengeMember.Region.Members /* Stop recursive mapping */
                    //        } : null,
                    //        ChallengeID = mChallengeMember.ChallengeID,
                    //        Challenge = mChallengeMember.Challenge /* Stop recursive mapping */
                    //    }).ToList()
                    //} : null
                })
                .ToListAsync();

            if (!memberDTO.Any())
                return NotFound(new { Message = "No members found for this region." });

            return memberDTO;
        }

        // GET: api/Member/ByChallenge/13
        [HttpGet("ByChallenge/{id:int}")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembersByChallenge(int id)
        {
            var data = await _context.Members
                .AsNoTracking()
                .Where(m => m.ChallengeID == id)
                .OrderBy(m => m.LastName).ThenBy(m => m.FirstName)
                .Select(m => new MemberDTO
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
                })
                .ToListAsync();

            if (!data.Any())
                return NotFound(new { Message = "No members found for this challenge." });

            return data;
        }

        // PUT: api/Member/5
        //AI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, MemberDTO member)
        {
            if (id != member.ID)
            {
                return BadRequest(new { Message = "Error: Route id and body id do not match." });
            }
             
            
            var entity = await _context.Members.FirstOrDefaultAsync(m => m.ID == id);
            if (entity == null)
            {
                return NotFound(new { Message = $"Member with id {id} not found." });
            }
                

            entity.FirstName = member.FirstName;
            entity.MiddleName = member.MiddleName;
            entity.LastName = member.LastName;
            entity.MemberCode = member.MemberCode;
            entity.DOB = member.DOB;
            entity.SkillRating = member.SkillRating;
            entity.YearsExperience = member.YearsExperience;
            entity.Category = member.Category;
            entity.Organization = member.Organization;
            entity.RegionID = member.RegionID;
            entity.ChallengeID = member.ChallengeID;

            // Tell EF what RowVersion the client had
            if (member.RowVersion != null)
                _context.Entry(entity).Property(e => e.RowVersion).OriginalValue = member.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Message = "Concurrency conflict: the member was updated by someone else. Re-load and try again." });
            }

            return NoContent();
        }

        // POST: api/Member
        //AI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MemberDTO>> PostMember(MemberDTO memberDTO)
        {
            bool regionExists = await _context.Regions.AnyAsync(r => r.ID == memberDTO.RegionID);
            bool challengeExists = await _context.Challenges.AnyAsync(c => c.ID == memberDTO.ChallengeID);

            if (!regionExists)
            {
                return BadRequest(new { Message = "Invalid RegionID." });
            }
                

            if (!challengeExists)
            {
                return BadRequest(new { Message = "Invalid ChallengeID." });
            }
                

            var entity = new Member
            {
                FirstName = memberDTO.FirstName,
                MiddleName = memberDTO.MiddleName,
                LastName = memberDTO.LastName,
                MemberCode = memberDTO.MemberCode,
                DOB = memberDTO.DOB,
                SkillRating = memberDTO.SkillRating,
                YearsExperience = memberDTO.YearsExperience,
                Category = memberDTO.Category,
                Organization = memberDTO.Organization,
                RegionID = memberDTO.RegionID,
                ChallengeID = memberDTO.ChallengeID
            };

            _context.Members.Add(entity);
            await _context.SaveChangesAsync();

            // Return created DTO (include RowVersion)
            memberDTO.ID = entity.ID;
            memberDTO.RowVersion = entity.RowVersion;

            return CreatedAtAction(nameof(GetMember), new { id = memberDTO.ID }, memberDTO);
        }

        // DELETE: api/Member/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var entity = await _context.Members.FindAsync(id);
            if (entity == null)
                return NotFound(new { Message = $"Member with id {id} not found." });

            _context.Members.Remove(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Message = "Concurrency conflict while deleting. Try again." });
            }

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.ID == id);
        }
    }
}
