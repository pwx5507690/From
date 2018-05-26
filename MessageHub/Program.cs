using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using System.Linq.Expressions;
using System.Collections;
using GS.SQL.DataSource;

namespace MessageHub
{
    class Program
    {
        static void Main(string[] args)
        {
            var startUrl = System.Configuration.ConfigurationSettings.AppSettings["StartUrl"];
            using (WebApp.Start<Startup>(startUrl))
            {
                Console.WriteLine($"Server running at {startUrl} \r\n按esc退出服务");
                while (true)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                        Environment.Exit(0);
                    Console.ReadLine();
                }
            }
        }
    }
}
