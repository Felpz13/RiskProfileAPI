using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiskProfileAPI.Models;
using RiskProfileAPI.Services;

namespace RiskProfileAPI.Controllers
{
    [Route("api")]
    [ApiController]

    public class RiskController : ControllerBase
    {
        private RiskScoreApplicationService _riskScoreApplicationService;

        public RiskController()
        {
            _riskScoreApplicationService = new RiskScoreApplicationService();
        }

        [HttpGet]
        [Route("GetResult")]
        [AllowAnonymous]
        public IActionResult Risk([FromBody]User user)
        {
            var scoreResult = _riskScoreApplicationService.getUserScore(user);

            return Ok(scoreResult);
        }           
    }
}