using Microsoft.AspNetCore.Mvc;
using BusData.Repositories;
using BusData.Models;

[ApiController]
[Route("api/[controller]")]
public class TransitController : ControllerBase
{
    private readonly ShuttleRepository _repository;
    public TransitController(ShuttleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("routes")]
    public async Task<ActionResult<IEnumerable<RouteDto>>> GetRoutes()
    {
        var routes = await _repository.GetRoutesAsync();
        return Ok(routes);
    }

    [HttpGet("stops")]
    public async Task<ActionResult<IEnumerable<StopDto>>> GetStops([FromQuery] string routeId)
    {
        var stops = await _repository.GetStopsByRouteAsync(routeId);
        return Ok(stops);
    }
}

