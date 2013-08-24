PushPro
=====================

PushPro is a library for push notifications that can be filtered and paged on the server side.
It is based on ASP.NET SignalR self hosting and hub connections. The query operators
are based on ASP.NET WEB API OData.

Push your notifications from the server:

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
                                 .Select((i) => repository.Books.FirstOrDefault(b => b.Id == i))
                                 .Where(c => c != null)
                                 .Take(booksCount)
                                 .AsQbservable();
            }
        }
    }

and observe them on the client side. The filter (Where), skip and top (Take) operators are
performed on the server side.

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

The book with an Id of 5 and the first pushed book will never be sent to this client
and it will receive only the maximum specified number (7) of book notifications:

 

For more information see Wiki
