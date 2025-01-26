using UnityEngine;

public class sal : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("CLICKED " + hit.collider.name);

                // Draw the ray to the hit point
                Debug.DrawLine(ray.origin, hit.point, Color.green, 2f);
            }
            else
            {
                // Draw the ray forward even if it doesn't hit anything
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red, 2f);
            }
        }
    }
}
