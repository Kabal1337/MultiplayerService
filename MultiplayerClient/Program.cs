using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace MultiplayerClient
{
    class Client
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8005; // порт сервера
        static string address = "127.0.0.1"; // адрес сервера
        
        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
                //Console.Write("Введите сообщение:");
                StreamReader stream = new StreamReader("ld.json");
                
                while (true)
                { 
                    
                    string message = stream.ReadToEnd();
                    stream.BaseStream.Position=0;
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    socket.Send(data);
                    data = new byte[256]; // буфер для ответа
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байт

                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (socket.Available > 0);
                    using (StreamWriter stream_serv = new StreamWriter("sd.json", false, Encoding.ASCII))
                    {
                        stream_serv.WriteLine(builder.ToString());
                    }
                    //stream_serv.Close();
                }
                // закрываем сокет
                //socket.Shutdown(SocketShutdown.Both);
                //socket.Close();
                //Console.WriteLine("SocketClosed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
