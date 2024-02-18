using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    public static Action destroyBullet;
    private SpriteRenderer sp;

    public List<Sprite> sprites;

    private void Awake()
    {
        transform.Rotate(Vector3.zero);
    }

    private void OnEnable()
    {
        destroyBullet += BulletPool;
        Invoke("BulletPool", 4.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BulletPool();
        }
    }

    public void BulletPool()
    {
        Destroy(gameObject);
    }
}
