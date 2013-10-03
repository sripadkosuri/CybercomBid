using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Cybercom_Bid.Models;
using Microsoft.AspNet.SignalR;

namespace Cybercom_Bid.Hubs
{
    public class ChatHub : Hub
    {
        private InMemoryRepository _repository;

        public ChatHub()
        {
            _repository = InMemoryRepository.GetInstance();
        }

        #region IDisconnect and IConnected event handlers implementation

        public override Task OnDisconnected()
        {
            string userId = _repository.GetUserByConnectionId(Context.ConnectionId);
            if (userId != null)
            {
                var bidder = _repository.Users.FirstOrDefault(u => u.Id == userId);
                if (bidder != null)
                {
                    _repository.Remove(bidder);
                    return Clients.All.leaves(bidder.Id, bidder.Username, DateTime.Now);
                }
            }

            return base.OnDisconnected();
        }

        #endregion

        #region Chat event handlers

        public void UpdateBidPrice(BidPrice bidPrice)
        {
            _repository.UpdateBidPrice(bidPrice.UserName,bidPrice.Price);

            Clients.All.onBidUpdate(bidPrice);
        }

        public void Send(ChatMessage message)
        {
            if (!string.IsNullOrEmpty(message.Content))
            {
                message.Content = HttpUtility.HtmlEncode(message.Content);

                message.Timestamp = DateTime.Now;
                Clients.All.onMessageReceived(message);
            }
        }

        public void Joined()
        {
            var bidder = new Bidder()
            {
                Id = Guid.NewGuid().ToString(),
                Username = Clients.Caller.username
            };

            _repository.Add(bidder);
            _repository.AddMapping(Context.ConnectionId, bidder.Id);
            Clients.All.joins(bidder.Id, Clients.Caller.username, DateTime.Now);
        }

        public ICollection<Bidder> GetConnectedUsers()
        {
            return _repository.Users.ToList<Bidder>();
        }

        #endregion
    }

}