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
    public GameObject windZone;
    public string mechaType;
    public string workerName;
    public bool openOnStart = false;
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        if(openOnStart)
        {
            ActivateMecha();
        }
    }

    public void decideAndAct(string message)
    {
        message = message.ToLower();
        if(message.Contains("open"))
        {
            ActivateMecha();
        }

        if(message.Contains("close"))
        {
            DeactivateMecha();
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
