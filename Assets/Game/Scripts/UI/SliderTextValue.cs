using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SliderTextValue : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        if (slider)
        {
            slider.onValueChanged.AddListener(UpdateText);
        }
    }

    private void UpdateText(float value)
    {
        if (text)
        {
            text.text = $"{value:0}/{slider.maxValue:0}";
        }
    }
}