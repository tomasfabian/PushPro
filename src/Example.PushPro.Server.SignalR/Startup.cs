// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using Microsoft.AspNet.SignalR;
using Owin;

namespace Example.PushPro.Server.SignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapHubs("Books", new HubConfiguration());
            app.MapHubs("Authors", new HubConfiguration());
        } 
    }
}