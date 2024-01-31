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

        public List<TeamTree> GetAllMembers(string? userName)
        {
            var member = GetMember(userName);

            var data = ExcecuteTeamTree(member.Id);
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

        private List<TeamTree> ExcecuteTeamTree(int id)
        {
            return _context.TeamTree.FromSqlInterpolated($"EXEC [GetMembersTreeview] @id = {id}").ToList();
        }

        /// <summary>
        /// Team Tree for 10 levels
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>

        public List<TeamTreeModel> GetTeamTree(int id)
        {

            var a = ExcecuteTeamTree(id);
            var teamTree = a.Where(x => x.TreeOrder <= 10).GroupBy(x => x.TreeOrder, (obj, det) => new TeamTreeModel
            {
                Level = $"Level {obj}",
                Count = det.Count(),
                TeamIncome = det.Sum(x => x.Rate) * 0.015

            }).ToList();


            return teamTree;
        }

        public decimal GetTotalAffiliateIncome(int id)
        {
            var ttt = GetTeamDetails(id);
            float sum = 0;
            sum = ttt.Skip(1).Take(126).Sum(x => x.rate);
            decimal total = Convert.ToDecimal(sum * 0.025);
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
            if (count > 1)
            {
                model.TeamTree.AddRange(ttt.Skip(a).Take(2).Select(a => new TeamTreeViewModel
                {
                    Id = a.memberId,
                    Name = a.name,
                    Level = 2,
                    ParentId = a.parentId,
                    Rate = a.rate,
                    TeamTree = new List<TeamTreeViewModel>()
                }));

                if (count > 3)
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
                        a += 2;
                    }
                    if (count > 7)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                if (count + 2 > a)
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

                        if (count > 15)
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
