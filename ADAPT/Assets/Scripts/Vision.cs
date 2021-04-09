using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{

    float timer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("hello?");
            timer = 0;
            SendMessageUpwards("RecieveVisual", true);

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            timer += Time.deltaTime;
            if (timer >= .1f)
            {
                timer = 0;
                SendMessageUpwards("RecieveVisual", false);
            }
        }
    }
}
