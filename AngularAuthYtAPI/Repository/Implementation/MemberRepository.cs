using AngularAuthYtAPI.Context;
using AngularAuthYtAPI.Models;
using AngularAuthYtAPI.Models.ViewModel;
using AngularAuthYtAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

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

            //var a = _context.TeamTree.FromSqlInterpolated($"EXEC [GetMembersTreeview] @id = 4").ToList();
            //var test = GetTeamTree(3);
            return data;
        }

        public List<TeamDetail> GetTeamDetails(int id)
        {
            var teamDetails = (from m in _context.Members
                               join p in _context.PlanTypes on m.PlanId equals p.Id
                               where m.Id >= id
                               select new TeamDetail
                               {
                                   memberId = m.Id,
                                   parentId = m.ParentId,
                                   name = m.Name,
                                   rate = p.Rate == null ? 0 : p.Rate,
                                   planId = p.Id
                               }).OrderBy(x => x.memberId).ToList();
            return teamDetails;
        }

        /// <summary>
        /// Team Tree for 10 levels
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>

        public TeamTreeViewModel GetTeamTree(string? userName)
        {
            var model = new TeamTreeViewModel();
            var member = GetMember(userName);

            var ttt = GetTeamDetails(member.Id);
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
                        Rate = i.rate,
                        TeamTree = ttt.Where(x => x.parentId == i.memberId).Any()
                    ? ttt.Where(x => x.parentId == i.memberId).Select(j => new TeamTreeViewModel
                    {
                        Id = j.memberId,
                        Name = j.name,
                        Level = 10,
                        ParentId = j.parentId,
                        Rate = j.rate

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                    }).ToList() : new List<TeamTreeViewModel>()

                }).ToList() : new List<TeamTreeViewModel>();



            return model;
        }

        public decimal GetTotalAffiliateIncome(int id)
        {
            var ttt = GetTeamDetails(id);
            float sum = 0;
            sum = ttt.Skip(1).Take(126).Sum(x => x.rate);
            decimal total = Convert.ToDecimal(sum * 0.025);
            return total;
        }

        /// <summary>
        /// Get Total Team Income for 10 levels
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public decimal GetTotalTeamIncome(string? userName)
        {
            float sum = 0;
            var model = GetTeamTree(userName);
            foreach (var a in model.TeamTree)
            {
                //levl1
                sum += a.Rate;
                if (a.TeamTree.Count() > 0)
                {
                    foreach (var b in a.TeamTree)
                    {
                        //level2
                        sum += b.Rate;
                        if (b.TeamTree.Count > 0)
                        {
                            //level3
                            foreach (var c in b.TeamTree)
                            {
                                sum += c.Rate;
                                if (c.TeamTree.Count > 0)
                                {
                                    //level4
                                    foreach (var d in c.TeamTree)
                                    {
                                        sum += d.Rate;
                                        if (d.TeamTree.Count > 0)
                                        {
                                            //level5
                                            foreach (var e in d.TeamTree)
                                            {
                                                sum += e.Rate;
                                                if (e.TeamTree.Count > 0)
                                                {
                                                    //level6
                                                    foreach (var f in e.TeamTree)
                                                    {
                                                        sum += f.Rate;
                                                        if (f.TeamTree.Count > 0)
                                                        {
                                                            //level7
                                                            foreach (var g in f.TeamTree)
                                                            {
                                                                sum += g.Rate;
                                                                if (g.TeamTree.Count > 0)
                                                                {
                                                                    //level8
                                                                    foreach (var h in g.TeamTree)
                                                                    {
                                                                        sum += h.Rate;
                                                                        if (h.TeamTree.Count > 0)
                                                                        {
                                                                            //level9
                                                                            foreach (var i in h.TeamTree)
                                                                            {
                                                                                sum += i.Rate;
                                                                                if (i.TeamTree.Count > 0)
                                                                                {
                                                                                    //level10
                                                                                    foreach (var j in i.TeamTree)
                                                                                    {
                                                                                        sum += j.Rate;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            decimal total = Convert.ToDecimal(sum * 0.015);
            return total;
        }


        public TeamTreeViewModel GetAutofillTree(string? userName)
        {
            var model = new TeamTreeViewModel();
            var member = GetMember(userName);
            var ttt = GetTeamDetails(member.Id);
            var mem = ttt.FirstOrDefault();
            model.Id = mem.memberId;
            model.Name = mem.name;
            model.Level = 1;
            model.ParentId = mem.parentId;
            model.Rate = mem.rate;
            var count = ttt.Count();
            int a = 1;
           if(count > 1)
            {
                model.TeamTree.AddRange(ttt.Skip(a).Take(2).Select(a => new TeamTreeViewModel
                {
                    Id = a.memberId,
                    Name = a.name,
                    Level = 2,
                    ParentId = a.parentId,
                    Rate = a.rate,
                    TeamTree = new List<TeamTreeViewModel>()
                })) ;

                if(count >3)
                {
                    a += 2;
                    for (int i = 0; i < 2; i++)
                    {
                        model.TeamTree[i].TeamTree.AddRange(ttt.Skip(a).Take(2).Select(a => new TeamTreeViewModel
                        {
                            Id = a.memberId,
                            Name = a.name,
                            Level = 3,
                            ParentId = a.parentId,
                            Rate = a.rate,
                            TeamTree = new List<TeamTreeViewModel>()
                        }));
                        a+= 2;
                    }
                    if(count > 7)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                if(count+2 > a)
                                { 
                                model.TeamTree[i].TeamTree[j].TeamTree.AddRange(ttt.Skip(a).Take(2).Select(a => new TeamTreeViewModel
                                {
                                    Id = a.memberId,
                                    Name = a.name,
                                    Level = 4,
                                    ParentId = a.parentId,
                                    Rate = a.rate,
                                    TeamTree = new List<TeamTreeViewModel>()
                                }));
                                a = a + 2;
                                }
                            }
                        }

                        if(count > 15)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    for (int k = 0; k < 2; k++)
                                    {
                                        if (count + 2 > a)
                                        {
                                            model.TeamTree[i].TeamTree[j].TeamTree[k].TeamTree.AddRange(ttt.Skip(a).Take(2).Select(a => new TeamTreeViewModel
                                            {
                                                Id = a.memberId,
                                                Name = a.name,
                                                Level = 5,
                                                ParentId = a.parentId,
                                                Rate = a.rate,
                                                TeamTree = new List<TeamTreeViewModel>()
                                            }));
                                            a = a + 2;
                                        }
                                    }
                                }
                            }

                            if (count > 31)
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    for (int j = 0; j < 2; j++)
                                    {
                                        for (int k = 0; k < 2; k++)
                                        {
                                            for (int l = 0; l < 2; l++)
                                            {
                                                if (count + 2 > a)
                                                {
                                                    model.TeamTree[i].TeamTree[j].TeamTree[k].TeamTree[l].TeamTree.AddRange(ttt.Skip(a).Take(2).Select(a => new TeamTreeViewModel
                                                    {
                                                        Id = a.memberId,
                                                        Name = a.name,
                                                        Level = 6,
                                                        ParentId = a.parentId,
                                                        Rate = a.rate,
                                                        TeamTree = new List<TeamTreeViewModel>()
                                                    }));
                                                    a = a + 2;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return model;
        }
    }
}
