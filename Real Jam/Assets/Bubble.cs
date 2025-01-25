using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float floatForce = 5f; // Upward force to make the bubble float
    public float maxVerticalSpeed = 2f; // Limit for the upward speed
    private Rigidbody rb;
    private string gameStatus = "progressing";
    public Animator UI;
    public GameObject popEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Apply upward force to simulate floating
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug GameOver message on collision
        if(gameStatus == "progressing")
        {
            Debug.Log("Game Over: The bubble touched something!");
            gameStatus = "lose";
            UI.SetTrigger("GameOver");
            Instantiate(popEffect, gameObject.transform.position, popEffect.transform.rotation);
            popEffect.SetActive(true);
            gameObject.SetActive(false);
        }
        // You can trigger GameOver logic here, like stopping the game or reloading the scene
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(gameStatus == "progressing")
        {
            if(collider.gameObject.tag == "Goal")
            {
                Debug.Log("Win");
                gameStatus = "win";
                UI.SetTrigger("Win");
            }
        }

    }
}
