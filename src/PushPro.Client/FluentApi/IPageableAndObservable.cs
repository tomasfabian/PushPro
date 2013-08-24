namespace PushPro.Client.FluentApi
{
    public interface IPageableAndObservable<out TEntity> : IObservablePushService<TEntity>
    {
        /// <summary>
        /// Returns a specified number of contiguous elements from the start of an observable sequence.
        /// </summary>
        /// <typeparam name="TEntity">The type of the elements in the source sequence.</typeparam>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>An observable sequence that contains the specified number of elements from the start of the input sequence.</returns>
        IPageableAndObservable<TEntity> Take(int count);

        /// <summary>
        /// Bypasses a specified number of elements in an observable sequence on the server side and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="TEntity">The type of the elements in the source sequence.</typeparam>
        /// <param name="count">The number of elements to skip before returning the remaining elements from the server.</param>
        /// <returns>An observable sequence that contains the elements that occur after the specified index in the input sequence.</returns>
        IPageableAndObservable<TEntity> Skip(int count);
    }
}