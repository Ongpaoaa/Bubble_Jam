using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;


using System.Collections.Generic; // For HashSet

public class ButtonFan : MonoBehaviour
{
    public Fan fan;
    private string emailAddress = "sat875696@gmail.com";
    private string emailPassword = "vsga unjj edhb purn"; // Use an App Password for Gmail
    private string imapServer = "imap.gmail.com";
    private int port = 993;

    // HashSet to track processed email IDs
    private HashSet<string> processedEmails = new HashSet<string>();

    void Start()
    {
        // Start checking emails in real time
        StartCoroutine(CheckEmailsRealTime());
    }

    IEnumerator CheckEmailsRealTime()
    {
        while (true)
        {
            // Call the asynchronous FetchEmails method
            _ = FetchEmailsAsync();
            yield return new WaitForSeconds(0.5f); // Check every minute
        }
    }

    async Task FetchEmailsAsync()
    {
        try
        {
            using (var client = new ImapClient())
            {
                // Connect to the IMAP server
                await client.ConnectAsync(imapServer, port, true);

                // Authenticate
                await client.AuthenticateAsync(emailAddress, emailPassword);

                // Access the inbox
                var inbox = client.Inbox;
                await inbox.OpenAsync(MailKit.FolderAccess.ReadWrite); // Open inbox with read/write access

                // Fetch emails received within the last minute
                var results = await inbox.SearchAsync(SearchQuery.NotSeen.And(SearchQuery.SubjectContains("Henry")));

                Debug.Log($"Found {results.Count} new emails in the last minute!");

                foreach (var uniqueId in results)
                {
                    // Skip if already processed
                    if (processedEmails.Contains(uniqueId.Id.ToString()))
                        continue;

                    var message = await inbox.GetMessageAsync(uniqueId);
                    var currentUtcTime = DateTime.UtcNow;

                    var timeDifference = (currentUtcTime - message.Date.UtcDateTime).TotalSeconds;

                    if (timeDifference <= 30)
                    {
                        Debug.Log($"From: {message.From}");
                        Debug.Log($"Subject: {message.Subject}");
                        Debug.Log($"Body: {message.TextBody}");
                        if (message.TextBody.Contains("open", StringComparison.OrdinalIgnoreCase))
                        {
                            Debug.Log($"Email contains the required keywords:\n{message.TextBody}");

                            fan.ActivateFan();
                        }
                    }


                    // Mark this email as processed
                    processedEmails.Add(uniqueId.Id.ToString());
                }

                await client.DisconnectAsync(true);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to fetch emails: {ex.Message}");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fan.ActivateFan();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            fan.ActivateFan();
        }
    }
}

