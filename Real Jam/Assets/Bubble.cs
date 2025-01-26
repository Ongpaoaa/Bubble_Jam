using UnityEngine;

public class Bubble : MonoBehaviour
{

    private Rigidbody rb;
    public GameObject popEffect;
    private levelManager LevelManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        LevelManager = FindObjectOfType<levelManager>();
    }

    void FixedUpdate()
    {
        // Apply upward force to simulate floating
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug GameOver message on collision
        if(LevelManager.gameStatus == "progressing")
        {
            LevelManager.gameOver();
            Instantiate(popEffect, gameObject.transform.position, popEffect.transform.rotation);
            gameObject.SetActive(false);
        }
        // You can trigger GameOver logic here, like stopping the game or reloading the scene
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(LevelManager.gameStatus == "progressing")
        {
            if(collider.gameObject.tag == "Goal")
            {
                LevelManager.success();
            }
        }
    }
}
