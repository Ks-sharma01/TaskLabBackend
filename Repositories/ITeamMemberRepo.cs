using TaskLabBackend.Dto;
using TaskLabBackend.Models;

namespace TaskLabBackend.Repositories
{
    public interface ITeamMemberRepo
    {
        Task<TeamMember> AddMemberDetails(AddMemberDto addMember, int userId);
        Task<List<AddMemberDto>> GetMemberDetails(int userId);
    }
}
