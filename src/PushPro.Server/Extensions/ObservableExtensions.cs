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
using System.Reactive.Linq;

namespace PushPro.Server.Extensions
{
    public static class ObservableExtensions
    {
        /// <summary>
        /// Wraps the push notifications to a serializable Notification object.
        /// </summary>
        /// <param name="source">The source sequence to wrap</param>
        /// <typeparam name="TSource">Notifications value type</typeparam>
        /// <returns>The wrapped push notifications from the server.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static IObservable<PushPro.Client.Notification<TSource>> WrapNotification<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return Observable.Create<Client.Notification<TSource>>(observer => source.Subscribe(
                value =>
                {
                    var notification = new Client.Notification<TSource>()
                    {
                        Kind = NotificationKind.OnNext,
                        Value = value,
                    };

                    observer.OnNext(notification);
                }
                , error =>
                {
                    var notification = new Client.Notification<TSource>()
                    {
                        Kind = NotificationKind.OnError,
                        Exception = error,
                    };

                    observer.OnNext(notification);

                }
                , () =>
                {
                    var notification = new Client.Notification<TSource>()
                    {
                        Kind = NotificationKind.OnCompleted,
                    };

                    observer.OnNext(notification);
                }
                ));
        }
    }
}