using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    void Start(){
        //Debug.Log("collected nest");
    }

    void OnCollisionEnter2D(Collision2D col){
        //if(col.gameObject.tag == "player"){
            Debug.Log("collected nest");
            //add score?
            gameObject.SetActive(false);
        //}
    }
}
