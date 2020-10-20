using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class freeze_bar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image bar;

    public void set_max_freeze(float max_freeze) {
        slider.maxValue = max_freeze;
        slider.value = max_freeze;
        bar.color = gradient.Evaluate(1f);
    }

    public void set_freeze(float freeze) {
        slider.value = freeze;
        bar.color = gradient.Evaluate(slider.normalizedValue);
    }
}
