using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoisonAmountBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 100;
        slider.value = 100;
        gameObject.SetActive(false);
    }

    public void SetPoisonAmount(int poisonAmount)
    {
        slider.value = poisonAmount;
    }
}
