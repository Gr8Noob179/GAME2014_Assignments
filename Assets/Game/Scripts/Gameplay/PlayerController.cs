using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Rect movementBounds = new Rect(-5f, -3f, 10f, 6f);

    private Rigidbody2D rb;
    private Vector2 desiredPos;
    bool bHasTarget = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

    private void FixedUpdate()
    {
        if (!bHasTarget)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (desiredPos - rb.position);
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