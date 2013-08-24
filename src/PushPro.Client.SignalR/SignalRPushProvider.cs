// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using System.Collections.Generic;
using System.Linq;
using PushPro.Client.Extensions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace PushPro.Client.SignalR
{
    /// <summary>
    /// Provides a SignalR hub proxy.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class SignalRPushProvider<TEntity> : PushProvider<TEntity>
    {
        private readonly string hubName;
        private HubConnection connection;

        public SignalRPushProvider(Uri uri, string hubName) 
            : base(uri)
        {
            if (hubName == null || string.IsNullOrEmpty(hubName)) throw new ArgumentNullException("hubName");

            this.hubName = hubName;

            this.EventName = "pushMessage";
        }

        #region EventName

        public string EventName { get; set; }

        #endregion

        #region OnConnect

        protected override IObservable<TEntity> OnConnect()
        {
            return CreateHubConnection();
        }

        #endregion

        private readonly ISubject<Notification<TEntity>> subject = new Subject<Notification<TEntity>>();

        #region CreateHubConnection

        private IObservable<TEntity> CreateHubConnection()
        {
            this.connection = new HubConnection(this.Uri.GetMainComponents(),this.Uri.ToQueryDictionary());

            IHubProxy hubProxy = connection.CreateHubProxy(hubName);

            hubProxy.On(this.EventName, (Notification<TEntity> notification) => subject.OnNext(notification));

            connection.Start();

            return subject
                .UnWrapNotification()
                .Finally(this.Dispose);
        }

        #endregion

        #region OnDispose

        /// <summary>
        /// Closes the connection.
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();

            this.connection.Stop();
        }

        #endregion

    }
}