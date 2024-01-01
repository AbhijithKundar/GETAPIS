using AngularAuthYtAPI.Context;
using AngularAuthYtAPI.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace AngularAuthYtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MemberController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAllMembers")]
        public async Task<ActionResult> GetAllMembers()
        {
            return Ok(await _context.Members.ToListAsync());
        }

        [HttpGet("getMembersForUser")]
        public async Task<ActionResult> GetAllMembers(string? userName)
        {
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Name == userName);
            if (member is null)
            {
                return BadRequest(new { Message = "Please login again" });
            }

            var data = await (from m in _context.Members.AsQueryable()
                              join u in _context.Users.AsQueryable() on m.UserId equals u.Id
                              join p in _context.PlanTypes.AsQueryable() on m.PlanId equals p.Id
                              where m.ParentId == member.Id
                              select new MemberModel
                              {
                                  Id = m.Id,
                                  UserName = m.Name,
                                  MemberFullName = $"{u.FirstName} {u.LastName}",
                                  PlanType = p.Name,
                                  JoiningDate = m.JoiningDate
                              }).ToListAsync();

            return Ok(data);
        }

    }
}
