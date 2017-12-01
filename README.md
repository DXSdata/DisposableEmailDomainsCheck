# DisposableEmailDomainsCheck for .NET Core / .NET Standard 2.0
Library for simple checks if an email or host address is listed in https://github.com/ivolo/disposable-email-domains


# nBayes for .NET Core / .NET Standard 2.0


# Sample



```CSharp
	using DisposableEmailDomainsCheck;
	
	//...

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
```

# Links
- Website http://www.dxsdata.com/2017/12/net-core-standard-2-0-library-to-check-if-email-or-host-address-is-on-disposable-email-domains-list-trash-mail