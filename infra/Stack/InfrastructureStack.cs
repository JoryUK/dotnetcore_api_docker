using System;
using System.IO;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECS;
using Amazon.CDK.AWS.ECS.Patterns;
using Amazon.CDK.AWS.ECR;
using Amazon.CDK.AWS.Ecr.Assets;
using Amazon.JSII.JsonModel.Spec;

namespace Infrastructure.Stack
{
    public class InfrastructureStack : Amazon.CDK.Stack
    {
        internal InfrastructureStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            //setup the image
            var asset = new DockerImageAsset(this, $"{App.Name}Image", new DockerImageAssetProps{
                Directory = Path.Combine(System.Environment.CurrentDirectory, "../api"),
                
            });

            //Create the Fargate service
            var vpc = Vpc.FromLookup(
                this, "sandbox", new VpcLookupOptions
                {
                    VpcName = "sandbox_vpc"
                }
            );

            var cluster = new Cluster(this, $"{App.Name}Cluster", new ClusterProps
            {
                Vpc = vpc
            });

            // Create a load-balanced Fargate service and make it public
            var fargateService = new ApplicationLoadBalancedFargateService(this, $"{App.Name}Service",
                new ApplicationLoadBalancedFargateServiceProps
                {
                    Cluster = cluster,          // Required
                    DesiredCount = 1,           // Default is 1
                    TaskImageOptions = new ApplicationLoadBalancedTaskImageOptions
                    {
                        Image = ContainerImage.FromDockerImageAsset(asset)
                    },
                    MemoryLimitMiB = 1024,      // Default is 256
                    PublicLoadBalancer = true    // Default is false
                }
            );
        }
    }
}
