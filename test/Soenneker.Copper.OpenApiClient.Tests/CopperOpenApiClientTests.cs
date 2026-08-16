using Soenneker.Tests.HostedUnit;

namespace Soenneker.Copper.OpenApiClient.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class CopperOpenApiClientTests : HostedUnitTest
{
    public CopperOpenApiClientTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}
