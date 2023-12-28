var builder = DistributedApplication.CreateBuilder(args);

var rabbit = builder.AddRabbitMQContainer("rabbit_messaging", password: "passwordless");

var apiservice = builder.AddProject<Projects.NetAspireApp_ApiService>("apiservice")
    .WithReference(rabbit);

builder.AddProject<Projects.NetAspireApp_Web>("webfrontend")
    .WithReference(apiservice);

builder.Build().Run();
