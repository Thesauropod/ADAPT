using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public float damage, knockBackFactor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            collision.GetComponent<SivonController>().HitTarget(damage, gameObject.transform.position, knockBackFactor);
        }
        if (collision.gameObject.tag == "enemy")
        {

        }
    }

  
}
