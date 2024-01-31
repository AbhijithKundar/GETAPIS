using AngularAuthYtAPI.Context;
using AngularAuthYtAPI.Models.ViewModel;
using AngularAuthYtAPI.Repository.Interface;
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
        private readonly IMemberRepository _repository;
        public MemberController(IMemberRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("getAllMembers")]
        public ActionResult GetAllMembers()
        {
            return Ok(_repository.GetAllMembers());
        }

        [HttpGet("getMembersForUser")]
        public ActionResult GetAllMembers(string? userName)
        {
            var data = _repository.GetAllMembers(userName);
            return Ok(data);
        }

        [HttpGet("getTeamTree")]
        public ActionResult GetTeamTree(string? userName)
        { 
            var member = _repository.GetMember(userName);
            if(member is null)
            {
                return BadRequest("Invalid User");
            }
            var data = _repository.GetTeamTree(member.Id);
            return Ok(data);
        }

        [HttpGet("getAutofillTree")]
        public ActionResult GetAutofillTree(string? userName)
        {
            var data = _repository.GetAutofillTree(userName);
            return Ok(data);
        }
        





    }
}
