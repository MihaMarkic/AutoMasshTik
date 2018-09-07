#addin "Cake.Squirrel"
#tool "Squirrel.Windows&version=1.8.0" 

var target = Argument("target", "Default");
var runtime = Argument<Runtime>("runtime", Runtime.Win10);
var certificateThumbprint = Argument("thumbprint", EnvironmentVariable("SignatureCertThumbprint"));

enum Runtime
{
	Win10,
	OSX,
	Linux
}

const string Configuration = "Release";
var solutionRoot = Directory("./src/");
var appIcon = solutionRoot + File("AutoMasshTik.ico");
var solution = solutionRoot + File("AutoMasshTik.sln");
var uiRoot = solutionRoot + Directory("AutoMasshTik.UI");
var ui = uiRoot + File("AutoMasshTik.UI.csproj");
var deploymentRoot = Directory("./deployment");
var win10DeploymentRoot = deploymentRoot + Directory("win10-x64");
var macOSeploymentRoot = deploymentRoot + Directory("macOS-x64");
var linuxDeploymentRoot = deploymentRoot + Directory("linux-x64");
var nuSpecTemplate = win10DeploymentRoot + File("AutoMasshTik.nuspec.Template");
var nuSpec = win10DeploymentRoot + File("AutoMasshTik.nuspec");
var win10PublishRoot = win10DeploymentRoot + Directory("files");
var win10OutputRoot = win10DeploymentRoot + Directory("output");
var win10Nupgk = win10DeploymentRoot + Directory("nupkg");
var macOSPublishRoot = macOSeploymentRoot + Directory("files");
const string NuspecFileTemplate = "\t<file src=\".\\files\\{1}{0}\" target=\"lib\\net45\\{1}{0}\" />";

string GetRuntime(Runtime runtime)
{
	switch (runtime)
	{
		case Runtime.Win10: return "win10-x64";
		case Runtime.OSX: return "osx-x64";
		case Runtime.Linux: return "linux-x64";
		default: throw new ArgumentException(nameof(runtime), $"Unknown runtime {runtime}");
	}
}
string GetDeploymentRoot(Runtime runtime)
{
	switch (runtime)
	{
		case Runtime.Win10: return win10DeploymentRoot;
		case Runtime.OSX: return macOSeploymentRoot;
		case Runtime.Linux: return linuxDeploymentRoot;
		default: throw new ArgumentException(nameof(runtime), $"Unknown runtime {runtime}");
	}
}
string GetPublishRoot(Runtime runtime)
{
	var deploymentRoot = GetDeploymentRoot(runtime);
	return deploymentRoot + Directory("/files");
}

void SignFiles(IEnumerable<FilePath> files)
{
	if (string.IsNullOrWhiteSpace(certificateThumbprint))
	{
		Information("No thumbprint to sign");
		return;
	}
	var validFiles = files.Where(f => 
		string.Equals(f.GetExtension(), ".exe", StringComparison.OrdinalIgnoreCase)
		|| string.Equals(f.GetExtension(), ".dll", StringComparison.OrdinalIgnoreCase));
    Sign(validFiles, new SignToolSignSettings {
            TimeStampUri = new Uri("http://timestamp.digicert.com"),
            CertThumbprint = certificateThumbprint.Replace(" ", "")
    });
}

Task("Clean")
	.Does(() =>{
		DotNetCoreClean(solution);
    });
Task("Restore")
	.IsDependentOn("Clean")
	.Does(() =>{
		DotNetCoreRestore(solution);
    });
Task("Build")
	.IsDependentOn("Restore")
    .Does(() =>{
		var settings = new DotNetCoreBuildSettings
		 {
			 Configuration = Configuration,
			 NoRestore = true,
		 };
		DotNetCoreBuild(solution, settings);
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
		// currently there is no unit testing

        //var settings = new DotNetCoreTestSettings
        //{
        //    Configuration = configuration,
        //    NoBuild = true
        //};
        //foreach (string testProjectName in testProjects)
        //{
        //    var testProject = solutionRoot + Directory(testProjectName) + File($"{testProjectName}.csproj");
        //    DotNetCoreTest(testProject.Path.FullPath, settings);
        //}
    });

Task("Publish")
	.IsDependentOn("Test")
    .Does(() =>{
		var publishRoot = GetPublishRoot(runtime);
		CleanDirectory(publishRoot);
		string targetRuntime = GetRuntime(runtime);
		Information($"Publishing {runtime}");
		var settings = new DotNetCorePublishSettings
		{
			Configuration = Configuration,
			SelfContained = true,
			Runtime = targetRuntime,
			OutputDirectory = publishRoot,
			//Can't have this when buillding using runtime
			//NoRestore = true,
			//NoBuild = true,
		};
		DotNetCorePublish(ui, settings);
		if (runtime == Runtime.Win10)
		{
			AddIcon();
		}
    });

