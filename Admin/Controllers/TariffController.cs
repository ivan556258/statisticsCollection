using Microsoft.AspNetCore.Mvc;
using WebApplication1.Admin.Repository.Query;

namespace WebApplication1.Admin.Controllers;

public class TariffController : ControllerBase
{
    private readonly TariffQuery tariffQuery;

    public TariffController()
    {
        tariffQuery = new TariffQuery();
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var tariffs = await tariffQuery.GetList();

        var response = new
        {
            data = tariffs,
        };
        return Ok(response);
    }
}