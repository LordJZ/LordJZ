using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LordJZ.Collections.Proxies;

namespace LordJZ.Linq
{
    partial class Enumerable
    {
        public static IEnumerable MakeProxy(this IEnumerable enumerable, CollectionProxyKind kind)
        {
            Contract.Requires(enumerable != null);
            Contract.Ensures(Contract.Result<IEnumerable>() != null);

            return MakeProxy(enumerable, kind, null);
        }

        public static IEnumerable MakeProxy(this IEnumerable enumerable,
                                                 CollectionProxyKind kind, bool? readOnly)
        {
            Contract.Requires(enumerable != null);
            Contract.Ensures(Contract.Result<IEnumerable>() != null);

            switch (kind)
            {
                case CollectionProxyKind.Enumerable:
                    return new EnumerableProxy(enumerable);
                case CollectionProxyKind.Collection:
                    if (true == readOnly)
                    {
                        var message = "Cannot make a read-only proxy of an IEnumerable.";
                        throw new InvalidOperationException(message);
                    }

                    if (!(enumerable is ICollection))
                        throw new ArgumentException("enumerable is not an ICollection.");

                    return new CollectionProxy((ICollection)enumerable);
                case CollectionProxyKind.List:
                    if (true == readOnly)
                    {
                        var message = "Cannot make a read-only proxy of an IEnumerable.";
                        throw new InvalidOperationException(message);
                    }

                    if (!(enumerable is IList))
                        throw new ArgumentException("enumerable is not an ICollection.");

                    return new ListProxy((IList)enumerable);
                default:
                    throw new ArgumentOutOfRangeException("kind");
            }
        }

        public static IEnumerable<T> MakeProxy<T>(this IEnumerable<T> enumerable, CollectionProxyKind kind)
        {
            Contract.Requires(enumerable != null);
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            return MakeProxy(enumerable, kind, null);
        }

        public static IEnumerable<T> MakeProxy<T>(this IEnumerable<T> enumerable,
                                                       CollectionProxyKind kind, bool? readOnly)
        {
            Contract.Requires(enumerable != null);
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            switch (kind)
            {
                case CollectionProxyKind.Enumerable:
                    return new EnumerableProxy<T>(enumerable);

                case CollectionProxyKind.Collection:
                    // If not explicit readonly
                    if (true != readOnly)
                    {
                        // Try to create mutable collection proxy
                        if (enumerable is ICollection<T>)
                            return new CollectionProxy<T>((ICollection<T>)enumerable);

                        // Handle error if explicit mutable
                        if (false == readOnly)
                            throw new ArgumentException(
                                "Cannot create mutable proxy: enumerable is not ICollection<T>."
                                );
                    }

                    // Try to create readonly collection proxy
                    if (enumerable is IReadOnlyCollection<T>)
                        return new ReadOnlyCollectionProxy<T>((IReadOnlyCollection<T>)enumerable);

                    // Handle error if explicit readonly
                    if (true == readOnly)
                        throw new ArgumentException(
                            "Cannot create readonly proxy: enumerable is not IReadOnlyCollection<T>."
                            );

                    throw new ArgumentException("Cannot create collection proxy: " +
                        "enumerable is neither an ICollection<T> nor an IReadOnlyCollection<T>.");

                case CollectionProxyKind.List:
                    // If not explicit readonly
                    if (true != readOnly)
                    {
                        // Try to create mutable list proxy
                        if (enumerable is IList<T>)
                            return new ListProxy<T>((IList<T>)enumerable);

                        // Handle error if explicit mutable
                        if (false == readOnly)
                            throw new ArgumentException(
                                "Cannot create mutable proxy: enumerable is not IList<T>."
                                );
                    }

                    // Try to create readonly list proxy
                    if (enumerable is IReadOnlyList<T>)
                        return new ReadOnlyListProxy<T>((IReadOnlyList<T>)enumerable);

                    // Handle error if explicit readonly
                    if (true == readOnly)
                        throw new ArgumentException(
                            "Cannot create readonly proxy: enumerable is not IReadOnlyList<T>."
                            );

                    throw new ArgumentException("Cannot create list proxy: " +
                        "enumerable is neither an IList<T> nor an IReadOnlyList<T>.");

                default:
                    throw new ArgumentOutOfRangeException("kind");
            }
        }
    }
}
