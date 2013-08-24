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
using Example.PushPro.Client.SignalR.FakeReposoitory;
using Example.PushPro.Server.SignalR.Domain;
using PushPro.Server;

namespace Example.PushPro.Server.SignalR.Hubs
{
    public class BooksHub : PushProHub<Book>
    {
        private readonly Repository repository;

        public BooksHub()
        {
            this.repository = new Repository();
        }
        
        protected override IQbservable<Book> Source
        {
            get
            {
                int booksCount = this.repository.Books.Count();

                return Observable.Interval(TimeSpan.FromSeconds(1))
                                 .Select(i => repository.Books.FirstOrDefault(b => b.Id == i))
                                 .Where(c => c != null)
                                 .Take(booksCount)
                                 .AsQbservable();
            }
        }
    }
}