namespace SampleOrg.Foo.Website.Infrastructure;

/// <summary>
/// Declares a log-normal simulated latency for a controller class or action method.
/// The action-level attribute takes precedence over the controller-level one.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class SimulatedDelayAttribute : Attribute
{
    /// <summary>Median delay in milliseconds (50th percentile).</summary>
    public double MedianMs { get; init; }

    /// <summary>
    /// Log-normal sigma (shape). Higher = longer tail.
    /// Default 1.0 → P95 ≈ 258 ms at MedianMs=55, P99 ≈ 508 ms.
    /// </summary>
    public double Sigma { get; init; } = 1.0;
}
