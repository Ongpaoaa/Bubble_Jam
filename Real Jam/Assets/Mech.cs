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

    [Header("Fan Sound Set")]
    public string fanStartSound = "Fan-TurnOn";  // Sound to play when fan starts
    public string fanWhileSound = "Fan-Steady";  // Looping sound for fan
    public string fanEndSound = "Fan-TurnOff";   // Sound to play when fan stops

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
                Debug.Log("Fan Activated");
                etc.SetActive(true);

                // Play the sound set for the fan
                SoundEffectManager.Instance.PlaySoundSet(fanStartSound, fanWhileSound, fanEndSound);
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
                Debug.Log("Fan Deactivated");
                etc.SetActive(false);

                // Stop the sound set for the fan
                SoundEffectManager.Instance.StopSoundSet(fanEndSound);
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
        if (openTime != 0)
        {
            yield return new WaitForSeconds(openTime);
            closeMech();
        }
    }
}
