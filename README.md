# HaulTech CSP - Sample Integration Project
A C# console application provided "as is" as an example of how to integrate with HaulTech's Customer Service Portal (CSP) solution.

# Pre-Requisites
- [Visual Studio Code](https://code.visualstudio.com/Download)
- [.NET 5.x SDK](https://dotnet.microsoft.com/en-us/download/dotnet/5.0) - whatever version suits your needs

# Reference Material
- [CSP OAuth Tokens](https://adminhttms.blob.core.windows.net/public/documents/CustomerServicePortal/HTC-TRD-CSP%20OAuth%20Tokens.pdf)
- [CSP API Onboarding](https://adminhttms.blob.core.windows.net/public/documents/CustomerServicePortal/HTC-TRD-CSP%20API%20Onboarding.pdf)
- NuGet Package [HaulTech.Public.CSP](https://www.nuget.org/packages/HaulTech.Public.CSP)

# How To Use This Sample
- Clone the repo locally and open with Visual Studio Code
- Open a new Terminal (Crtl+Shift+') and run `dotnet restore`
- Edit `Program.cs` and update the following variables according to the target CSP instance
  - `_apiUrl`
  - `_authorisationUrl`
  - `_username`
  - `_password`
- Note: CSP URLs and credentials are unique to each CSP instance - speak to your haulier/customer to obtain this information
- Save `Program.cs` with "File > Save" or "Ctrl+S"

# Run and Debug
- To run the sample, execute `dotnet run` from the Terminal
- To start debugging, press F5

# Developing and Contributing
- Development contributions are currently not being accepted
- We welcome reports of any technical issue with the project
  - First, check if the issue already exists in our [GitHub issues](../../issues)
  - If you can't find your issue, [open a new issue](../../issues/new/choose) ensuring the directions are followed as best you can
  - Please file documentation suggestions as "Something Else"