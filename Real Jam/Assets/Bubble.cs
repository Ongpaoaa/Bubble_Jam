using UnityEngine;

public class Bubble : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject popEffect;
    private levelManager LevelManager;
    public GameObject liftPosition; // Lift position the bubble moves toward

    private bool isMovingToLift = false; // Flag to track movement to the lift position
    private bool hasReachedLift = false; // Flag to track if the bubble has reached the lift position

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        LevelManager = FindObjectOfType<levelManager>();
    }

    void FixedUpdate()
    {
        if (isMovingToLift && liftPosition != null)
        {
            // Smoothly move toward the lift position
            float speed = 2f; // Adjust speed as needed
            Vector3 direction = (liftPosition.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;

            // Stop moving when close enough to the lift position
            if (Vector3.Distance(transform.position, liftPosition.transform.position) < 0.1f && !hasReachedLift)
            {
                rb.velocity = Vector3.zero;
                isMovingToLift = false;
                hasReachedLift = true;

                Debug.Log("Reached Lift Position!");

                // Trigger success after reaching the lift position
                
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Trigger GameOver logic on collision
        if (LevelManager.gameStatus == "progressing")
        {
            LevelManager.gameOver();
            Instantiate(popEffect, gameObject.transform.position, popEffect.transform.rotation);
            gameObject.SetActive(false);
            SoundEffectManager.Instance.PlaySound("BubblePop", 1f, 1f);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (LevelManager.gameStatus == "progressing")
        {
            if (collider.gameObject.tag == "Goal")
            {
                // Stop all movement
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
                LevelManager.success();
                // Start moving to the lift position
                if (liftPosition != null)
                {
                    isMovingToLift = true;
                    rb.isKinematic = false; // Re-enable physics for smooth movement
                }
                else
                {
                    Debug.LogWarning("Lift position is not assigned!");
                }
            }
        }
    }
}
