using AngularAuthYtAPI.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthYtAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanTypeController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        public PlanTypeController(AppDbContext context)
        {
            _authContext = context;
        }

   

    /// <summary>
    /// Get Plan types
    /// </summary>
    /// <returns></returns>
    [HttpGet("getPlanTypes")]
        public async Task<ActionResult> GetPlanTypes()
        {
            return Ok(await _authContext.PlanTypes.ToListAsync());
        }
    }
}
