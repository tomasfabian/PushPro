using System;
using Example.PushPro.Server.SignalR.Domain;
using PushPro.Client;
using PushPro.Client.SignalR;

namespace Example.PushPro.Client.SignalR
{
    class Program
    {
        static void Main(string[] args)
        {
            var booksSubscription = SubscribeToBooks();
            var authorsSubscription = SubscribeToAuthors();

            Console.ReadKey();

            using (authorsSubscription)
            using (booksSubscription)
            {
            }
        }

        private static IDisposable SubscribeToBooks()
        {
            var pushProvider = new SignalRPushProvider<Book>(new Uri(@"http://localhost:49895/api/Books"), "BooksHub");

            var pushService = new PushService<Book>(pushProvider);

            return pushService
                .Where(c => c.Id != 5)
                .Skip(1)
                .Take(7)
                .ToObservable()
                .Subscribe(
                    value => Console.WriteLine("Id: " + value.Id + " Book Title: '" + value.Title + "'"),
                    error => Console.WriteLine("Books error: " + error.Message),
                    () => Console.WriteLine("Books Completed"));
        }

        private static IDisposable SubscribeToAuthors()
        {
            var pushProvider = new SignalRPushProvider<Author>(new Uri(@"http://localhost:49895/api/Authors"), "AuthorsHub");

            var pushService = new PushService<Author>(pushProvider);

            return pushService
                .ToObservable()
                .Subscribe(
                    value => Console.WriteLine("Id: " + value.Id + " Author: '" + value.LastName + "' Books count: " + value.Books.Count),
                    error => Console.WriteLine("Authors error: " + error.Message),
                    () => Console.WriteLine("Authors Completed"));
        }
    }
}
