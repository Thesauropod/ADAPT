using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    //Get components
    Collider2D myCollider;

    SpriteRenderer mySprite;

    public Sprite brokenImage;

    public GameObject brokenPiece;

    //Set Variables
    public bool isDestroyed;
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        mySprite = GetComponent<SpriteRenderer>();

        isDestroyed = false;
    }

    private void Update()
    {
        if (isDestroyed)
        {
            Broken();
        }
    }


    void Broken()
    {
        myCollider.enabled = false;
        mySprite.sprite = brokenImage;
        brokenPiece.GetComponent<SpriteRenderer>().enabled = true;
    }
}
