var builder = DistributedApplication.CreateBuilder(args);

var apiservice = builder.AddProject<Projects.NetAspireApp_ApiService>("apiservice");

builder.AddProject<Projects.NetAspireApp_Web>("webfrontend")
    .WithReference(apiservice);

builder.Build().Run();
