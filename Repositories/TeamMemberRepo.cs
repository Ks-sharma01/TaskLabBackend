using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TaskLabBackend.Db;
using TaskLabBackend.Dto;
using TaskLabBackend.Models;
using System.Security.Claims;

namespace TaskLabBackend.Repositories
{
    public class TeamMemberRepo : ITeamMemberRepo
    {
        private readonly ApplicationDbContext _context;
        public TeamMemberRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TeamMember> AddMemberDetails(AddMemberDto addMember, int userId)
        {
            try
            {
                var member = new TeamMember
                {
                    Name = addMember.Name,
                    Role = addMember.Role,
                    Email = addMember.Email,
                    UserID = userId,
                    ExperienceInYears = addMember.ExperienceInYears,
                };
                await _context.TeamMembers.AddAsync(member);
                await _context.SaveChangesAsync();
                return member;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<AddMemberDto>> GetMemberDetails(int userId)
        {
            try
            {
                var members = await _context.TeamMembers.Where(p => p.UserID == userId).Select(x => new AddMemberDto
                {
                    Id = x.Id,
                    Name = x.Name,

                }).ToListAsync();
                return members;
              
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
