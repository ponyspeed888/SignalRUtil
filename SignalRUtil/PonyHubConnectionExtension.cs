using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;

using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRUtil
{
    public static class PonyHubConnectionExtension
    {

        public static async Task StartAsyncWithClientID (this HubConnection connection , object param)
        {
            await connection.StartAsync();
            await connection.InvokeAsync("RegisterMe", "Desktop");

  
        }

    }

    public static class PonyHubConnectionBuilderExtensions
    {
        
        public static IHubConnectionBuilder WithUrlAndClientID(this IHubConnectionBuilder hubConnectionBuilder, string url, string ClientID)

        {

            return hubConnectionBuilder.WithUrl(url, opt =>
            {
                opt.Headers.Add(SignalRUtil.HubConnectionWithClientID<string>.CLIENT_ID_NAME, ClientID);
            });

        }

    }


}
