using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerServ
{
    class Server
    {
        static int port = 8005; //порт для приема входящих запросов
        List<string> messages_list = new List<string>();
        
        static void Main(string[] args)
        {
            
            //адрес локальной точки, с которой будет связан сокет
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

            //создаие сокета
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //свзять сокета с локальной точкой
                listenSocket.Bind(ipPoint);
                List<Socket> socket_list = new List<Socket>();
                //начало прослушивания подключений
                listenSocket.Listen(10); //10 - максимальная длина очереди подключений
                Console.WriteLine("Сервер запущен. Ожидание подключений...");
                while (true)
                {
                    //получение сообщения
                    Socket handler_tmp = listenSocket.Accept();
                    if (handler_tmp != null)
                    {
                        int socket_num = socket_list.Count;
                        Task task = Task.Factory.StartNew(() => SocketDataHandler(handler_tmp, socket_list, socket_num)); //запуск отдельным потоком обработку сокета.
                        socket_list.Add(handler_tmp);
                        
                        //if (handler_tmp != null)
                        //socket_list.Add(handler_tmp);
                        //Socket handler = handler_tmp;
                    }
                    Console.WriteLine("Число подключений: " + socket_list.Count);
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            void SocketDataHandler(Socket handler, List<Socket> socket_list, int socket_num)
            {
                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; //кол-во полученныйх байт
                    byte[] data = new byte[256]; //буфер данных
                                                 //сборка сообщения
                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);
                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());
                    // отправляем ответ
                    //string message = "ваше сообщение доставлено";
                    data = Encoding.Unicode.GetBytes(builder.ToString());
                    for (int i = 0; i < socket_list.Count; i++)
                    {
                        if (i != socket_num) socket_list[i].Send(data);
                    }
                    //handler.Send(data);
                    // закрываем сокет
                    //handler.Shutdown(SocketShutdown.Both);
                    //handler.Close();
                }
            }

        }

        
    }
}
