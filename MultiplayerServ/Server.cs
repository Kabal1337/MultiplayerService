using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MultiplayerServ
{
    class Server
    {
        static int port = 8005; //порт для приема входящих запросов
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

                //начало прослушивания подключений
                listenSocket.Listen(10); //10 - максимальная длина очереди подключений
                Console.WriteLine("Сервер запущен. Ожидание подключений...");
                while (true)
                {
                    //получение сообщения
                    Socket handler = listenSocket.Accept();
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
                    string message = "ваше сообщение доставлено";
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



        }
    }
}
