using System.Collections.Generic;
using Amazon.CDK;

namespace Infrastructure
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new Stack(app, $"{Config.AppName}InfraStack", new StackProps
            {
                Env = new Environment
                {
                    Account = Config.AccountId,
                    Region = Config.Region
                },
                Tags = new Dictionary<string, string>
                {
                    {"Cost Center", "303200"},
                    {"Team", "storefront"},
                    {"Domain", "knowledge-sharing"},
                    {"Purpose", "Chapter Day"},
                    {"Environment", "test"},
                    {"Name", $"{Config.AppName}"}

                }
            });
            app.Synth();
        }
    }
}
