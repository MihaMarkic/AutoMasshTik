using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMasshTik.Engine.Services.Abstract;
using Newtonsoft.Json;
using NLog;

namespace AutoMasshTik.Engine.Services.Implementation
{
    public class WinAppUpdater : IAppUpdater
    {
        static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        const string UpdateUrl = "https://automasshtik.rthand.com/";
        static readonly string updateExe;
        static WinAppUpdater()
        {
            string currentDirectory = Path.GetDirectoryName(typeof(WinAppUpdater).Assembly.Location);
            updateExe = Path.Combine(currentDirectory, "..", "Update.exe");
            logger.Info($"Update.exe should be at at {updateExe}");
        }
        public Task<WinCheckForUpdateResult?> GetLatestVersionAsync(CancellationToken ct)
        {
            if (File.Exists(updateExe))
            {
                return Task.Run(() =>
                {
                    string textResult = null;
                    var pi = new ProcessStartInfo(updateExe, $"--checkForUpdate={UpdateUrl}");
                    pi.RedirectStandardOutput = true;
                    var p = new Process();
                    pi.UseShellExecute = false;
                    p.StartInfo = pi;
                    p.OutputDataReceived += (s, e) =>
                    {
                        Debug.WriteLine($"Checking: {e.Data}");
                        if (e.Data?.StartsWith("{") ?? false)
                        {
                            textResult = e.Data;
                        }
                    };
                    p.Start();
                    p.BeginOutputReadLine();
                    p.WaitForExit();
                    if (textResult != null)
                    {
                        logger.Info($"Updater response is: {textResult}");
                    }
                    else
                    {
                        logger.Warn("Got no meaningful response from updater");
                    }
                    if (textResult != null)
                    {
                        return JsonConvert.DeserializeObject<WinCheckForUpdateResult>(textResult);
                    }
                    else
                    {
                        return (WinCheckForUpdateResult?)null;
                    }
                });
            }
            else
            {
                logger.Warn($"Couldn't find Update.exe");
            }
            return Task.FromResult((WinCheckForUpdateResult?)null);
        }
        public Task UpdateAsync(CancellationToken ct)
        {
            if (File.Exists(updateExe))
            {
                return Task.Run(() =>
                {
                    var pi = new ProcessStartInfo(updateExe, $"--update={UpdateUrl}");
                    pi.RedirectStandardOutput = true;
                    var p = new Process();
                    pi.UseShellExecute = false;
                    p.StartInfo = pi;
                    p.OutputDataReceived += (s, e) =>
                    {
                        Debug.WriteLine($"Updating: {e.Data}");
                    };
                    p.Start();
                    p.BeginOutputReadLine();
                    p.WaitForExit();
                });
            }
            return null;
        }
    }
}
