using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        test[] workers = FindObjectsOfType<test>();
        foreach (var item in workers)
        {
            Debug.Log(item); // This will log each element in the array
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
