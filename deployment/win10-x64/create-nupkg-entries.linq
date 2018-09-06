<Query Kind="Program">
  <Reference Relative="..\..\..\..\Righthand\Bolnica\Paladin\application\Sources\Paladin.Server\Paladin.Server.Frontend\bin\CommunicationRead.dll">D:\GitProjects\Righthand\Bolnica\Paladin\application\Sources\Paladin.Server\Paladin.Server.Frontend\bin\CommunicationRead.dll</Reference>
  <NuGetReference Version="3.1.6">Npgsql</NuGetReference>
  <Namespace>CommunicationRead</Namespace>
</Query>

void Main()
{
	string scriptDir = Path.GetDirectoryName (Util.CurrentQueryPath);
	string releaseDirectory = Path.Combine(scriptDir, "./files");
	
	$"\t<!-- Autogenerated using {Path.GetFileName(Util.CurrentQueryPath)} linqpad script -->".Dump();

	var exeQuery = from f in Directory.GetFiles(releaseDirectory, "*.exe")
				let name = Path.GetFileName(f)
				orderby name
				select name;
	var exe = exeQuery.Single();
	$"\t<file src=\"..\\files\\{exe}\" target=\"lib\\net45\\{exe}\" />"
			.Dump();

	ImportFiles(releaseDirectory, "dll");
	ImportFiles(Path.Combine(releaseDirectory, "publish"), "dll", "publish");
}

void ImportFiles(string releaseDirectory, string filter, string additionalPath = null)
{
	string pathExtension = string.IsNullOrWhiteSpace(additionalPath) ? "": $"{additionalPath}\\";
	var query = from f in Directory.GetFiles(releaseDirectory, $"*.{filter}")
				let name = Path.GetFileName(f)
				select name;
	foreach (string line in query)
	{
		$"\t<file src=\"..\\files\\{line}\" target=\"lib\\net45\\{pathExtension}{line}\" />"
			.Dump();
	}
}

// Define other methods and classes here