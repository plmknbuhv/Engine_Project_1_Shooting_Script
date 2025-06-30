using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float lifeTime = 2f;
    private WeaponFire _weaponFire;
    private float _timer;
    public bool isDead;

    private void Awake()
    {
        _weaponFire = GameObject.Find("Weapon").GetComponent<WeaponFire>();
    }

    private void Update()
    {
        transform.position += transform.up * (moveSpeed * Time.deltaTime);

        _timer += Time.deltaTime;
        if (_timer >= lifeTime)
        {
            DestroyBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Boss"))
        {
            Destroy(Instantiate(_weaponFire.effect, transform.position, quaternion.identity)
                , 0.99f);
        }
        
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        _weaponFire.bulletPool.Push(this);
        gameObject.SetActive(false);
        isDead = true;
        _timer = 0;
    }
}