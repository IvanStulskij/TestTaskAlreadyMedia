using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TestTaskAlreadyMedia.Core.Handlers;

namespace TestTaskAlreadyMedia.Controllers;

[ApiController]
[Route("[controller]")]
public class NasaObjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NasaObjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] DataSourceLoadOptionsBase loadOptions)
    {
        var result = await _mediator.Send(new GetNasaObjectsRequest { DataSourceLoadOptions = loadOptions });

        return Ok(result);
    }
}
