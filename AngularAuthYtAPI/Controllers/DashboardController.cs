using AngularAuthYtAPI.Context;
using AngularAuthYtAPI.Models;
using AngularAuthYtAPI.Models.ViewModel;
using AngularAuthYtAPI.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthYtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly IDashboardRepository _dashboardRepository;
        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }


        [HttpGet("GetDashboard")]
        public async Task<ActionResult> GetDashboard(string? userName)
        {


        var dashboardModel = _dashboardRepository.GetDashboard(userName);

            return Ok(dashboardModel);
        }
    }
}
