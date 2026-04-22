namespace learning_api.Dto
{
    public class MessagesDto
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsSent { get; set; }

    }
}
