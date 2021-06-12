using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TradingSystem.Business.Market;
using TradingSystem.Service;
using TradingSystem.WebApi.DTO;

namespace TradingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatisticsController : ControllerBase
    {
        public StatisticsController(MarketUserService marketUserService)
        {
            MarketUserService = marketUserService;
        }

        public MarketUserService MarketUserService { get; }

        [HttpGet]
        public ActionResult<StatisticsDTO> FetchVisitingStatisticsForToday()
        {
            Statistics stats = MarketUserService.getTodayStats();
            if (stats == null)
            {
                return InternalServerError();
            }

            return Ok(StatisticsDTO.FromStatistics(stats));
        }

        [HttpGet]
        public ActionResult<StatisticsDTO> FetchVisitingStatisticsForDatesRange
        (
            [FromQuery(Name = "dateStart")] DateTime? dateStartNullable,
            [FromQuery(Name = "dateEnd")] DateTime? dateEndNullable
        )
        {
            DateTime dateStart = Utils.ConvertDate(dateStartNullable);
            DateTime dateEnd = Utils.ConvertDate(dateEndNullable);

            Statistics stats = MarketUserService.getDatesStats(dateStart, dateEnd);
            if (stats == null)
            {
                return InternalServerError();
            }

            return Ok(StatisticsDTO.FromStatistics(stats));
        }
    }
}
