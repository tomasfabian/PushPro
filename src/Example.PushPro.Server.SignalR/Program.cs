using System;
using Microsoft.Owin.Hosting;

namespace Example.PushPro.Server.SignalR
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:49895/api/"))
            {
                Console.WriteLine("Server running at http://localhost:49895/api");
                Console.ReadLine();
            }
        }
    }
}
