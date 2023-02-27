using Apache.NMS;

namespace MinimalNetworkRailOpenDataClient
{
    /*
     * This sample illustrates how to use .Net generally and C# specifically to 
     * receive and process messages from the Network Rail Open Data Platform.  Originally written by Chris Bailiss.
     * This sample makes use of the Apache NMS Messaging API - http://activemq.apache.org/nms/
     * This sample was built against v2.0.1 of the API.  
     * The Apache.NMS and Apache.NMS.ActiveMQ assemblies can be downloaded/restored into the solution using NuGet
     */

    internal class Program
    {
        private static int _miMessageCount1;
        private static int _miMessageCount2;

        static void Main(string[] args)
        {
            try
            {
                // CONNECTION SETTINGS:  In your code, move these into some form of configuration file / table
                // *** change the following lines to your user, password and feeds of interest - get this from the Network Rail Data Feeds portal *** 
                string sConnectUrl = "activemq:tcp://publicdatafeeds.networkrail.co.uk:61619?transport.receiveTimeout=30000&amp;transport.sendTimeout=30000";
                string sUser = "InsertYourUserIdHere";
                string sPassword = "InsertYourPasswordHere";
                string sTopic1 = "TRAIN_MVT_ALL_TOC";
                string sTopic2 = "VSTP_ALL";

                if ((sUser == "InsertYourUserIdHere") || (sPassword == "InsertYourPasswordHere"))
                {
                    Console.WriteLine("NETWORK RAIL OPEN DATA RECEIVER SAMPLE: ");
                    Console.WriteLine();
                    Console.WriteLine("ERROR:  Please update the source code (in the Program.cs file) to use your Network Rail Open Data credentials!");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("Network Rail Open Data Minimal Example");
                Console.WriteLine("Starting...");

                IConnectionFactory oConnectionFactory = new NMSConnectionFactory(new Uri(sConnectUrl));
                var oConnection = oConnectionFactory.CreateConnection(sUser, sPassword);
                oConnection.ClientId = sUser;
                oConnection.ExceptionListener += OnConnectionException;
                var oSession = oConnection.CreateSession();
                var oTopic1 = oSession.GetTopic(sTopic1);
                var oConsumer1 = oSession.CreateConsumer(oTopic1);
                var oTopic2 = oSession.GetTopic(sTopic2);
                var oConsumer2 = oSession.CreateConsumer(oTopic2);

                oConsumer1.Listener += OnMessageReceived1;
                oConsumer2.Listener += OnMessageReceived2;

                oConnection.Start();

                var dtRunUntil = DateTime.Now.AddSeconds(30);
                while (DateTime.Now < dtRunUntil)
                {
                    // we wait here while messages are received...
                    Thread.Sleep(50);
                }

                oConnection.Stop();

                var dtWaitUntil = DateTime.Now.AddSeconds(2);
                while (DateTime.Now < dtWaitUntil)
                {
                    Thread.Sleep(50);
                }

                Console.WriteLine("Press any key to finish");
                Console.ReadKey();

            }
            catch (Exception oException)
            {
                Console.WriteLine("FATAL ERROR:  " + oException.GetType().FullName);
                Console.WriteLine(oException.Message);
            }
        }

        private static void OnConnectionException(Exception oException)
        {
            Console.WriteLine("CONNECTION ERROR:  " + oException.GetType().FullName);
            Console.WriteLine(oException.Message);
        }

        private static void OnMessageReceived1(IMessage message)
        {
            try
            {
                OpenRailTextMessage? oMessage = null;
                if (message is ITextMessage msgText) oMessage = new OpenRailTextMessage(msgText.Text);

                if (oMessage == null) return;
                _miMessageCount1++;
                var sMessageText = oMessage == null ? "" : oMessage.Text.Length > 70 ? oMessage.Text.Substring(0, 70) : oMessage.Text;
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Total Msgs Recvd = {_miMessageCount1}: Last Msg 1 = {sMessageText}");
            }
            catch (Exception oException)
            {
                Console.WriteLine("MESSAGE ERROR:  " + oException.GetType().FullName);
                Console.WriteLine(oException.Message);
            }
        }

        private static void OnMessageReceived2(IMessage message)
        {
            try
            {
                OpenRailTextMessage? oMessage = null;
                if (message is ITextMessage msgText) oMessage = new OpenRailTextMessage(msgText.Text);

                if (oMessage == null) return;
                _miMessageCount2++;
                var sMessageText = oMessage == null ? "" : oMessage.Text.Length > 70 ? oMessage.Text.Substring(0, 70) : oMessage.Text;
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Total Msgs Recvd = {_miMessageCount2}: Last Msg 2 = {sMessageText}");
            }
            catch (Exception oException)
            {
                Console.WriteLine("MESSAGE ERROR:  " + oException.GetType().FullName);
                Console.WriteLine(oException.Message);
            }
        }
    }
}