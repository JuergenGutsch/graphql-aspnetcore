#addin "nuget:?package=NuGet.Core&version=2.14.0"
#addin "Cake.ExtendedNuGet"
using Cake.ExtendedNuGet;

var target = Argument("target", "Default");

var nugetSource = "https://www.myget.org/F/juergengutsch/api/v3/index.json";
var apiKey = "9ff4cf57-c7b5-4441-a48f-c22bab77d60e";

Task("Clean")
	.Does(() =>
	{
		Information("Clean Workspace!");
		DotNetCoreClean("./GraphQlDemo.sln");
		CleanDirectory("./output/");
		CleanDirectory("./artifacts/");

	});

Task("Restore")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		Information("Restore Workspace!");
		DotNetCoreRestore("./GraphQlDemo.sln"); 
	});

Task("Build")
	.IsDependentOn("Restore")
	.Does(() =>
	{
		var settings = new DotNetCoreBuildSettings
		{
			Configuration = "Release",
			OutputDirectory = "./output/"
		};

		Information("Build Project!");
		//DotNetCoreBuild("./GraphQlDemo.sln"); 

		var exitCodeWithArgument = StartProcess("dotnet", "build  GraphQlDemo.sln -c Release");

	});

Task("Test")
	.IsDependentOn("Build")
	.Does(() =>
	{
		Information("Test Projects!");
		Information("No tests yet :-(");
	});

Task("Publish")
	.IsDependentOn("Test")
	.Does(() =>
	{
		Information("Publish Libraries!");
		var settings = new DotNetCorePublishSettings
		{
			Configuration = "Release",
			OutputDirectory = "./artifacts/"
		};

		//DotNetCorePublish("./GraphQl.AspNetCore", settings);
		StartProcess("dotnet", "pack  GraphQl.AspNetCore -c Release -o ../artifacts");

		//DotNetCorePublish("./GraphQL.AspNetCore.Graphiql", settings);
		StartProcess("dotnet", "pack  GraphQl.AspNetCore.Graphiql -c Release -o ../artifacts");
	});

Task("Deploy")
	.IsDependentOn("Publish")
	.Does(() =>
	{
		Information("DepPackagesloy Workspace!");
		var settings = new NuGetPushSettings {
			Source = nugetSource,
			ApiKey = apiKey
		};

		var files = GetFiles("./artifacts/**/GraphQl.AspNetCore*.nupkg");
		foreach(var file in files)
		{
			Information("Push package: {0}", file);
			NuGetPush(file, settings);
		}
	});

Task("Default")
	.IsDependentOn("Deploy")
	.Does(() =>
	{
		Information("Hello World!");
	});

RunTarget(target);