void AddIcon()
{
	var settings = new ProcessSettings
	{
		WorkingDirectory = win10DeploymentRoot.Path.FullPath,
		Arguments = @"-open .\files\AutoMasshTik.exe -action add -resource ..\..\src\automasshtik.ico -save .\files\AutoMasshTik.exe -mask ICONGROUP,MAINICON,"
	};
	using (var process = StartAndReturnProcess(@"C:\Program Files (x86)\Resource Hacker\ResourceHacker.exe", settings))
	{
		process.WaitForExit();
		if (process.GetExitCode() != 0)
		{
			throw new Exception("Failed to add icon");
		}
	}
}

void ImportExe(List<string> files, string releaseDirectory)
{
    var exeQuery = from f in System.IO.Directory.GetFiles(releaseDirectory, "*.exe")
				let name = System.IO.Path.GetFileName(f)
				orderby name
				select name;
	var exe = exeQuery.Single();
    files.Add(exe);
}

void ImportFiles(List<string> files, string releaseDirectory, string filter)
{   
	var query = from f in System.IO.Directory.GetFiles(releaseDirectory, "*." + filter)
				let name = System.IO.Path.GetFileName(f)
				where !string.Equals(System.IO.Path.GetExtension(name), ".exe", StringComparison.OrdinalIgnoreCase)
				orderby name
                select name;
    files.AddRange(query);
}

Task("NuSpec")
  .IsDependentOn("Publish")
  .Does(() =>
{
    CopyFile(File(nuSpecTemplate), nuSpec);
    var settings = new XmlPokeSettings {
            Namespaces = new Dictionary<string,string> {
                {"ns", "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"}
            }
    };
    XmlPoke(nuSpec, "/ns:package/ns:metadata/ns:version", GetVersion(), settings);
    List<string> files = new List<string>();
    ImportExe(files, win10PublishRoot);
    ImportFiles(files, win10PublishRoot, "*");
    var absoluteFiles = files.Select(f => (win10PublishRoot + File(f)).Path);
    //SignFiles(absoluteFiles.ToArray());
    var relativeFiles = files.Select(f => string.Format(NuspecFileTemplate, f, "")).ToArray();

	//var publishFiles = new List<string>();
	//ImportFiles(publishFiles, win10PublishRoot + Directory("publish"), "*");
	//var publishAbsoluteFiles = publishFiles.Select(f => (win10PublishRoot + Directory("publish") + File(f)).Path);
	//// takes too long to sign all these
	////SignFiles(absoluteFiles);
	//var publishRelativeFiles = publishFiles.Select(f => string.Format(NuspecFileTemplate, f, "publish\\"));
    XmlPoke(nuSpec, "/ns:package/ns:files", "\n" + string.Join("\n", relativeFiles) + "\n", settings);
});

Task("Nupkg")
  .IsDependentOn("NuSpec")
  .Does(() =>
	{
		NuGetPack(nuSpec, new NuGetPackSettings { OutputDirectory = win10Nupgk });
	});

string FormatNupkgName(string version)
{
    return string.Format("AutoMasshTik.{0}.nupkg", version);
}

Task("Squirrel")
  .IsDependentOn("Nupkg")
  .Does(() =>
{
    FilePath nupkgPath = win10Nupgk + File(FormatNupkgName(GetVersion()));
    //CleanDirectory(WpfClientSquirrelDeploy);
    Squirrel(nupkgPath, new SquirrelSettings 
        { 
            NoMsi = true,
            ReleaseDirectory = win10OutputRoot
        });
    SignFiles(new FilePath[]{ win10OutputRoot + File("Setup.exe") });
});

string GetVersion()
{
	return XmlPeek(ui, "/Project/PropertyGroup/AssemblyVersion");
}
Task("IncVersion")
	.Does(() => {
		string version = GetVersion();
		var parts = version.Split('.');
		parts[2] = (int.Parse(parts[2])+1).ToString();
		version = string.Join(".", parts);
		XmlPoke(ui, "/Project/PropertyGroup/AssemblyVersion", version);
		Information($"New version is {version}");
	});

Task("GetVersion")
	.Does(() => {
		string version = GetVersion();
		Information($"Version is {version}");
	});

Task("Default")
    .Does(() => { 
	Information($"thumbprint is {(EnvironmentVariable("SignatureCertThumbprint"))}");
        Information(
@"AutoMasshTik build process
IncVersion   .... increased version's build part (in AssemblyInfo.cs)
SetVersion .... sets version based on NewVersion argument (1.2.3 format)
GetVersion .... displays current version
Build      .... builds solution
Test       .... builds and tests solution
Publish    .... publish
Nupkg      .... builds, tests and packs for distributuion
SetTag     .... sets tag with current version to git repository
Squirrel   .... Creates Squirell win10 deployment files
Default    .... displays info

Arguments
	Runtime    ... Win10*, Linux, OSX
	Thumbprint ... Certificate thumbprint used to sign Setup.exe (by default taken from environment variable 'SignatureCertThumbprint'). When empty, no signature is performed.
");
});

RunTarget(target);