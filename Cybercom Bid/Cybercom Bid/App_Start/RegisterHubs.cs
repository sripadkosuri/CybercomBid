using System.Web;
using Microsoft.AspNet.SignalR;
using System.Web.Routing;

[assembly: PreApplicationStartMethod(typeof(Cybercom_Bid.RegisterHubs), "Start")]

namespace Cybercom_Bid
{
    public class RegisterHubs
    {
        public static void Start()
        {
            // Register the default hubs route: ~/signalr/hubs
            RouteTable.Routes.MapHubs();
        }
    }
}

