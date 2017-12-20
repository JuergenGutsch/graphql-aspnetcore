#addin "nuget:?package=NuGet.Core&version=2.14.0"
#addin nuget:?package=Cake.ArgumentHelpers
#addin "Cake.ExtendedNuGet"
using Cake.ExtendedNuGet;

var target = Argument("target", "Default");
string nugetSource, apiKey;

var branch = ArgumentOrEnvironmentVariable("branch", String.Empty, "none");
Information($"branch is {branch}!");
if (branch == "master")
{
	nugetSource = ArgumentOrEnvironmentVariable("NuGetFeed", String.Empty, String.Empty);
	apiKey = ArgumentOrEnvironmentVariable("NuGetFeed", String.Empty, String.Empty);
}
if (branch == "develop")
{
	nugetSource = ArgumentOrEnvironmentVariable("MyGetFeed", String.Empty, String.Empty);
	apiKey = ArgumentOrEnvironmentVariable("MyGetApiKey", String.Empty, String.Empty);
}

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
		Information("Build Project!");

		var exitCodeWithArgument = StartProcess("dotnet", "build  GraphQlDemo.sln -c Release");

	});

Task("Test")
	.IsDependentOn("Build")
	.Does(() =>
	{
		Information("Test Projects!");
		Information("No tests yet :-(");
	});

Task("Pack")
	.WithCriteria(() => branch != "none" )
	.IsDependentOn("Test")
	.Does(() =>
	{
		Information("Publish Libraries!");

		StartProcess("dotnet", "pack  GraphQl.AspNetCore -c Release -o ../artifacts");
		StartProcess("dotnet", "pack  GraphQl.AspNetCore.Graphiql -c Release -o ../artifacts");
	});

Task("Deploy")
	.WithCriteria(() => branch != "none" )
	.IsDependentOn("Pack")
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
