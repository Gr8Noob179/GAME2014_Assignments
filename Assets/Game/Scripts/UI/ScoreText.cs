using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class ScoreText : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onScoreChanged;

    [SerializeField]
    private UnityEvent onFinish;

    [SerializeField]
    private int maxScore;

    private TMP_Text text;
    private int score = 0;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void UpdateScore()
    {
        score++;
        text.text = score.ToString();

        onScoreChanged?.Invoke();

        if (score == maxScore)
        {
            onFinish?.Invoke();
        }
    }
}
