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
            Storage storage = new Storage("ulyssestorage", "REPLACE__MY_KEY");
            
            Console.WriteLine("init OK");
            
            storage.User.Create("ulysse", "ulysse.beaugnon@free.fr");
            Guid id = storage.User.GetId("ulysse");
            Console.WriteLine("Ulysse OK");
            IUserInfo info = storage.User.GetInfo(id);
            Console.WriteLine(info.Login);
            Console.WriteLine(info.Email);
            
            storage.User.Create("bob", "bob@ens.fr");
            

            Console.WriteLine("bob OK");
            
            Console.ReadLine();
        }
    }
}
