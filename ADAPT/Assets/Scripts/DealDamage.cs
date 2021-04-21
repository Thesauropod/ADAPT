using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{

    GameObject Parent;

    public float damage, knockBackFactor;
    // Start is called before the first frame update
    void Start()
    {
        Parent = this.Parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
     /*   if (Input.GetKeyDown(KeyCode.Space)) {
            enemy.HitTarget(2f, Direction(enemy.gameObject.transform.position), 10f);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            //VVVVV Do this VVVVVVVV
          //  collision.GetComponent<WalkController>().hitTarget(damage, Direction(collision.gameObject.transform.position), knockBackFactor);
        }
    }

  
}
