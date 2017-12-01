using System;
using DisposableEmailDomainsCheck;
using System.Threading;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            String [] toCheck = { "asdf@mydomain.com", "mydomain.com", "sd9f8sd@mailinator.com" };

            do
            {
                Console.WriteLine(DisposableEmailDomains.State.ToString() + " | Waiting...");
                Thread.Sleep(2000);

            } while (!DisposableEmailDomains.IsReady);

            Console.WriteLine("Index state: " + DisposableEmailDomains.State.ToString());

            foreach (var hostOrEmail in toCheck)
            {
                bool isListed = DisposableEmailDomains.Contains(hostOrEmail);

                Console.WriteLine(hostOrEmail + " is listed: " + isListed);
            }

            Console.ReadKey();

        }
    }
}
