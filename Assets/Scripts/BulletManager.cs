using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public delegate void BulletHitEnemyHandler();
    public static event BulletHitEnemyHandler OnBulletHitEnemy;
    [SerializeField]
    private float bulletSpan = 2f;
    void Start()
    {
        StartCoroutine(DestroyBullet(bulletSpan));
    }

    IEnumerator DestroyBullet(float destroyTime )
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.collider.gameObject);
            OnBulletHitEnemy?.Invoke();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            OnBulletHitEnemy?.Invoke();
            Destroy(gameObject);
        }
    }
}
