using UnityEngine;
using UnityEngine.EventSystems;
public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Refs")]
    [SerializeField]
    private RectTransform background;

    [SerializeField]
    private RectTransform handle;

    [Header("Tuning")]
    [SerializeField]
    private float radius = 100f;

    [SerializeField]
    private float deadZone = 0.1f;

    [SerializeField]
    private float moveThreshold = 20f;

    public Vector2 Direction { get; private set; }
    public float Magnitude { get; private set; }

    void Awake()
    {
        handle.anchoredPosition = Vector2.zero;
        Direction = Vector2.zero;
        Magnitude = 0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, null, out var localPoint))
        {
            localPoint -= background.rect.center;
            Vector2 clampedHandlePosition = Vector2.ClampMagnitude(localPoint, radius);
            handle.anchoredPosition = Vector2.ClampMagnitude(localPoint, radius);

            Vector2 normalizedInput = clampedHandlePosition / radius;
            float inputMagnitude = normalizedInput.magnitude;
            if (inputMagnitude < deadZone)
            {
                Direction = Vector2.zero;
                Magnitude = 0f;
            }
            else
            {
                Direction = normalizedInput;
                Magnitude = Mathf.InverseLerp(deadZone, 1f, inputMagnitude);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Direction = Vector2.zero;
        Magnitude = 0f;
        handle.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData) { }
}