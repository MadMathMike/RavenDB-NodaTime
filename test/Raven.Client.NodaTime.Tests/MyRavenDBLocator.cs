using Raven.TestDriver;
using System;
using System.IO;

namespace Raven.Client.NodaTime.Tests
{
    public class MyRavenDBLocator : RavenServerLocator
    {
        private const string RavenServerPath = @"..\..\..\..\..\RavenDB\Server\Raven.Server.exe";

        public override string ServerPath
        {
            get
            {
                return RavenServerPath;
            }
        }

        public override string Command => RavenServerPath;
        public override string CommandArguments => "";
    }
}
