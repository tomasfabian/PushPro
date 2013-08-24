using System;
using System.Linq.Expressions;

namespace PushPro.Client.FluentApi
{
    public interface IObservableOrPageableOrQuueryableService<TEntity> : IPageableAndObservable<TEntity>
    {
        /// <summary>
        /// Filters the elements of an observable sequence based on a predicate on the server side.
        /// </summary>
        /// <typeparam name="TEntity">The type of the elements in the source sequence.</typeparam>
        /// <param name="predicate">A function to test each source element for a condition.</param>
        /// <returns>An observable sequence that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
        IObservableOrPageableOrQuueryableService<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
    }
}