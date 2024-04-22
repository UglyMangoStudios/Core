namespace Core.Types
{
    /// <summary>
    /// An extremely simple class that collects multiple tasks easily and allows for a wait all method
    /// </summary>
    public class MultiTask
    {
        /// <summary>
        /// The collected tasks
        /// </summary>
        private readonly List<Task> tasks = new();


        /// <summary>
        /// Creates a new task and adds it to this multitask object
        /// </summary>
        /// <param name="taskFunction">The function to return a task</param>
        public void Run(Func<Task?> taskFunction) =>
            tasks.Add(taskFunction() ?? Task.CompletedTask);


        /// <summary>
        /// Subscribes a task to the list
        /// </summary>
        /// <param name="multiTask">This object to subscribe the task to</param>
        /// <param name="task">The task to subscribe</param>
        /// <returns>The same object with the task subscribed to</returns>
        public static MultiTask operator +(MultiTask multiTask, Task task)
        {
            multiTask.tasks.Add(task);
            return multiTask;
        }

        /// <summary>
        /// Simple addition of one multitask to the other. All of b goes to a
        /// </summary>
        /// <param name="a">The first multitask that also gets returned</param>
        /// <param name="b">The other multitask to merge with</param>
        /// <returns>ZMultitask a with b's tasks</returns>
        public static MultiTask operator +(MultiTask a, MultiTask b)
        {
            a.tasks.AddRange(b.tasks);
            return a;
        }

        /// <summary>
        /// Removes a task from this object
        /// </summary>
        /// <param name="multiTask">The multitask to remove from</param>
        /// <param name="task">The task to remove</param>
        /// <returns>The original multitask object</returns>
        public static MultiTask operator -(MultiTask multiTask, Task task)
        {
            multiTask.tasks.Remove(task);
            return multiTask;
        }

        /// <summary>
        /// Awaits all of the tasks stored within this object. All tasks are then cleared.
        /// </summary>
        /// <returns>An awaitable task</returns>
        public async Task WaitAll()
        {
            await Task.WhenAll(tasks);
            tasks.Clear();
        }
    }
}
