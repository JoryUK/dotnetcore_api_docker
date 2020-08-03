using System.Collections.Generic;
using Amazon.CDK;

namespace Infrastructure.Stack
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new Amazon.CDK.App();
            new InfrastructureStack(app, $"{App.Name}InfraStack", new StackProps
            {
                Env = new Environment
                {
                    Account = "",
                    Region = "eu-west-1"
                },
                Tags = new Dictionary<string, string>
                {
                    {"Cost Center", "303200"},
                    {"Team", "storefront"},
                    {"Domain", "knowledge-sharing"},
                    {"Purpose", "Chapter Day"},
                    {"Environment", "test"},
                    {"Name", $"{App.Name}"}

                }
            });
            app.Synth();
        }
    }
}
