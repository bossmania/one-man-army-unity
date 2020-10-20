using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health_bar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image bar;
    

    public void set_max_hp(float max_hp) {
        slider.maxValue = max_hp;
        slider.value = max_hp;
        bar.color = gradient.Evaluate(1f);
    }

    public void set_hp(float hp) {
        slider.value = hp;
        bar.color = gradient.Evaluate(slider.normalizedValue);
    }
}
