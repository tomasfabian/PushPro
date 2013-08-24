// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using PushPro.Server.Extensions;
using System.Threading.Tasks;
using System.Web.Http.OData;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Query;
using Microsoft.AspNet.SignalR;
using Microsoft.Data.Edm;

namespace PushPro.Server
{
    /// <summary>
    /// Provides methods that communicate with SignalR connections that connected to a Microsoft.AspNet.SignalR.Hub.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class PushProHub<TEntity> : Hub
        where TEntity : class, new()
    {
        public void Send<TItemType>(string connectionId, PushPro.Client.Notification<TItemType> item)
        {
            Clients.Client(connectionId).pushMessage(item);
        }

        protected abstract IQbservable<TEntity> Source { get; }

        private static readonly ConcurrentDictionary<string, IDisposable> Subscriptions = new ConcurrentDictionary<string, IDisposable>();

        #region GetModel

        protected virtual IEdmModel GetModel()
        {
            var modelBuilder = new ODataConventionModelBuilder();

            string name = typeof (TEntity).Name.ToLower() + "s";
            modelBuilder.EntitySet<TEntity>(name);

            return modelBuilder.GetEdmModel();
        }

        #endregion

        #region OnConnected

        public override Task OnConnected()
        {
            return Task.Factory.StartNew(() =>
            {
                var connectionId = this.Context.ConnectionId;

                if (Subscriptions.ContainsKey(connectionId))
                {
                    return;
                }

                var queryOptions =
                    new ODataQueryOptions(
                        new ODataQueryContext(this.GetModel(), typeof (TEntity)),
                        new HttpRequestMessage(HttpMethod.Get, Context.Request.Url));

                var querySettings = new ODataQuerySettings
                {
                    EnsureStableOrdering = false
                };

                var source =
                    queryOptions.ApplyTo(this.Source.ToQueryable(), querySettings) as IQueryable<TEntity>;

                var observer = CreateObserver(connectionId);

                var subscription = source
                    .ToObservable()
                    .WrapNotification()
                    .SubscribeOn(TaskPoolScheduler.Default)
                    .Subscribe(observer);

                Subscriptions.TryAdd(connectionId, subscription);
            });
        }

        #endregion
        
        #region CreateObserver

        private IObserver<PushPro.Client.Notification<TEntity>> CreateObserver(string clientId)
        {
            return Observer.Create<PushPro.Client.Notification<TEntity>>(x => Send(clientId, x),
            ex =>
            {
                //TODO:log error
            }
            , () =>
            {
            });
        }
	
        #endregion

        #region OnDisconnected

        public override Task OnDisconnected()
        {
            IDisposable disposable;

            Subscriptions.TryRemove(this.Context.ConnectionId, out disposable);

            using (disposable)
            {
            }

            return base.OnDisconnected();
        }

        #endregion
    }
}