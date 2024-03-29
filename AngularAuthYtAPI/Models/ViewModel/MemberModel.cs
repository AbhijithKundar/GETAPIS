﻿namespace AngularAuthYtAPI.Models.ViewModel
{
    public class MemberModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? PlanType { get; set; }
        public DateTime JoiningDate { get; set; }
        public string? MemberFullName { get; set; }
    }

    public class TeamDetail
    {
        public int memberId { get; set; }
        public int? parentId { get; set; }
        public string? name { get; set; }
        public int rate { get; set; }
        public int planId { get; set; }

    }

    public class TeamTree
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Name { get; set; }
        public int TreeOrder { get; set; }
        public int Rate { get; set; }
        public string? Package { get; set; }
        public string? Path { get; set; }
        public int Count { get; set; }
        public int ParentId { get; set; }
    }

    public class TeamTreeViewModel
    {
        public TeamTreeViewModel()
        {
            TeamTree = new List<TeamTreeViewModel>();
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentId { get; set; }
        public int Level { get; set; }
        public int Rate { get; set; }
        public List<TeamTreeViewModel> TeamTree { get; set; }

    }

    public class TeamTreeModel
    {
        public string? Level { get; set; }
        public int Count { get; set; }
        public double TeamIncome { get; set; }
    }

}
