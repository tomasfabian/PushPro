using System;
using System.Data.Services.Client;

namespace PushPro.Client
{
    public interface IPushProvider<TEntity> : IDisposable
    {
        IObservable<TEntity> Connect();
        DataServiceQuery<TEntity> Entities { get; }
        Uri Uri { get; set; }
    }
}