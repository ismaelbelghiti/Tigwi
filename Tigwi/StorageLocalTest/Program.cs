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
            Mutex m1 = new Mutex(storage.connexion.userContainer, "lock/1");
            Mutex m2 = new Mutex(storage.connexion.userContainer, "lock/1");

            Console.WriteLine("Init ended");
            m1.Acquire();
            Console.WriteLine("m1 ok");
            m2.Acquire();
            Console.WriteLine("m2 ok");
        }
    }
}
