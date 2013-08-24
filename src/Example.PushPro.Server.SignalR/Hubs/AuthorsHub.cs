// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using System.Linq;
using System.Reactive.Linq;
using System.Web.Http.OData.Builder;
using Example.PushPro.Client.SignalR.FakeReposoitory;
using Example.PushPro.Server.SignalR.Domain;
using Microsoft.Data.Edm;
using PushPro.Server;

namespace Example.PushPro.Server.SignalR.Hubs
{
    public class AuthorsHub : PushProHub<Author>
    {
        private readonly Repository repository;

        public AuthorsHub()
        {
            this.repository = new Repository();
        }
        
        protected override IQbservable<Author> Source
        {
            get
            {
                int authorsCount = this.repository.Authors.Count();

                return Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(0.5))
                                 .Select((_, i) =>
                                             {
                                                 ++i; 
                                                 return this.repository.Authors.FirstOrDefault(a => a.Id == i); 
                                             })
                                 .Where(c => c != null)
                                 .Take(authorsCount)
                                 .AsQbservable();
            }
        }

        #region GetModel

        protected override IEdmModel GetModel()
        {
            var modelBuilder = new ODataConventionModelBuilder();

            modelBuilder.EntitySet<Author>("Authors");
            modelBuilder.EntitySet<Book>("Booka");

            return modelBuilder.GetEdmModel();
        }

        #endregion
    }
}