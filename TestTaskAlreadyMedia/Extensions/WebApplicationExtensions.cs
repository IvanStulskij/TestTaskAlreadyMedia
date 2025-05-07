using AutoWrapper;

namespace TestTaskAlreadyMedia.Extensions;

public static class WebApplicationExtensions
{
    public static void UseAutoWrapper(this WebApplication webApplication)
    {
        webApplication.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
        {
            EnableExceptionLogging = true,
            UseApiProblemDetailsException = true,
            IgnoreWrapForOkRequests = true,
        });
    }
}
