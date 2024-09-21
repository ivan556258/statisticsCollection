using WebApplication1.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackerController : ControllerBase
    {
        private TrackerServices _trackerService;
        private TrackerHttpService _trackerHttpServie;

        private string _externalIp;
        private string _referrer;
        
        public TrackerController(TrackerServices trackerService, TrackerHttpService trackerHttpServie)
        {
            _trackerService = trackerService;
            _trackerHttpServie = trackerHttpServie;
            
            _externalIp = _trackerService.GetExternalIpAddress();
            _referrer = _trackerService.GetReferrerUrl();
        }

        /// <summary>
        /// Обрабатывает статистику трекера и перенаправляет на указанный URL.
        /// </summary>
        /// <param name="lang">Язык, используемый в запросе (например, 'tj' для таджикского).</param>
        /// <param name="to">Целевой URL для перенаправления.</param>
        /// <param name="from">Источник запроса (например, 'from_dzen').</param>
        /// <returns>Перенаправление на указанный URL в параметре <paramref name="to"/>.</returns>
        [HttpGet]
        [Route("/api/v1/Link")]
        public RedirectResult SetStat(string lang, string to, string from)
        {
            ParamDto paramDto = new ParamDto
            {
                Referrer = _referrer,
                ExternalIp = _externalIp,
                Lang = lang,
                To = to,
                From = from
            };
            
            _trackerHttpServie.SetStat(paramDto);

            return Redirect(paramDto.To);
        }
    }
}
