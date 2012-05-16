using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tigwi.Storage.Library;

namespace SetUp
{
    class Program
    {
        const string azureAccountName = "__AZURE_STORAGE_ACCOUNT_NAME";
        const string azureAccountKey = "__AZURE_STORAGE_ACCOUNT_KEY";
 
        // Lauch this program to reinit the storage
        static void Main(string[] args)
        {
            Console.WriteLine("Vous êtes sur le point de réinitialiser le compte de storage suivant :");
            Console.WriteLine("nom de compte : " + azureAccountName);
            Console.WriteLine("clé du compte : " + azureAccountKey);
            Console.Write("Etes vous sur de vouloir effacer  le storage ? (o/N)");
            string answer = Console.ReadLine();
            if (answer == "o")
            {
                BlobFactory blobFactory = new BlobFactory(azureAccountName, azureAccountKey);
                blobFactory.InitStorage();
                Console.WriteLine("Le compte storage a été réinitialisé");
            }
            else
            {
                Console.WriteLine("Rien n'a été fait.");
            }
            Console.WriteLine("Appuyer sur une touche pour quitter.");
            Console.ReadKey();
        }
    }
}
