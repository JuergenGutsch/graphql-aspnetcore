var target = Argument("target", "Default");


Task("DotNetCoreClean")
	.Does(() => {
		DotNetCoreClean("./GraphQlDemo.sln");
	});

Task("AssemblyInfo")
	.Does(() =>
	{
		//var file = "./SolutionInfo.cs";
		//var version = "1.0.0";
		//var buildNo = "1";
		//var settings = new AssemblyInfoSettings {
		//	Company = "CBRE",
		//	Copyright = string.Format("Copyright (c) CBRE {0}", DateTime.Now.Year),
		//	ComVisible = false,
		//	Version = version,
		//	FileVersion = version,
		//	InformationalVersion = version
		//};
		//CreateAssemblyInfo(file, settings);
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
		DotNetCoreBuild("./GraphQlDemo.sln");
	});

Task("DotNetCoreTest")
	.IsDependentOn("DotNetCoreBuild")
	.Does(() => {
		// nothing yet
		DotNetCoreTest("./GraphQlDemo.sln");
	});

Task("Default")
	.IsDependentOn("DotNetCoreBuild")
	.Does(() =>
	{
	  Information("You build is done!");
	});

RunTarget(target);