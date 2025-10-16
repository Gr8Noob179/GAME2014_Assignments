using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float damage;

    [SerializeField]
    private float speed;

    [HideInInspector]
    public List<GameObject> exceptions= new List<GameObject>();

    [HideInInspector]
    public float directionSign;

    private void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocityY = speed * directionSign;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!exceptions.Contains(collision.gameObject))
        {
            gameObject.SetActive(false);

            if (collision.TryGetComponent(out Health target))
            {
                target.ApplyDamage(damage);
            }

            AudioManager.Instance.PlaySFX("Hit");
        }
    }
}
