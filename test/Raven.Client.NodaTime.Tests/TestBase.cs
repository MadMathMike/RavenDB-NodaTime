using Raven.Client.Documents;
using Raven.TestDriver;

namespace Raven.Client.NodaTime.Tests
{
    public class TestBase : RavenTestDriver<MyRavenDBLocator>
    {
        protected override void PreInitialize(IDocumentStore documentStore)
        {
            documentStore.ConfigureForNodaTime();
        }
    }
}
