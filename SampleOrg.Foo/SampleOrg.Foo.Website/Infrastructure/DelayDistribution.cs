namespace SampleOrg.Foo.Website.Infrastructure;

/// <summary>
/// Samples a delay in milliseconds from a log-normal distribution.
///
/// Log-normal: delay = exp(ln(medianMs) + sigma * Z)  where Z ~ N(0,1)
/// Box-Muller is used to produce Z from two uniform samples.
/// Result is clamped to [0, 30 000] ms so there is no infinite tail.
/// </summary>
public static class DelayDistribution
{
    private const double MaxMs = 30_000;

    public static int SampleMs(double medianMs, double sigma)
    {
        var z = NextStandardNormal();
        var ms = Math.Exp(Math.Log(medianMs) + sigma * z);
        return (int)Math.Clamp(ms, 0, MaxMs);
    }

    // Box-Muller transform — produces one standard-normal sample.
    private static double NextStandardNormal()
    {
        double u1, u2;
        do { u1 = Random.Shared.NextDouble(); } while (u1 == 0.0); // avoid ln(0)
        u2 = Random.Shared.NextDouble();
        return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
    }
}
