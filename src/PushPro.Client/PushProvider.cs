// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using System.Data.Services.Client;

namespace PushPro.Client
{
    /// <summary>
    /// Proxy class for the server side push service.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class PushProvider<TEntity> : IPushProvider<TEntity>
    {
        protected readonly Uri uri;
        private readonly DataServiceContext dataServiceContext;

        #region Constructors

        protected PushProvider()
        {
            this.EntitySetName = "entities";
        }

        protected PushProvider(Uri uri)
            : this()
        {
            if (uri == null) throw new ArgumentNullException("uri");

            this.uri = uri;

            this.dataServiceContext = new DataServiceContext(uri);
        }

        #endregion

        protected string EntitySetName { get; set; }

        #region Entities

        /// <summary>
        /// Query object.
        /// </summary>
        public DataServiceQuery<TEntity> Entities
        {
            get
            {
                if (this.entities == null)
                {
                    this.entities = dataServiceContext.CreateQuery<TEntity>(this.EntitySetName);
                }

                return this.entities;
            }
        }

        private DataServiceQuery<TEntity> entities;

        #endregion

        /// <summary>
        /// Uri of the remote push provider.
        /// </summary>
        public Uri Uri { get; set; }

        #region Connect

        /// <summary>
        /// Connects to a remote push service based on the provided uri.
        /// </summary>
        /// <returns>An observable sequece of entities.</returns>
        public IObservable<TEntity> Connect()
        {
            return this.OnConnect();
        }

        #endregion

        protected abstract IObservable<TEntity> OnConnect();

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void Dispose()
        {
            this.OnDispose();
        }

        protected virtual void OnDispose()
        {
        }
    }
}