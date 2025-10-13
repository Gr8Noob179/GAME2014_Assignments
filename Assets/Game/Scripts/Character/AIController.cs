using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AIController : MonoBehaviour, IDamageable
{
    [Header("Attack")]
    [SerializeField]
    private float damage = 10f;

    [SerializeField]
    private float attackCooldown = 0.8f;

    [Header("Health")]
    [SerializeField]
    private Health health;

    private CircleCollider2D trigger;
    private AnimationController anim;
    private Dictionary<Collider2D, IDamageable> targetsInRange = new Dictionary<Collider2D, IDamageable>();

    private bool isAttacking;
    private float lastAttackTime = -999f;

    private void Awake()
    {
        trigger = GetComponent<CircleCollider2D>();
        anim = AnimationController.Get(gameObject);
    }

    private void Start()
    {
        if (health)
        {
            health.OnDeath.AddListener(OnDeath);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.TryGetComponent(out IDamageable target))
        {
            targetsInRange.Add(other, target);

            TryStartAttack();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (targetsInRange.ContainsKey(other))
        {
            targetsInRange.Remove(other);

            if (targetsInRange.Count == 0)
            {
                anim.SetValue(EAnimationParameter.Attack, false);
                isAttacking = false;
            }
        }
    }

    public void TryStartAttack()
    {
        if (isAttacking || (targetsInRange.Count == 0))
        {
            return;
        }

        isAttacking = true;
        anim.SetValue(EAnimationParameter.Attack);
    }

    public void OnAttackHit()
    {
        Vector2 center = (Vector2)trigger.bounds.center;
        float radius = trigger.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius);
        if (hits.Length == 0)
        {
            return;
        }

        foreach (Collider2D hit in hits)
        {
            if (targetsInRange.ContainsKey(hit))
            {
                Debug.Log(hit.name);
                targetsInRange[hit].ApplyDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected() 
    {
        CircleCollider2D cc = GetComponent<CircleCollider2D>();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(cc.bounds.center, cc.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y));
    }

    public void ApplyDamage(float amount)
    {
        if (health)
        {
            health.ApplyDamage(amount);
            anim.SetValue(EAnimationParameter.Hit);
        }
    }

    private void OnDeath()
    {
        anim.SetValue(EAnimationParameter.Death);
        trigger.enabled = false;
    }
}