using System;
using System.Data.Services.Client;
using System.Linq;
using System.Linq.Expressions;
using PushPro.Client.FluentApi;

namespace PushPro.Client
{
    /// <summary>
    /// Proxy class for the server side push service.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PushService<TEntity> : IObservableOrPageableOrQuueryableService<TEntity>
    {
        private readonly IPushProvider<TEntity> pushProvider;
        private IQueryable<TEntity> queryable;

        public PushService(IPushProvider<TEntity> pushProvider)
        {
            if (pushProvider == null) throw new ArgumentNullException("pushProvider");

            this.pushProvider = pushProvider;
        }

        #region PushProvider

        /// <summary>
        /// The provider of push notifications.
        /// </summary>
        public IPushProvider<TEntity> PushProvider
        {
            get { return pushProvider; }
        }

        #endregion

        #region ToObservable

        /// <summary>
        /// Connects to a push provider and converts the push notifications to an observable sequence.
        /// </summary>
        /// <typeparam name="TEntity">The type of the elements in the source sequence.</typeparam>
        /// <returns>The observable sequence whose elements are pushed from the server.</returns>
        public IObservable<TEntity> ToObservable()
        {
            var dataServiceQuery = this.queryable as DataServiceQuery<TEntity>;
            if (dataServiceQuery != null)
            {
                pushProvider.Uri = dataServiceQuery.RequestUri;
            }
            else
            {
                pushProvider.Uri = this.PushProvider.Entities.RequestUri;
            }

            return this.PushProvider.Connect();
        }

        #endregion

        #region Where

        /// <summary>
        /// Filters the elements of an observable sequence based on a predicate on the server side.
        /// </summary>
        /// <typeparam name="TEntity">The type of the elements in the source sequence.</typeparam>
        /// <param name="predicate">A function to test each source element for a condition.</param>
        /// <returns>An observable sequence that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
        public IObservableOrPageableOrQuueryableService<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            if (queryable == null)
            {
                queryable = this.PushProvider.Entities.Where(predicate);
            }
            else
            {
                queryable = queryable.Where(predicate);
            }

            return this;
        }

        #endregion

        #region Take
        
        /// <summary>
        /// Returns a specified number of contiguous elements from the start of an observable sequence.
        /// </summary>
        /// <typeparam name="TEntity">The type of the elements in the source sequence.</typeparam>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>An observable sequence that contains the specified number of elements from the start of the input sequence.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
        public IPageableAndObservable<TEntity> Take(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if (queryable == null)
            {
                queryable = this.PushProvider.Entities.Take(count);
            }
            else
            {
                queryable = queryable.Take(count);
            }

            return this;
        }

        #endregion

        #region Skip

        /// <summary>
        /// Bypasses a specified number of elements in an observable sequence on the server side and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="TEntity">The type of the elements in the source sequence.</typeparam>
        /// <param name="count">The number of elements to skip before returning the remaining elements from the server.</param>
        /// <returns>An observable sequence that contains the elements that occur after the specified index in the input sequence.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
        public IPageableAndObservable<TEntity> Skip(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
         
            if (queryable == null)
            {
                queryable = this.PushProvider.Entities.Skip(count);
            }
            else
            {
                queryable = queryable.Skip(count);
            }

            return this;
        }

        #endregion
    }

    /// <summary>
    /// Factory for the push service.
    /// </summary>
    public static class PushService
    {
        /// <summary>
        /// Creates an instance of PushService class.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="pushProvider">The push provider.</param>
        /// <returns>Observable push service with server side filtering and paging.</returns>
        public static IObservablePushService<TEntity> Create<TEntity>(IPushProvider<TEntity> pushProvider)
        {
            return new PushService<TEntity>(pushProvider);
        }
    }
}