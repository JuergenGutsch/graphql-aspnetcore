using Cake.Common.Tools.DotNetCore;

var target = Argument("target", "Default");


Task("DotNetCoreClean")
	.Does(() => 
	{
	//	DotNetCoreClean("./GraphQlDemo.sln");
	});

Task("DotNetCoreRestore")
	.IsDependentOn("DotNetCoreClean")
	.Does(() => 
	{	
		DotNetCoreRestore("./GraphQlDemo.sln");
	});

Task("DotNetCoreBuild")
	.IsDependentOn("DotNetCoreRestore")
	.Does(() => 
	{	
		var settings = new DotNetCoreBuildSettings{
			Configuration = "Release"
		};
		DotNetCoreBuild("./GraphQlDemo.sln", settings);
	});

Task("DotNetCoreTest")
	.IsDependentOn("DotNetCoreBuild")
	.Does(() => 
	{
		var settings = new DotNetCoreTestSettings
		{
			Configuration = "Release"
		};

		var projectFiles = GetFiles("./*.Tests/**/*.Tests.csproj");
		foreach(var file in projectFiles)
		{
			// 'dotnet test'
			DotNetCoreTest(file.FullPath, settings);
		}
	});

Task("DotNetCorePack")
	.IsDependentOn("DotNetCoreTest")
	.Does(() => 
	{	
		var settings = new DotNetCorePackSettings
		{
			Configuration = "Release",
			OutputDirectory = "./artifacts/"
		};

		DotNetCorePack("./GraphQlDemo.sln", settings);
	});

Task("DotNetCorePush")
	.IsDependentOn("DotNetCorePack")
	.Does(() => 
	{		
		//pusch from ./artifacts/ to nuget/myget
	});

Task("Default")
	.IsDependentOn("DotNetCorePush")
	.Does(() =>
	{
	  Information("You build is done! :-)");
	});

RunTarget(target);