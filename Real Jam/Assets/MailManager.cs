using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MailManager : MonoBehaviour
{
    public TMP_InputField workerName;
    public TMP_InputField message;
    void Start()
    {

    }

    public void sentMessage()
    {
        Worker[] workers = FindObjectsOfType<Worker>();
        
        foreach (Worker worker in workers)
        {
            if (worker.name == workerName.text)
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
