var target = Argument("target", "Default");

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

Task("NuGetRestore")
	.IsDependentOn("AssemblyInfo")
	.Does(() => 
	{	
		NuGetRestore("./GraphQlDemo.sln");
	});

Task("Build")
	.IsDependentOn("NuGetRestore")
	.Does(() => 
	{	
		MSBuild("./GraphQlDemo.sln");
	});

Task("Default")
	.IsDependentOn("Build")
	.Does(() =>
	{
	  Information("You build is done!");
	});

RunTarget(target);