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

     

    }
}
