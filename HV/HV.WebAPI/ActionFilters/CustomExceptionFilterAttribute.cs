using HV.BLL.DTO;
using HV.BLL.Exceptions;
using HV.BLL.Exceptions.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HV.WebAPI.ActionFilters;

public class CustomExceptionFilterAttribute(IWebHostEnvironment environment) : ExceptionFilterAttribute
{
    private readonly IWebHostEnvironment _environment = environment;

    public override void OnException(ExceptionContext context)
    {
        var actionResult = context.Exception switch
        {
            NotFoundException ex => new NotFoundObjectResult(new ErrorResponse(ex.Message)),
            IncorrectParametersException ex => new BadRequestObjectResult(new ErrorResponse(ex.Message)),
            CustomExceptionBase ex => new BadRequestObjectResult(new ErrorResponse(ex.Message)),
            _ => _environment.IsDevelopment()
                ? new ObjectResult(new
                    {
                        context.Exception.Message,
                        context.Exception.StackTrace
                    })
                    { StatusCode = 500 }
                : new ObjectResult(new ErrorResponse("Unknown server error"))
                    { StatusCode = 500 }
        };

        context.ExceptionHandled = true;
        context.Result = actionResult;
    }
}