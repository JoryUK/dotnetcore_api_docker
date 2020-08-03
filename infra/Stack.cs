using System.IO;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.Ecr.Assets;
using Amazon.CDK.AWS.ECS;
using Amazon.CDK.AWS.ECS.Patterns;
using Amazon.CDK.AWS.Route53;

namespace Infrastructure
{
    public class Stack : Amazon.CDK.Stack
    {
      


        internal Stack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            //setup the image
            var asset = new DockerImageAsset(this, $"{Config.AppName}Image", new DockerImageAssetProps{
                Directory = Path.Combine(System.Environment.CurrentDirectory, "api"),
            });

            //Create the Fargate service
            var vpc = Vpc.FromLookup(
                this, "sandbox", new VpcLookupOptions
                {
                    VpcName = "sandbox_vpc"
                }
            );

            var cluster = new Cluster(this, $"{Config.AppName}Cluster", new ClusterProps
            {
                Vpc = vpc
            });

            var applicationDomain = $"{Config.ApplicationSubdomain}.{Config.DomainName}";
            var hostedZone = HostedZone.FromLookup(
                this, "HostedZone", new HostedZoneProviderProps
                {
                    DomainName = $"{Config.DomainName}.",
                    PrivateZone = false
                }
            );

            // Create a load-balanced Fargate service and make it public
            var fargateService = new ApplicationLoadBalancedFargateService(this, $"{Config.AppName}Service",
                new ApplicationLoadBalancedFargateServiceProps
                {
                    Cluster = cluster,          // Required
                    DesiredCount = 1,           // Default is 1
                    TaskImageOptions = new ApplicationLoadBalancedTaskImageOptions
                    {
                        Image = ContainerImage.FromDockerImageAsset(asset)
                    },
                    MemoryLimitMiB = 1024,      // Default is 256
                    PublicLoadBalancer = true,    // Default is false
                    DomainName = applicationDomain,
                    DomainZone = hostedZone,
                }
            );

            new CfnOutput(
                this, "Route53Url", new CfnOutputProps
                {
                    Value = applicationDomain,
                    Description = "Nice Route53 Url"
                }
            );
        }
    }
}
