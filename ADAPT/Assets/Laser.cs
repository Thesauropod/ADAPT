using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public GameObject Lasert; 
    private float nextActionTime = 0.0f;
    public float period = 0.1f;

    public bool IsOn;

    // Update is called once per frame
    void Update()
    {    
        if (Time.time > nextActionTime ) {
            nextActionTime += period;
            if(Lasert.active == false){
                Lasert.active = true;
                IsOn = true;
            }else{
                Lasert.active = false;
                IsOn = false;
            }
        
        }
        
    }
}
