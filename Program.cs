using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Keyence_Comm_csharp
{
    class Program
    {
        public static void StartClient()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

                //IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPAddress ipAddress = System.Net.IPAddress.Parse("192.168.1.83"); //specific IP addressing

                //IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8500);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    //* THIS IS WHERE KEYENCE MESSENGE IS CREATED / SENT *
                    // Encode the data string into a byte array.  
                    //byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");
                    //byte[] msg = Encoding.ASCII.GetBytes("T1\r\n"); //T1 trigger message example
                    byte[] msg = Encoding.ASCII.GetBytes("MR,%Busy\r\n");

                    // Send the data through the socket.  
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.  
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        static int Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            StartClient();
            return 0;
        }
    }
}
