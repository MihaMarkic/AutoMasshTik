using System.Threading;
using System.Threading.Tasks;

namespace AutoMasshTik.Engine.Services.Abstract
{
    public struct WinCheckForUpdateResult
    {
        public string CurrentVersion { get; set; }
        public string FutureVersion { get; set; }
        public ReleaseToApply[] ReleasesToApply { get; set; }
    }
    public struct ReleaseToApply
    {
        public string Version { get; set; }
        public string ReleaseNotes { get; set; }
    }
    public interface IAppUpdater
    {
        Task<WinCheckForUpdateResult?> GetLatestVersionAsync(CancellationToken ct);
        Task UpdateAsync(CancellationToken ct);
    }
}
