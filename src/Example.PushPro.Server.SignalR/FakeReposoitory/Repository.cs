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
using Example.PushPro.Server.SignalR.Domain;

namespace Example.PushPro.Client.SignalR.FakeReposoitory
{
    public class Repository
    {
        readonly List<Author> authors;

        public int LastBookId;

        public Repository()
        {
            this.authors =
            Enumerable.Range(1, 10)
                      .Aggregate(new List<Author>(), (list, id) =>
                                                         {
                                                             list.Add(new Author() { Id = id, LastName = "LastName " + id });

                                                             return list;
                                                         });

            this.authors.ForEach(author
                                 =>
                                     {
                                         var book1 = new Book() { Id = ++LastBookId, Title = "Title " + LastBookId, AuthorId = author.Id };
                                         var book2 = new Book() { Id = ++LastBookId, Title = "Title " + LastBookId, AuthorId = author.Id };

                                         author.Books.AddRange(new[] { book1, book2 });
                                     });
        }

        public IQueryable<Author> Authors
        {
            get { return this.authors.AsQueryable(); }
        }

        public IQueryable<Book> Books
        {
            get { return this.authors.SelectMany(c => c.Books).AsQueryable(); }
        }
    }
}