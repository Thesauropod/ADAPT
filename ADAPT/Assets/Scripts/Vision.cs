using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{

    public float timer;
    public bool IsOn=false;

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            timer = 0;
            SendMessageUpwards("RecieveVisual", true);
            IsOn = true;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(LoseTarget());
            IsOn = false;
        }
    }

    IEnumerator LoseTarget() {
        yield return new WaitForSeconds(2f);
        SendMessageUpwards("RecieveVisual", false);
    }

    
}
