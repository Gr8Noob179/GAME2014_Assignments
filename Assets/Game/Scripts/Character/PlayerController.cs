using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Input")]
    [SerializeField]
    private Joystick joystick;

    [Header("Movement")]
    [SerializeField]
    private float maxSpeed = 6f;

    [SerializeField]
    private float acceleration = 30f;

    [SerializeField]
    private float deceleration = 40f;

    [Header("Attack")]
    [SerializeField]
    private float attackDamage = 10f;

    [SerializeField]
    private float attackCooldown = 0.5f;

    [SerializeField]
    private float attackRange = 1.5f;

    [Header("Visuals")]
    [SerializeField]
    private SpriteRenderer sprite;

    private AnimationController anim;
    private Health health;
    private Rigidbody2D rb;

    private float lastAttackTime;
    private bool canAttack = true;
    private bool hitAnim = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        anim = AnimationController.Get(gameObject);
    }

    private void FixedUpdate()
    {
        Vector2 desiredVel = joystick.Direction * maxSpeed;

        Vector2 actualVelocity = rb.linearVelocity;
        Vector2 delta = desiredVel - actualVelocity;

        float acceleration = (desiredVel.sqrMagnitude > 0.0001f) ? this.acceleration : deceleration;
        Vector2 change = Vector2.ClampMagnitude(delta, acceleration * Time.fixedDeltaTime);

        rb.linearVelocity = actualVelocity + change;

        if (anim)
        {
            float speed = Mathf.InverseLerp(0f, maxSpeed, rb.linearVelocity.magnitude);
            anim.SetValue(EAnimationParameter.SpeedFactor, speed);
        }

        if (sprite && Mathf.Abs(joystick.Direction.x) > 0.01f)
        {
            sprite.flipX = joystick.Direction.x < 0f;
        }
    }

    public void TryAttack()
    {
        if (!canAttack || Time.time - lastAttackTime < attackCooldown)
        {
            return;
        }

        lastAttackTime = Time.time;
        canAttack = false;

        anim.SetValue(EAnimationParameter.Attack);
        PerformAttack();
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void PerformAttack()
    {
        Vector2 attackOrigin = transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackOrigin, attackRange);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.CompareTag("Player"))
            {
                continue;
            }

            if (hit.TryGetComponent(out IDamageable target))
            {
                target.ApplyDamage(attackDamage);
            }
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    public void ApplyDamage(float amount)
    {
        if (health)
        {
            health.ApplyDamage(amount);
            ToggleHitAnim();
        }
    }

    public void ToggleHitAnim()
    {
        anim.SetValue(EAnimationParameter.Hit, !hitAnim);
        hitAnim = !hitAnim;
    }
}
