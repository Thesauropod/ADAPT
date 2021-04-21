using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorControl : MonoBehaviour
{
    public GameObject ExitDoor;
    public float speed;
    int timer = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {


            for(int i=0; i< 10; i++)
            {
                
                ExitDoor.transform.position = new Vector3(ExitDoor.transform.position.x, ExitDoor.transform.position.y + speed, ExitDoor.transform.position.z);
            }
            

        }
    }

}
