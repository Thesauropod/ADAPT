using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{

    public float timer;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            timer = 0;
            SendMessageUpwards("RecieveVisual", true);

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(LoseTarget());
        }
    }

    IEnumerator LoseTarget() {
        yield return new WaitForSeconds(2f);
        SendMessageUpwards("RecieveVisual", false);
    }

    
}
