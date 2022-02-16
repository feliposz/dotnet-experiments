namespace TodoApi.Models
{
    public class TodoItem
    {
        /// <summary>
        /// Identifier for a task
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Task name/description
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Task has been completed
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Some secret data kept by the application
        /// </summary>
        public string? Secret { get; set; }
    }
}