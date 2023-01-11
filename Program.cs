using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Drawing;
using System.Text;

namespace Screenshotter
{
    class Sreenshotter
    {
        static void Main(string[] args)
        {
            String webhookURL = ConfigurationManager.AppSettings.Get("webhookurl");
            String frequency = ConfigurationManager.AppSettings.Get("frequencyminutes");
            String windowName = ConfigurationManager.AppSettings.Get("windowname");

            Console.Title = "Discord Screenshotter";

            //////////////////////
            // validate the config file
            //////////////////////
            if (webhookURL is null || frequency is null || windowName is null)
            {
                Console.WriteLine("Error. Config file invalid, one or more required values are not present");
                return;
            }

            Double parsedFrequency = 10; // default 10 minutes
            try {
                parsedFrequency = Double.Parse(frequency);
            } catch (Exception) {
                Console.WriteLine("Failed to parse {0} as a number, defaulting to 10 minutes", frequency);
            }
            
            Console.WriteLine("Starting screenshotter...\n    Webhook: {0}\n    Frequency: {1} minutes\n    Target Window: {2}", webhookURL, frequency, windowName);
            //////////////////////

            //////////////////////
            // BEGIN SCREENSHOTTER
            //////////////////////
            RunInBackground(parsedFrequency, () => 
                {
                    Console.WriteLine("------------------");

                    List<byte[]> screenshots = TakeScreenshots(windowName);
                    foreach (byte[] image in screenshots) {
                        SendToWebhook(image, webhookURL);
                    }

                    Console.WriteLine("Sent {0} images to the webhook", screenshots.Count);
                    Console.WriteLine("Press any key to stop the monitor");
                    Console.WriteLine("------------------");
                }
            );

            Console.ReadKey(); 
        }

        private static async void RunInBackground(Double frequency, Action action) {

            PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMinutes(frequency));
            action(); // execute once at the start

            while (await timer.WaitForNextTickAsync())
            {
                action();
            }
        }

        private static List<byte[]> TakeScreenshots(String windowName)
        {
            // Create the list to return
            List<byte[]> screenshots = new List<byte[]>();

            // Get the required windows
            Dictionary<IntPtr, string> windows = OpenWindowGetter.GetOpenWindows()
                .Where(kvp => kvp.Value == windowName)
                .ToDictionary(item => item.Key, item => item.Value);
            Console.WriteLine("Got {0} open windows to capture", windows.Count);

            foreach (KeyValuePair<IntPtr, string> window in windows)
            {
                IntPtr handle = window.Key;

                // Maximise the window
                User32.ShowWindow(handle, User32.SW_MAXIMIZE);
                User32.SetForegroundWindow(handle);

                // Briefly sleep to enable java to render
                Thread.Sleep(500);

                // Capture the screenshot
                Screenshot ss = new Screenshot();
                Image screenshot = ss.CaptureWindow(handle);
                Console.WriteLine("Captured screenshot of window {0}: {1}", handle, window.Value);

                // Convert to a byte array
                using (MemoryStream ms = new MemoryStream())
                {
                    screenshot.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    screenshots.Add(ms.ToArray());
                    ms.DisposeAsync();
                }              
            }

            return screenshots;
        }
    
        private static void SendToWebhook(byte[] image, String webhookURL)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new ByteArrayContent(image, 0, image.Length), "Image", "image.png");
                form.Add(new StringContent("Screenshot taken", encoding: Encoding.UTF8, "application/x-www-form-urlencoded"), "content");
                httpClient.PostAsync(webhookURL, form).Wait();
                httpClient.Dispose();
            }
        }
    }
}