using UnityEngine;

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

    [Header("Visuals")]
    [SerializeField]
    private SpriteRenderer sprite;

    private AnimationController anim;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

    public void ApplyDamage(float amount)
    {
    }
}
