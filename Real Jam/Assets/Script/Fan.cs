using UnityEngine;
public class Fan : MonoBehaviour
{
    public GameObject windZone; // The WindZone GameObject
    public Animator fanAnimator; // Animator for fan's visual activation

    private bool isFanOn = false;

    public void ActivateFan()
    {
        isFanOn = true;
        windZone.SetActive(true); // Enable the WindZone

        // Optionally play an animation
        if (fanAnimator != null)
        {
            fanAnimator.SetTrigger("TurnOn");
        }
    }

    public void DeactivateFan()
    {
        isFanOn = false;
        windZone.SetActive(false); // Disable the WindZone
    }
}
