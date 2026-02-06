namespace MySpot.Tests.Integration.Infrastructure;

[CollectionDefinition("IntegrationTests", DisableParallelization = true)]
public class IntegrationTestsCollection : ICollectionFixture<ApplicationWebFactory>
{
}
