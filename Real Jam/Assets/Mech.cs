using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech : MonoBehaviour
{
    public string mechType; // Type of mechanism (e.g., "fan" or "door")
    public bool status;     // Status of the mechanism
    public GameObject etc;  // GameObject related to the mechanism (e.g., fan or door)
    public float openTime;  // Time in seconds before the mechanism automatically closes
    private Animator anim;  // Animator for doors

    // Start is called before the first frame update
    void Start()
    {
        // Optionally, initialize components here
    }

    public void openMech()
    {
        if (!status)
        {
            if (mechType == "fan")
            {
                Debug.Log("fan");
                etc.SetActive(true);
            }

            if (mechType == "door")
            {
                anim = GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetTrigger("Open");
                }
                else
                {
                    Debug.LogError("Animator not found on the object!");
                }
            }

            status = true;

            // Start coroutine to close after openTime seconds
            StartCoroutine(AutoCloseAfterTime());
        }
    }

    public void closeMech()
    {
        if (status)
        {
            if (mechType == "fan")
            {
                etc.SetActive(false);
            }

            if (mechType == "door")
            {
                anim = GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetTrigger("Close");
                }
                else
                {
                    Debug.LogError("Animator not found on the object!");
                }
            }

            status = false;
        }
    }

    private IEnumerator AutoCloseAfterTime()
    {
        if(openTime != 0)
        {
            yield return new WaitForSeconds(openTime);
            closeMech();
        }
        
    }
}
