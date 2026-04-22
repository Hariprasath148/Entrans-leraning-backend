namespace learning_api.Dto
{
    public class ChatListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageTime { get; set; }
        public bool IsOnline { get; set; }
    }
}
