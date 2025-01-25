using UnityEngine;

public class WindZone : MonoBehaviour
{
    public float windForce = 10f; // Strength of the wind
    public Vector3 windDirection = Vector3.forward; // Default wind direction

    private void OnTriggerStay(Collider other)
    {
        // Check if the object has a Rigidbody
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply force in the wind's direction
            rb.AddForce(windDirection.normalized * windForce);
        }
    }

    private void OnDrawGizmos()
    {
        // Optional: Draw wind direction in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, windDirection.normalized * 2f);
    }
}
