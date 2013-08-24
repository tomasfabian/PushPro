// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using System.Reactive;

namespace PushPro.Client
{
    /// <summary>
    /// Serializable push notifivation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Notification<T>
    {
        public T Value { get; set; }

        public Exception Exception { get; set; }

        public NotificationKind Kind { get; set; }
    }
}