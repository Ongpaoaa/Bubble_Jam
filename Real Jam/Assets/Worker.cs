using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using System;

using System.Collections.Generic;
public class Worker : MonoBehaviour
{
    public Animator m_Animator;
    public GameObject windZone;
    public GameObject mech;
    public string workerName;
    public bool openOnStart = false;
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        if(openOnStart)
        {
            mech.GetComponent<Mech>().openMech();
        }
    }

    public void decideAndAct(string message)
    {
        message = message.ToLower();
        if (message.Contains("open"))
        {
            mech.GetComponent<Mech>().openMech();
            return;
        }

        if (message.Contains("close"))
        {
            mech.GetComponent<Mech>().closeMech();
            return;
        }
    }

}
