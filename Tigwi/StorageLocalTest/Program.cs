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
            StorageTmp storage = new StorageTmp();

            storage.InitWithStupidData();
            storage.afficheDebug();
            
            Console.ReadLine();
        }
    }
}
