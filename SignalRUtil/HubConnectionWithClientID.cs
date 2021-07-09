using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Http;

namespace SignalRUtil
{

    public class HubConnectionWithClientID<T> : Hub<T> where T : class
    {
        public static Dictionary<string, string> ConnectionIDFromClientID = new Dictionary<string, string>();
        public static bool DebugMode = true;
        public const string CLIENT_ID_NAME = "ClientID";

        //protected ILogger<HubConnectionWithClientID<T>> logger;
        protected ILogger logger;


     
        public async Task<string[]> GetClientList( )
        {
            return ConnectionIDFromClientID.Keys.ToArray ()  ;
        }


        public string ClientID (string ConnectionID = null)
        {
            var conID = ConnectionID == null ? Context.ConnectionId : ConnectionID;
            var result = "ClientID Not found";
            ConnectionIDFromClientID.TryGetValue(conID, out result);
            return result;

        }

        public async Task<string> RegisterMe(string clientID)
        {
            string connectionID = Context.ConnectionId;

            if (!this.Context.Items.ContainsKey(CLIENT_ID_NAME))  Context.Items["clientID"] = clientID;

            if (ConnectionIDFromClientID.ContainsKey(clientID))
                ConnectionIDFromClientID[clientID] = connectionID;
            else
            {

                ConnectionIDFromClientID.Add(clientID, connectionID);
                if (DebugMode) DumpClientList(nameof (RegisterMe) );

            }

            return ($"{clientID},{connectionID}");


        }


        public void DumpClientList(string caller )
        {
            if (logger == null) return;

            logger.LogInformation($"{caller} : Clients: {String.Join(",", ConnectionIDFromClientID.Keys) }" );

 

        }


        public override Task OnConnectedAsync()
        {
  

            HttpContext ctx = this.Context.GetHttpContext();
            var hd = ctx.Request.Headers;
            if (hd.ContainsKey("ClientID"))
                RegisterMe(hd["ClientID"].ToString()).Wait();

            if (DebugMode) DumpClientList(nameof(OnConnectedAsync));


            return base.OnConnectedAsync();
        }




        public override Task OnDisconnectedAsync(Exception exception)
        {
            foreach (var c in ConnectionIDFromClientID)
            {
                if (c.Value == Context.ConnectionId)
                    ConnectionIDFromClientID.Remove(c.Key);

            }

            if (DebugMode) DumpClientList(nameof ( OnDisconnectedAsync) );



            return base.OnDisconnectedAsync(exception);
        }



    }




}
