using System;

namespace PushPro.Client.FluentApi
{
    public interface IObservablePushService<out TEntity>
    {
        /// <summary>
        /// Connects to a push provider and converts the push notifications to an observable sequence.
        /// </summary>
        /// <typeparam name="TEntity">The type of the elements in the source sequence.</typeparam>
        /// <returns>The observable sequence whose elements are pushed from the server.</returns>
        IObservable<TEntity> ToObservable();
    }
}