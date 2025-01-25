using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech : MonoBehaviour
{
    public string mechType;
    public bool status;
    public GameObject etc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openMech()
    {
        if(mechType == "fan")
        {
            etc.SetActive(true);
        }
    }


    public void closeMech()
    {
        if(mechType == "fan")
        {
            etc.SetActive(false);
        }
    }
}
