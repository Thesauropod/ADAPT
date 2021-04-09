using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public BellumAI enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            enemy.HitTarget(2f, Direction(enemy.gameObject.transform.position), 10f);
        }
    }

    private Vector2 Direction(Vector3 tempTarget)
    {

        if (tempTarget.x - this.gameObject.transform.position.x < 0)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.right;
        }

    }
}
