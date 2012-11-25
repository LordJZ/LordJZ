using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LordJZ
{
    public static class IOHelper
    {
        public const int RetryCount = 3;

        public const int SleepDelayMs = 70;

        #region Internal IO

        static T PerformIOOperation<T>(object operation)
        {
            Contract.Requires(operation != null);
            Contract.Requires(operation is Func<T>);

            return PerformIOOperation((Func<T>)operation);
        }

        static void PerformIOOperation(object operation)
        {
            Contract.Requires(operation != null);
            Contract.Requires(operation is Action);

            PerformIOOperation((Action)operation);
        }

        #endregion

        #region Synch

        /// <summary>
        /// Performs an input/output operation retrying numerous times if it fails.
        /// </summary>
        /// <param name="operation">
        /// Input/output operation to perform.
        /// </param>
        /// <exception cref="System.IO.IOException">
        /// The input/output operation has failed <see cref="RetryCount"/> times.
        /// </exception>
        /// <exception cref="System.Exception">
        /// A non-IO exception has occured while performing the operation.
        /// </exception>
        public static void PerformIOOperation(Action operation)
        {
            Contract.Requires(operation != null);

            int retries = 0;

            while (true)
            {
                if (++retries < RetryCount)
                {
                    try
                    {
                        operation();
                        return;
                    }
                    catch (IOException)
                    {
                    }
                }
                else
                {
                    // Doing out last retry, with IO exceptions.
                    operation();
                    return;
                }

                Thread.Sleep(SleepDelayMs);
            }
        }

        public static T PerformIOOperation<T>(Func<T> operation)
        {
            Contract.Requires(operation != null);

            int retries = 0;

            while (true)
            {
                if (++retries < RetryCount)
                {
                    try
                    {
                        return operation();
                    }
                    catch (IOException)
                    {
                    }
                }
                else
                {
                    // Doing out last retry, with IO exceptions.
                    return operation();
                }

                Thread.Sleep(SleepDelayMs);
            }
        }

        #endregion

        #region Async

        /// <summary>
        /// Performs an input/output operation asyncronously retrying numerous times if it fails.
        /// </summary>
        /// <param name="operation">
        /// Input/output operation to perform.
        /// </param>
        /// <exception cref="System.IO.IOException">
        /// The input/output operation has failed <see cref="RetryCount"/> times.
        /// </exception>
        /// <exception cref="System.Exception">
        /// A non-IO exception has occured while performing the operation.
        /// </exception>
        public static async Task AsyncIOOperation(Action operation)
        {
            Contract.Requires(operation != null);

            await Task.Factory.StartNew(PerformIOOperation, operation);
        }

        public static async Task<T> AsyncIOOperation<T>(Func<T> operation)
        {
            Contract.Requires(operation != null);

            return await Task<T>.Factory.StartNew(PerformIOOperation<T>, operation);
        }

        #endregion

        public static async Task<string> AsyncReadFile(string path)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(path));

            return await AsyncIOOperation(() => File.ReadAllText(path));
        }

        public static async Task AsyncWriteFile(string path, string contents)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(path));
            Contract.Requires(contents != null);

            await AsyncIOOperation(() => File.WriteAllText(path, contents));
        }

        public static async Task AsyncWriteFile(string path, byte[] contents)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(path));
            Contract.Requires(contents != null);

            await AsyncIOOperation(() => File.WriteAllBytes(path, contents));
        }
    }
}
