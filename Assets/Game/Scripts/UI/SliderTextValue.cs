using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SliderTextValue : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;

    public float Value { get => slider.value; set => slider.value = value; }

    private void Start()
    {
        if (slider)
        {
            slider.onValueChanged.AddListener(UpdateText);
        }
    }

    public void UpdateMaxValue(float value)
    {
        slider.maxValue = value;
    }

    public void UpdateText(float value)
    {
        if (text)
        {
            text.text = $"{value:0}/{slider.maxValue:0}";
        }
    }
}