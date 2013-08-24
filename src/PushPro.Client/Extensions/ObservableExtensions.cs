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

namespace PushPro.Client.Extensions
{
    public static class ObservableExtensions
    {
        /// <summary>
        /// UnWraps the push notifications from a serializable Notification object.
        /// </summary>
        /// <param name="source">The source sequence to unwrap</param>
        /// <typeparam name="TSource">Wrapped notifications value type</typeparam>
        /// <returns>The unwrapped push notifications from the server.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static IObservable<TSource> UnWrapNotification<TSource>(this IObservable<Notification<TSource>> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return Observable.Create<TSource>(observer =>
            source.Subscribe(notification =>
            {
                switch (notification.Kind)
                {
                    case NotificationKind.OnNext:
                        observer.OnNext(notification.Value);
                        break;
                    case NotificationKind.OnError:
                        observer.OnError(notification.Exception);
                        break;
                    case NotificationKind.OnCompleted:
                        observer.OnCompleted();
                        break;
                }
            }
            , observer.OnError
            , observer.OnCompleted));
        }
    }
}