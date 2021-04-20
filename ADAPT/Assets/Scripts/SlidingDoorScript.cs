using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorScript : MonoBehaviour
{
    Vector2 startPos;
    public Transform targetPos;
    public float speed;
    bool moveUp;
    bool moveDown;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        moveUp = false;
        moveDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(startPos);
        //Debug.Log(transform.position.y);

        //Moves object to position of target object
        if (moveUp)
        {
            //transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
        }

        //Moves object to original position when game started
        if (moveDown)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);

        if (collision.tag == "Player")
        {
            moveUp = true;
            moveDown = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            moveUp = false;
            moveDown = true;
        }
    }
}
