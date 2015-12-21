# Securing and consuming a Web API with OAuth

## Prerequisites

To build and run this project, you'll need to install the following bits on your dev machine:
* Azure SDK 2.8.1 (http://go.microsoft.com/fwlink/?linkid=518003&clcid=0x409)
* Windows 10 SDK 10.0.10586.15 (https://go.microsoft.com/fwlink/p/?LinkID=698771)
* IIS with ASP.NET (Start, "feature", Turn Windows features on and off)

Optionally:
* Windows 10 Mobile Emulator - 10.0.10586.0 (https://go.microsoft.com/fwlink/?LinkId=698769) to test the UWP app
* Deploy the STS to Azure to prevent SSL certificate issues on a mobile device (or the emulator) because you need to access the STS using HTTPS.

## Getting started

* To open the solution, you'll need to launch Visual Studio as an Administrator
* Before running the solution, don't forget to start the Azure Storage Emulator. You can find it in the Start menu as Microsoft Azure Storage Emulator - v4.2.
* To create test data, comment out the line `config.Filters.Add(new AuthorizeAttribute());` in the API's Startup class. Then you can POST to the various resource URI's using Fiddler or another API tool.
