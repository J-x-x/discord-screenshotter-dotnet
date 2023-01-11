# discord-screenshotter-dotnet
Periodically opens windows with a given name and sends screenshots of them to a discord webhook
# Get Started
To get started, download the latest release and create a config file as seen below.

This also requires a discord webhook to be created. For instructions on creating a webhook visit https://support.discord.com/hc/en-us/articles/228383668-Intro-to-Webhooks

.NET 7.0 or newer must be installed as a pre-requisite

## Config

Requires a config file to be created and saved as `discord-screenshotter-dotnet.dll.config` in the same directory as the .exe
   ```xml
    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
        <appSettings>
            <add key="webhookurl" value="WEBHOOK URL" />
            <add key="frequencyminutes" value="FREQUENCY TO SEND SCREENSHOTS IN MINUTES" />
            <add key="windowname" value="NAME OF THE WINDOW TO SEND SCREENSHOTS OF" />
        </appSettings>
    </configuration>
```
    
## Running on a Remote Desktop Connection (RDP)
When ending a RDP session the GUI session ends and therefore screenshots cannot be taken. This can be worked around using the batch file below which runs the screenshotter then ends the RDP session whilst keeping the GUI session alive. Save the file below as a `.bat` file in the same directory as the .exe.
   ```bat
   for /f "skip=1 tokens=3" %%s in ('query user %USERNAME%') do (tscon.exe %%s /dest:console)
   timeout /t 10
   discord-screenshotter-dotnet.exe
   pause
```
