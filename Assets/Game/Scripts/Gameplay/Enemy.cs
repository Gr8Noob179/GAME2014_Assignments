using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private float collisionDamage;

    [SerializeField]
    private float fireFrequency = 1f;

    [SerializeField]
    private bool canShoot = true;

    [SerializeField]
    private Projectile bullet;

    [SerializeField]
    private SpriteRenderer explosionRenderer;

    [SerializeField]
    private List<Sprite> explosionFrames = new List<Sprite>();

    [SerializeField]
    private float animationSpeed = 0.1f;

    [HideInInspector]
    public float destination = 0;
    [HideInInspector]
    public int spotIndex;
    [HideInInspector]
    public Spawner spawner;

    private Health health;
    private ScoreText score;
    private float elapsedTime;
    private int animationFrame = 0;
    private bool bHasShot = false;

    private void Awake()
    {
        score = FindFirstObjectByType<ScoreText>();

        health = GetComponent<Health>();
        health.OnDeath.AddListener(() =>
        {
            StartCoroutine(StartExplosionAnimation());
            Destroy(gameObject, animationSpeed * explosionFrames.Count);
            score.UpdateScore();
            AudioManager.Instance.PlaySFX("Death");
        });

    }

    private void Update()
    {
        if (canShoot)
        {
            if (bHasShot && elapsedTime < fireFrequency)
            {
                elapsedTime += Time.deltaTime;
                return;
            }

            bHasShot = true;
            elapsedTime = 0;

            Projectile bullet = Instantiate(this.bullet, gameObject.transform.position, Quaternion.identity);
            bullet.directionSign = -1;
            bullet.exceptions.Add(gameObject);
            bullet.gameObject.SetActive(true);

            AudioManager.Instance.PlaySFX("Shoot");
            Destroy(bullet, 5f);
        }
    }

    private void FixedUpdate()
    {
        if (destination != 0 && transform.position.y > destination)
        {
            transform.Translate(0, -speed / 100, 0);
        }
    }

    private IEnumerator StartExplosionAnimation()
    {
        yield return new WaitForSeconds(animationSpeed);

        explosionRenderer.sprite = explosionFrames[animationFrame];

        if ((explosionFrames.Count - 1) >= animationFrame)
        {
            animationFrame++;
            StartCoroutine(StartExplosionAnimation());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out Health target))
        {
            target.ApplyDamage(collisionDamage);
        }

        health.ApplyDamage(1000);
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.FreeSpot(spotIndex);
        }
    }
}