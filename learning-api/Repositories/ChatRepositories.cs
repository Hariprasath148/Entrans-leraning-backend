using learning_api.Data;
using learning_api.Dto;
using learning_api.Models;
using Microsoft.EntityFrameworkCore;

namespace learning_api.Repositories
{
    public class ChatRepositories : IChatRepositories
    {
        private readonly AppDbContext _context;

        public ChatRepositories(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ChatListDto>> GetChatUsers(int userId)
        {
            var userMessages = _context.ChatMessage
                                       .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                                       .Select(m => new
                                       {
                                            m.Id,
                                            OtherUserId = m.SenderId == userId ? m.ReceiverId : m.SenderId,
                                            m.Message,
                                            m.Timestamp
                                       });

            var lastPerUser = userMessages.GroupBy(m => m.OtherUserId)
                                          .Select(g => new
                                          {
                                                OtherUserId = g.Key,
                                                LastTimestamp = g.Max(x => x.Timestamp)
                                          });

            var lastMessages = lastPerUser.Join(userMessages,
                                            l => new { l.OtherUserId, l.LastTimestamp },
                                            m => new { OtherUserId = m.OtherUserId, LastTimestamp = m.Timestamp },
                                            (l, m) => m);

            var result = await lastMessages
                .Join(_context.Users,
                      m => m.OtherUserId,
                      u => u.Id,
                      (m, u) => new ChatListDto
                      {
                          Id = u.Id,
                          Name = u.Name,
                          LastMessage = m.Message,
                          LastMessageTime = m.Timestamp
                      })
                .OrderByDescending(x => x.LastMessageTime)
                .ToListAsync();

            return result;
        }
    }
}
