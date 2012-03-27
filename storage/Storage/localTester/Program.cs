using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageLibrary;

namespace localTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Local tester");
            IStorage storage = new Storage("", ""); // a completer avec votre clée
            storage.User.SetInfo(10, "ulysse", "ulysse.beaugnon@ens.fr");
            IUserInfo info = storage.User.GetInfo(11);
            Console.WriteLine("login : " + info.Login);
            Console.WriteLine("email : " + info.Email);
            Console.ReadLine();
        }
    }
}
