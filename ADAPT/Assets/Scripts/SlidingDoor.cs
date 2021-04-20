using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{

    float startPos;
    public float maxYPos;
    public float speed;
    bool moveUp;
    bool moveDown;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.y;
        moveUp = false;
        moveDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position.y);

        if (transform.position.y <= startPos)
        {
            moveDown = false;
        }

        if (moveUp && startPos <= maxYPos)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
            //transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else if(transform.position.y >= maxYPos)
        {
            moveUp = false;
        }

        if (moveDown && startPos >= maxYPos)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
        }
       
    }

    IEnumerator MoveUp()
    {

        for (float f = 0.05f; f <= maxYPos; f += 0.05f)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            //transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
        }

        yield return new WaitForSeconds(0.05f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);

        if (collision.tag == "Player")
        {
            moveUp = true;
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
