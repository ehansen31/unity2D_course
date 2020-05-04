using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject laserObject;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] GameObject explodeEffect;
    [SerializeField] AudioClip deathSFX;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
        }
    }

    private void Fire()
    {
        var laserPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        GameObject laser = Instantiate(laserObject, laserPosition, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            StartCoroutine(ExplodeEffect());
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(deathSFX, transform.position, .5f);
        }
    }

    private IEnumerator ExplodeEffect()
    {
        var effect = Instantiate<GameObject>(explodeEffect, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        Destroy(effect, 1);
        yield return new WaitForSeconds(1);
    }
}
