using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskLabBackend.Dto;
using TaskLabBackend.Models;
using TaskLabBackend.Repositories;

namespace TaskLabBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly ITeamMemberRepo _teamMemberRepo;
        public MemberController(ITeamMemberRepo teamMemberRepo)
        {
            _teamMemberRepo = teamMemberRepo;
        }

        [Authorize]
        [HttpPost("Member")]
        public async Task<TeamMember> AddTeamMember(AddMemberDto addMember)
        {
            try
            {

            var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(userIdClaims);
            var members = await _teamMemberRepo.AddMemberDetails(addMember, userId);
            return members;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("MemberDetails")]
        public async Task<List<AddMemberDto>> GetMemberDetails()
        {
            try
            {
                var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int userId = Convert.ToInt32(userIdClaims);
                var members = await _teamMemberRepo.GetMemberDetails(userId);
                return members;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
    }
}
