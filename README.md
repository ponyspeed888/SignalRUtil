Out of box SignalR manage connection by ConnectionId, but most use case require some kind of human readable ConnectionId, and this is what this project is about.  At the moment, only strongly typed Hub is supported, and only .NET client is implemented

How to use

1. Reference the project or nuget package
2. On the server side, dervied you Hub class from HubConnectionWithClientID<T>

       public class MyHub : HubConnectionWithClientID <IChromeExt>

3. On the client side create a HubConnection like this :


            connection = new HubConnectionBuilder()
                 .WithUrlAndClientID("http://localhost:5000/ChromeExtHubBase", "yourconnectionid")
                .Build();
    
    alternatively use :

            connection.StartAsyncWithClientID("yourconnectionid");
 
4. Now you can lookup ClientID from connectionId by looking at the dictionary defined in HubConnectionWithClientID class

     public static Dictionary<string, string> ConnectionIDFromClientID = new Dictionary<string, string>();
  
5. Call ClientID () to get the client ID of current connectionId
