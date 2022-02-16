namespace TodoApi.Models
{
    public class TodoItemDTO
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
    }
}