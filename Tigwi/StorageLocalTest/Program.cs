using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageCommon;
using StorageLibrary;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageLocalTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Storage storage = new Storage("ulyssestorage", "");

            Console.WriteLine("Init ended");
            
            using(Mutex m1 = new Mutex(storage.connexion.userContainer, "locklogin/main"))
            {
                Console.WriteLine("m1 taken");

                using (Mutex m2 = new Mutex(storage.connexion.userContainer, "locklogin/main"))
                {
                    Console.WriteLine("m2 taken");
                }
                Console.WriteLine("m2 ok");
            }
            Console.WriteLine("m1 ok");

            Console.ReadLine();
        }
    }
}
