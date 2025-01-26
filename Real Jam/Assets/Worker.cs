using System.Collections;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections.Generic;
public class Worker : MonoBehaviour
{
    public Animator m_Animator;
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

    public async  void decideAndAct(string message)
    {
        message = message.ToLower();

        Match waitMatch = Regex.Match(message, @"(after|in|wait\s*for)\s+(\d+)");
        if (waitMatch.Success)
        {
            // Extract the number and parse it as seconds
            int waitTime = int.Parse(waitMatch.Groups[2].Value);
            Debug.Log($"Waiting for {waitTime} seconds...");
            await Task.Delay(waitTime * 1000);
        }
        Debug.Log(message);
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
