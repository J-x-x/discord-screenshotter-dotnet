# discord-screenshotter-dotnet
Periodically opens windows with a given name and sends screenshots of them to a discord webhook

Requires a config file to be created in the format:
    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
        <appSettings>
            <add key="webhookurl" value="WEBHOOK URL" />
            <add key="frequencyminutes" value="FREQUENCY TO SEND SCREENSHOTS IN MINUTES" />
            <add key="windowname" value="NAME OF THE WINDOW TO SEND SCREENSHOTS OF" />
        </appSettings>
    </configuration>
