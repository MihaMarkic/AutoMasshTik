using AutoMasshTik.Engine.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMasshTik.Engine.Services.Abstract
{
    public interface IUpdater: IDisposable
    {
        Task UpdateAsync(Server[] servers, string username, string password, int port, bool useCredentials, CancellationToken ct);
    }
}
