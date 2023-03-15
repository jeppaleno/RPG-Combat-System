using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusPointBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxFocusPoints(int maxFocusPoints)
    {
        slider.maxValue = maxFocusPoints;
        slider.value = maxFocusPoints;
    }
    public void SetCurrentFocusPoints(int currentFocusPoints)
    {
        slider.value = currentFocusPoints;
    }
}
