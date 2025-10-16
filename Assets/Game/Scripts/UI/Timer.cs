using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float timer;

    [SerializeField]
    private TMP_Text text;

    public UnityEvent onTimerFinish;

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            int totalSeconds = Mathf.CeilToInt(timer);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            text.text = $"{minutes:00}:{seconds:00}";

            if (timer <= 0)
            {
                onTimerFinish?.Invoke();
            }
        }
    }
}
