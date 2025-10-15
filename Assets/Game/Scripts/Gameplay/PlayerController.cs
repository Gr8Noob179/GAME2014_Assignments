using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float fireFrequency = 1f;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Projectile bulletPrefab;

    [SerializeField]
    private Rect movementBounds = new Rect(-5f, -3f, 10f, 6f);

    private Rigidbody2D rb;
    private Vector2 desiredPos;
    private float elapsedTime;
    bool bHasTarget = false;
    bool bHasShot = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void PerformShoot()
    {

    }

    public void SetDesiredPosition(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());

        worldPos.x = Mathf.Clamp(worldPos.x, movementBounds.xMin, movementBounds.xMax);
        worldPos.y = Mathf.Clamp(worldPos.y, movementBounds.yMin, movementBounds.yMax);

        desiredPos = worldPos;
        bHasTarget = true;
    }

    private void Update()
    {
        if (bHasShot && elapsedTime < fireFrequency)
        {
            elapsedTime += Time.deltaTime;
            return;
        }

        if (Physics2D.Raycast(transform.position, transform.up))
        {
            bHasShot = true;
            elapsedTime = 0;

            Projectile bullet = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);
            bullet.directionSign = Mathf.Sign(gameObject.transform.up.y);
            bullet.exceptions.Add(gameObject);
            Destroy(bullet, 5f);
        }
    }

    private void FixedUpdate()
    {
        if (!bHasTarget)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (desiredPos - (Vector2)transform.position);
        float distance = direction.magnitude;

        if (distance < 0.05f)
        {
            rb.linearVelocity = Vector2.zero;
            bHasTarget = false;
        }
        else
        {
            Vector2 velocity = direction.normalized * speed;
            rb.linearVelocity = velocity;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(movementBounds.center, movementBounds.size);
    }
#endif
}