using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeWork_Net_N3V3
{
    // Задание. 
    // В архиве с ДЗ найдете примеры работы сервера и клиента согласно линейному жизненному циклу и пример их запуска в разных потоках в одном приложении.
    // Необходимо реализовать взаимодействие между двумя потоками через сетевые сокеты (клиент/сервер), смоделировав диалог двух сущностей.За основу брать пример с урока.
    // Диалог запустить в двух потоках.
    // Варианты для диалогов (выбрать любой один):
    //      продавец/покупатель
    //      родитель/ребенок
    //      две бабушки на скамейке
    //      общение со службой поддержки
    //      разговор студента и профессора
    // не надо делать прям сценарий, 2-3 реплики от каждой стороны.
    // Задание 2* (на 12)
    // Реализовать блок схемы алгоритма работы сервера и клиента согласно написанной программе.
    // Диаграмму можно делать в draw.io(https://app.diagrams.net/)
    // Почитать про блок схемы тут: https://ru.wikipedia.org/wiki/%D0%91%D0%BB%D0%BE%D0%BA-%D1%81%D1%85%D0%B5%D0%BC%D0%B0

    internal class Program
    {

        // Процедура запуска линейного алгоритма работы сервера (пассивного сокета)
        static void RunServer(string serverIpStr, int serverPort)
        {
            Socket server = null;
            Socket client = null;

            try
            {
                IPAddress serverIp = IPAddress.Parse(serverIpStr);
                IPEndPoint serverEndpoint = new IPEndPoint(serverIp, serverPort);

                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                server.Bind(serverEndpoint);
                server.Listen(1);
                Console.WriteLine("(Продавец в ожидании покупателя) ...");
                client = server.Accept();

                // ОБЩЕНИЕ С КЛИЕНТОМ
                string message = "-Добрый день, Вам что-нибудь подсказать?";
                client.Send(Encoding.UTF8.GetBytes(message));
                Console.WriteLine($"Продавец говорит> {message}");

                byte[] buffer = new byte[1024];
                int bytesRead = client.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Продавец слышит>\t\t\t\t\t\t\t\t\t{message}");

                message = "-Отличный выбор. Это самая новая модель этой фирмы.";
                client.Send(Encoding.UTF8.GetBytes(message));
                Console.WriteLine($"Продавец говорит> {message}");
                
                buffer = new byte[1024];
                bytesRead = client.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Продавец слышит>\t\t\t\t\t\t\t\t\t{message}");

                message = "-Новый более мощный процессор, новый блок камер и дисплей повышенной четкости.";
                client.Send(Encoding.UTF8.GetBytes(message));
                Console.WriteLine($"Продавец говорит> {message}");

                buffer = new byte[1024];
                bytesRead = client.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Продавец слышит>\t\t\t\t\t\t\t\t\t{message}");

                client.Shutdown(SocketShutdown.Both);
                Console.WriteLine("(Магазин закрывается.)");

                server.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server> При работе сервера возникло исключение: {ex.Message}");
            }
            finally
            {
                server?.Close();
                client?.Close();
            }
        }

        // Процедура запуска линейного алгоритма работы клиента (активного сокета)
        static void RunClient(string serverIpStr, int serverPort)
        {
            Socket client = null;

            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                IPAddress serverIp = IPAddress.Parse(serverIpStr);
                IPEndPoint serverEndpoint = new IPEndPoint(serverIp, serverPort);

                Console.WriteLine("(Покупатель заходит в магазин)....");
                client.Connect(serverEndpoint);
                Console.WriteLine("(Покупатель подходит к продавцу)");

                //4, ОБЩЕНИЕ С СЕРВЕРОМ
                byte[] buffer = new byte[1024];
                int bytesRead = client.Receive(buffer);
                string message = Encoding.UTF8.GetString(buffer);
                Console.WriteLine($"Покупатель слышит>\t\t\t\t\t\t\t\t\t{message}");

                message = "-Добрый, да, расскажите вот про этот телефон.";
                client.Send(Encoding.UTF8.GetBytes(message));
                Console.WriteLine($"Покупатель говорит> {message}");

                buffer = new byte[1024];
                bytesRead = client.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer);
                Console.WriteLine($"Покупатель слышит>\t\t\t\t\t\t\t\t\t{message}");

                message = "-И чем он лучше предыдущего?";
                client.Send(Encoding.UTF8.GetBytes(message));
                Console.WriteLine($"Покупатель говорит> {message}");

                buffer = new byte[1024];
                bytesRead = client.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer);
                Console.WriteLine($"Покупатель слышит>\t\t\t\t\t\t\t\t\t{message}");

                message = "-Уговорили, покупаю.";
                client.Send(Encoding.UTF8.GetBytes(message));
                Console.WriteLine($"Покупатель говорит> {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"При работе клиента возникло исключение: {ex.Message}");
            }
            finally
            {
                client?.Close();
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string serverIpStr = "127.0.0.1";
            int serverPort = 1024;

            Thread serverThread = new Thread(() => RunServer(serverIpStr, serverPort));
            Thread clientThread = new Thread(() => RunClient(serverIpStr, serverPort));

            serverThread.Start();
            clientThread.Start();

            serverThread.Join();
            clientThread.Join();
        }
    }
}
