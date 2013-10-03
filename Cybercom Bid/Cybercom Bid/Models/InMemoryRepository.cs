using System;
using System.Collections.Generic;
using System.Linq;

namespace Cybercom_Bid.Models
{
    public class BidPrice
    {
        public string UserName { get; set; }
        public int Price { get; set; }
    }

    public class InMemoryRepository
    {
        private static ICollection<Bidder> _connectedUsers;
        private static Dictionary<string, string> _mappings;
        private static InMemoryRepository _instance = null;
        private const int max_random = 3;
        private static BidPrice _bidPrice;

        public static InMemoryRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new InMemoryRepository();
            }
            return _instance;
        }

        #region Private methods

        private InMemoryRepository()
        {
            _connectedUsers = new List<Bidder>();
            _mappings = new Dictionary<string, string>();
            _bidPrice = new BidPrice();
        }

        #endregion

        #region Repository methods

        public IQueryable<Bidder> Users { get { return _connectedUsers.AsQueryable(); } }

        public void UpdateBidPrice(string username,int bidPrice)
        {
            _bidPrice.Price =bidPrice;
            _bidPrice.UserName = username;
        }
        public void Add(Bidder user)
        {
            _connectedUsers.Add(user);
        }

        public void Remove(Bidder user)
        {
            _connectedUsers.Remove(user);
        }

        public string GetRandomizedUsername(string username)
        {
            var tempUsername = username;
            var newRandom = max_random;
            var oldRandom = 0;
            var loops = 0;
            var random = new Random();
            do
            {
                if (loops > newRandom)
                {
                    oldRandom = newRandom;
                    newRandom *= 2;
                }
                username = tempUsername + "_" + random.Next(oldRandom, newRandom).ToString();
                loops++;
            } while (GetInstance().Users.Any(u => u.Username.Equals(username)));

            return username;
        }

        public void AddMapping(string connectionId, string userId)
        {
            if (!string.IsNullOrEmpty(connectionId) && !string.IsNullOrEmpty(userId))
            {
                _mappings.Add(connectionId, userId);
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            string userId = null;
            _mappings.TryGetValue(connectionId, out userId);
            return userId;
        }

        #endregion
    }
}