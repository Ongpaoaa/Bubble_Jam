using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MailManager : MonoBehaviour
{
    public GameObject mailUI;
    public GameObject composeUI;
    public TMP_InputField workerName;
    public TMP_InputField message;

    void Update()
    {
        if(mailUI.active == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                sentMessage();
                mailUI.SetActive(false);
                composeUI.SetActive(true);
            }
        }
    }

    public void openMailUI()
    {
        mailUI.SetActive(true);
    }
    public void sentMessage()
    {
        Worker[] workers = FindObjectsOfType<Worker>();

        foreach (Worker worker in workers)
        {
            if (worker.name.ToLower() == workerName.text.ToLower())
            {
                worker.decideAndAct(message.text);
            }
        }
        clearText();
    }

    public void clearText()
    {
        workerName.text = "";
        message.text = "";
    }
}
