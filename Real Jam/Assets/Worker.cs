using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;

using System.Collections.Generic;
public class Worker : MonoBehaviour
{
    public Animator m_Animator;
    private string emailAddress = "sat875696@gmail.com";
    private string emailPassword = "vsga unjj edhb purn";
    private string imapServer = "imap.gmail.com";
    private int port = 993;
    private HashSet<string> processedEmails = new HashSet<string>();
    public GameObject windZone;
    public string mechaType;
    public string workerName;
    public bool openOnStart = false;
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        Debug.Log(m_Animator);
        StartCoroutine(CheckEmailsRealTime());
        if(openOnStart)
        {
            ActivateMecha();
        }
    }

    IEnumerator CheckEmailsRealTime()
    {
        while (true)
        {   
            _ = FetchEmailsAsync();
            yield return new WaitForSeconds(0.5f);
        }
    }

    async Task FetchEmailsAsync()
    {
        try
        {
            using (var client = new ImapClient())
            {
                await client.ConnectAsync(imapServer, port, true);

                await client.AuthenticateAsync(emailAddress, emailPassword);

                var inbox = client.Inbox;
                await inbox.OpenAsync(MailKit.FolderAccess.ReadWrite); 

                var results = await inbox.SearchAsync(SearchQuery.NotSeen.And(SearchQuery.SubjectContains(workerName)));

                Debug.Log($"Found {results.Count} new emails in the last minute!");

                foreach (var uniqueId in results)
                {
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

                            ActivateMecha();
                        }

                        if (message.TextBody.Contains("close", StringComparison.OrdinalIgnoreCase))
                        {
                            Debug.Log($"Email contains the required keywords:\n{message.TextBody}");

                            DeactivateMecha();
                        }
                    }

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

    public void ActivateMecha()
    {
        if(mechaType == "fan")
        {
            windZone.SetActive(true); 
            m_Animator.SetTrigger("OpenFan");
        }
    }

    public void DeactivateMecha()
    {
        if(mechaType == "fan")
        {
            windZone.SetActive(false);
            m_Animator.SetTrigger("CloseFan");
        }
    }
}
