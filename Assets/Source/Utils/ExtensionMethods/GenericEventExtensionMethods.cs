using System;

namespace GGS.CakeBox.Utils
{
    #region delegates

    public delegate void GenericEvent();

    public delegate void GenericEvent<T>(T t);

    public delegate void GenericEvent<T, U>(T t, U u);

    public delegate void GenericEvent<T, U, V>(T t, U u, V v);

    public delegate void GenericEvent<T, U, V, W>(T t, U u, V v, W w);

    public delegate void GenericEvent<T, U, V, W, X>(T t, U u, V v, W w, X x);

    public delegate void GenericEvent<T, U, V, W, X, Y>(T t, U u, V v, W w, X x, Y y);

    public delegate void GenericEvent<T, U, V, W, X, Y, Z>(T t, U u, V v, W w, X x, Y y, Z z);

    #endregion

    /// <summary>
    /// A class to provide a typesafe and exception safe way to dispatch the GenericEvent delegates.
    /// </summary>
    public static class GenericEventExtensions
    {
        #region dispatch methods

        /// <summary>
        /// A GenericEvent extension method that dispatch the given delegate.
        /// </summary>
        /// <param name="del">  The delegate to act on. </param>
        public static void Fire(this GenericEvent del)
        {
            del.UnsafeFire();
        }

        /// <summary>
        /// A GenericEvent&lt;T&gt; extension method that dispatch the given delegate.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="del">  The delegate to act on. </param>
        /// <param name="t">    The T to process. </param>
        public static void Fire<T>(this GenericEvent<T> del, T t)
        {
            del.UnsafeFire(t);
        }

        /// <summary>
        /// A GenericEvent&lt;T&gt; extension method that dispatches the given delegate.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <typeparam name="U">    Generic type parameter. </typeparam>
        /// <param name="del">  The delegate to act on. </param>
        /// <param name="t">    The T to process. </param>
        /// <param name="u">    The U to process. </param>
        public static void Fire<T, U>(this GenericEvent<T, U> del, T t, U u)
        {
            del.UnsafeFire(t, u);
        }

        /// <summary>
        /// A GenericEvent&lt;T&gt; extension method that dispatches the given delegate.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <typeparam name="U">    Generic type parameter. </typeparam>
        /// <typeparam name="V">    Generic type parameter. </typeparam>
        /// <param name="del">  The delegate to act on. </param>
        /// <param name="t">    The T to process. </param>
        /// <param name="u">    The U to process. </param>
        /// <param name="v">    The V to process. </param>
        public static void Fire<T, U, V>(this GenericEvent<T, U, V> del, T t, U u, V v)
        {
            del.UnsafeFire(t, u, v);
        }

        /// <summary>
        /// A GenericEvent&lt;T&gt; extension method that dispatches the given delegate.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <typeparam name="U">    Generic type parameter. </typeparam>
        /// <typeparam name="V">    Generic type parameter. </typeparam>
        /// <typeparam name="W">    Generic type parameter. </typeparam>
        /// <param name="del">  The delegate to act on. </param>
        /// <param name="t">    The T to process. </param>
        /// <param name="u">    The U to process. </param>
        /// <param name="v">    The V to process. </param>
        /// <param name="w">    The W to process. </param>
        public static void Fire<T, U, V, W>(this GenericEvent<T, U, V, W> del, T t, U u, V v, W w)
        {
            del.UnsafeFire(t, u, v, w);
        }

        /// <summary>
        /// A GenericEvent&lt;T&gt; extension method that dispatches the given delegate.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <typeparam name="U">    Generic type parameter. </typeparam>
        /// <typeparam name="V">    Generic type parameter. </typeparam>
        /// <typeparam name="W">    Generic type parameter. </typeparam>
        /// <typeparam name="X">    Generic type parameter. </typeparam>
        /// <param name="del">  The delegate to act on. </param>
        /// <param name="t">    The T to process. </param>
        /// <param name="u">    The U to process. </param>
        /// <param name="v">    The V to process. </param>
        /// <param name="w">    The W to process. </param>
        /// <param name="x">    The X to process. </param>
        public static void Fire<T, U, V, W, X>(this GenericEvent<T, U, V, W, X> del, T t, U u, V v, W w, X x)
        {
            del.UnsafeFire(t, u, v, w, x);
        }

        /// <summary>
        /// A GenericEvent&lt;T&gt; extension method that dispatches the given delegate.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <typeparam name="U">    Generic type parameter. </typeparam>
        /// <typeparam name="V">    Generic type parameter. </typeparam>
        /// <typeparam name="W">    Generic type parameter. </typeparam>
        /// <typeparam name="X">    Generic type parameter. </typeparam>
        /// <typeparam name="Y">    Generic type parameter. </typeparam>
        /// <param name="del">  The delegate to act on. </param>
        /// <param name="t">    The T to process. </param>
        /// <param name="u">    The U to process. </param>
        /// <param name="v">    The V to process. </param>
        /// <param name="w">    The W to process. </param>
        /// <param name="x">    The X to process. </param>
        /// <param name="y">    The Y to process. </param>
        public static void Fire<T, U, V, W, X, Y>(this GenericEvent<T, U, V, W, X, Y> del, T t, U u, V v, W w, X x, Y y)
        {
            del.UnsafeFire(t, u, v, w, x, y);
        }

        /// <summary>
        /// A GenericEvent&lt;T&gt; extension method that dispatches the given delegate.
        /// </summary>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <typeparam name="U">    Generic type parameter. </typeparam>
        /// <typeparam name="V">    Generic type parameter. </typeparam>
        /// <typeparam name="W">    Generic type parameter. </typeparam>
        /// <typeparam name="X">    Generic type parameter. </typeparam>
        /// <typeparam name="Y">    Generic type parameter. </typeparam>
        /// <typeparam name="Z">    Generic type parameter. </typeparam>
        /// <param name="del">  The delegate to act on. </param>
        /// <param name="t">    The T to process. </param>
        /// <param name="u">    The U to process. </param>
        /// <param name="v">    The V to process. </param>
        /// <param name="w">    The W to process. </param>
        /// <param name="x">    The X to process. </param>
        /// <param name="y">    The Y to process. </param>
        /// <param name="z">    The Z to process. </param>
        public static void Fire<T, U, V, W, X, Y, Z>(this GenericEvent<T, U, V, W, X, Y, Z> del, T t, U u, V v, W w, X x, Y y, Z z)
        {
            del.UnsafeFire(t, u, v, w, x, y, z);
        }

        #endregion

        /// <summary>
        /// A Delegate extension method to dispatch the delegate (type unsafe).
        /// </summary>
        /// <param name="del">  The delegate to act on. </param>
        /// <param name="args"> A variable-length parameters list containing arguments. </param>
        private static void UnsafeFire(this Delegate del, params object[] args)
        {
            if (del == null)
            {
                return;
            }

            Delegate[] delegates = del.GetInvocationList();

            // the mono compiler can handle "foreach" into "for" for arrays
            foreach (Delegate subscriber in delegates)
            {
                //try
                //{
                subscriber.DynamicInvoke(args);
                //}
                //catch (Exception innerException)
                //{
                //    throw new GenericEventHandlerInvokeException(
                //        "Could not invoke '" + subscriber.Method + "' for the target" +
                //        subscriber.Target + ". The target method threw the following exception: " +
                //        innerException.Message); //,innerException);
                //}
            }
        }
    }
}