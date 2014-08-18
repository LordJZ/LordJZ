using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace LordJZ.Threading
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Ensures exceptions in unawaited tasks would terminate the process.
        /// </summary>
        public static async void FireAndForget(this Task task)
        {
            Contract.Requires(task != null);

            await task;
        }
    }
}
