using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValue : MonoBehaviour
{
    public Slider timeSlider;
    public TMP_Text timeValueText;

    void Start()
    {
        UpdateTime();
    }

    public void UpdateTime()
    {
        timeValueText.text = timeSlider.value.ToString("0") + " seconds";
    }
}