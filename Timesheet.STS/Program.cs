using System;
using Microsoft.Owin.Hosting;

namespace Timesheet.STS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("https://localhost:44333"))
            {
                Console.WriteLine("Server is running...");
                Console.ReadLine();
            }
        }
    }
}