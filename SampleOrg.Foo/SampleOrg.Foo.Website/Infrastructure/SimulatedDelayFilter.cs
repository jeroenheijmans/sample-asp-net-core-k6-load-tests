using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleOrg.Foo.Website.Infrastructure;

/// <summary>
/// Global async action filter that sleeps for a log-normal sampled duration before
/// each action executes. The delay parameters are read from [SimulatedDelay] — first
/// on the action method, then on the controller class. If neither is present, the
/// action runs with no artificial delay.
/// </summary>
public sealed class SimulatedDelayFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

        // Action-level attribute wins; fall back to controller-level.
        var attr = descriptor?.MethodInfo.GetCustomAttributes(typeof(SimulatedDelayAttribute), inherit: false)
                       .FirstOrDefault() as SimulatedDelayAttribute
                   ?? descriptor?.ControllerTypeInfo.GetCustomAttributes(typeof(SimulatedDelayAttribute), inherit: false)
                       .FirstOrDefault() as SimulatedDelayAttribute;

        if (attr is not null)
        {
            var delayMs = DelayDistribution.SampleMs(attr.MedianMs, attr.Sigma);
            if (delayMs > 0)
                await Task.Delay(delayMs);
        }

        await next();
    }
}
