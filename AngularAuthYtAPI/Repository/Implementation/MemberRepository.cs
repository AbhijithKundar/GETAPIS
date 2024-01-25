using AngularAuthYtAPI.Context;
using AngularAuthYtAPI.Models;
using AngularAuthYtAPI.Models.ViewModel;
using AngularAuthYtAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthYtAPI.Repository.Implementation
{
    public class MemberRepository : IMemberRepository
    {

        private readonly AppDbContext _context;
        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Member> GetAllMembers()
        {
            return _context.Members.ToList();
        }

        public Member GetMember(string? userName)
        {
            return _context.Members.Where(x => x.Name == userName).FirstOrDefault();
        }

        public List<MemberModel> GetAllMembers(string? userName)
        {
            var member = GetMember(userName);

            var data = (from m in _context.Members.AsQueryable()
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
                        }).ToList();

            //var test = _context.Database.ExecuteSqlRaw("GetMembersTreeview {0}", 4);
            var a = _context.TeamTree.FromSqlInterpolated($"EXEC [GetMembersTreeview] @id = 4").ToList();
            var test = GetTeamTree(3);
            return data;
        }

        private TeamTreeViewModel GetTeamTree(int id)
        {
            var model = new TeamTreeViewModel();

            var ttt = from m in _context.Members
                      join p in _context.PlanTypes on m.PlanId equals p.Id
                      where m.Id >= id
                      select new
                      {
                          memberId = m.Id,
                          parentId = m.ParentId,
                          name = m.Name,
                          rate = p.Rate == null ? 0 : p.Rate,
                          planId = p.Id
                      };
            var mem = ttt.FirstOrDefault();

            model.Id = mem.memberId;
            model.Name = mem.name;
            model.Level = 1;
            model.ParentId = mem.parentId;
            model.Rate = mem.rate;
            //level1
            model.TeamTree = ttt.Where(x => x.parentId == mem.memberId).Any()
                ? ttt.Where(x => x.parentId == mem.memberId).Select(a => new TeamTreeViewModel
                {
                    Id = a.memberId,
                    Name = a.name,
                    Level = 2,
                    ParentId = a.parentId,
                    Rate = a.rate,
                    TeamTree = ttt.Where(x => x.parentId == a.memberId).Any()
                    ? ttt.Where(x => x.parentId == a.memberId).Select(b => new TeamTreeViewModel
                    {
                        Id = b.memberId,
                        Name = b.name,
                        Level = 3,
                        ParentId = b.parentId,
                        Rate = b.rate,
                        TeamTree = ttt.Where(x => x.parentId == b.memberId).Any()
                    ? ttt.Where(x => x.parentId == b.memberId).Select(c => new TeamTreeViewModel
                    {
                        Id = c.memberId,
                        Name = c.name,
                        Level = 4,
                        ParentId = c.parentId,
                        Rate = c.rate,
                        TeamTree = ttt.Where(x => x.parentId == c.memberId).Any()
                    ? ttt.Where(x => x.parentId == c.memberId).Select(d => new TeamTreeViewModel
                    {
                        Id = d.memberId,
                        Name = d.name,
                        Level = 5,
                        ParentId = d.parentId,
                        Rate = d.rate,
                        TeamTree = ttt.Where(x => x.parentId == d.memberId).Any()
                    ? ttt.Where(x => x.parentId == d.memberId).Select(e => new TeamTreeViewModel
                    {
                        Id = e.memberId,
                        Name = e.name,
                        Level = 6,
                        ParentId = e.parentId,
                        Rate = e.rate,
                        TeamTree = ttt.Where(x => x.parentId == e.memberId).Any()
                    ? ttt.Where(x => x.parentId == e.memberId).Select(f => new TeamTreeViewModel
                    {
                        Id = f.memberId,
                        Name = f.name,
                        Level = 7,
                        ParentId = f.parentId,
                        Rate = f.rate,
                        TeamTree = ttt.Where(x => x.parentId == f.memberId).Any()
                    ? ttt.Where(x => x.parentId == f.memberId).Select(g => new TeamTreeViewModel
                    {
                        Id = g.memberId,
                        Name = g.name,
                        Level = 8,
                        ParentId = g.parentId,
                        Rate = g.rate,
                        TeamTree = ttt.Where(x => x.parentId == g.memberId).Any()
                    ? ttt.Where(x => x.parentId == g.memberId).Select(h => new TeamTreeViewModel
                    {
                        Id = h.memberId,
                        Name = h.name,
                        Level = 9,
                        ParentId = h.parentId,
                        Rate = h.rate,
                        TeamTree = ttt.Where(x => x.parentId == h.memberId).Any()
                    ? ttt.Where(x => x.parentId == h.memberId).Select(i => new TeamTreeViewModel
                    {
                        Id = i.memberId,
                        Name = i.name,
                        Level = 10,
                        ParentId = i.parentId,
                        Rate = i.rate

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                }).ToList() : new List<TeamTreeViewModel>();

            var dat = model.TeamTree.Sum(a => a.TeamTree.Sum(b => b.TeamTree.Sum(c => c.TeamTree.Sum(d => 
            d.TeamTree.Sum(e => e.TeamTree.Sum(f => f.TeamTree.Sum(g => g.TeamTree.Sum(h =>
            h.TeamTree.Sum(h => h.Rate)))))))));

            return model;

        }
    }
}
