Run a dotnet core ECS Fargate docker task, provision all required infrastructure and deploy to AWS using CDK.

# Config

Must specify the acccount and route53 hosted zone domain name in the Config.cs

# Api

dot net core 3.1 project with docker support (runs on Linux container)

## Useful commands

Execute from the root directory

* `dotnet build api` compile the api code

# Infra

The `cdk.json` file tells the CDK Toolkit how to execute your app.

It uses the [.NET Core CLI](https://docs.microsoft.com/dotnet/articles/core/) to compile and execute your project.

## Useful commands

Execute from the root directory

* `dotnet build infra` compile the infra code
* `cdk deploy`       deploy this stack to your default AWS account/region
* `cdk diff`         compare deployed stack with current state
* `cdk synth`        emits the synthesized CloudFormation template
