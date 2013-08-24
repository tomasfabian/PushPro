// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System.Collections.Generic;

namespace Example.PushPro.Server.SignalR.Domain
{
    public class Book : DomainObject
    {
        public string Title
        {
            get; set;
        }

        public int AuthorId { get; set; }
    }
}