using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Rigidbody2D monsterRigidBody;
    private SpriteRenderer monsterSpriteRenderer;
    [HideInInspector]
    public float Monsterspeed ;
    // Start is called before the first frame update
    void Awake()
    {
        monsterRigidBody = gameObject.GetComponent<Rigidbody2D>();
        monsterSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Monsterspeed < 0)
            monsterSpriteRenderer.flipX = true;
        else
            monsterSpriteRenderer.flipX = false;

        monsterRigidBody.velocity = new Vector2(Monsterspeed, monsterRigidBody.velocity.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if(collision.collider.gameObject.CompareTag("Collector"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Collector"))
        {
            Destroy(gameObject);
        }
    }
}
