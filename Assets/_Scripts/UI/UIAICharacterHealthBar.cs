using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAICharacterHealthBar : MonoBehaviour
{
    public Slider slider;
    private float timeUntilBarIsHidden = 0;
    [SerializeField] UIYellowBar yellowBar;
    [SerializeField] float yellowBarTimer = 3;
    [SerializeField] TMP_Text damageText;
    [SerializeField] int currentDamageTaken;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    private void OnDisable()
    {
        currentDamageTaken = 0; // RESETS DAMAGE TEXT POP UP ON DISABLE
    }

    public void SetHealth(int health)
    {
        if (yellowBar != null)
        {
            yellowBar.gameObject.SetActive(true); // TRIGGERS THE ONENABLE FUNCTION

            yellowBar.timer = yellowBarTimer; // EVERY TIME WE GET HIT WE RENEW THE TIMER

            if (health > slider.value)
            {
                yellowBar.slider.value = health;
            }
        }

        currentDamageTaken = currentDamageTaken + Mathf.RoundToInt(slider.value - health);
        damageText.text = currentDamageTaken.ToString();

        slider.value = health;
        timeUntilBarIsHidden = 10; 
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;

        if (yellowBar != null)
        {
            yellowBar.SetMaxStat(maxHealth);
        }
    }

    private void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);

        timeUntilBarIsHidden = timeUntilBarIsHidden - Time.deltaTime;

        if (slider != null)
        {
            if (timeUntilBarIsHidden <= 0)
            {
                timeUntilBarIsHidden = 0;
                slider.gameObject.SetActive(false);
            }
            else
            {
                if (!slider.gameObject.activeInHierarchy)
                {
                    slider.gameObject.SetActive(true);
                }
            }

            if (slider.value <= 0)
            {
                Destroy(slider.gameObject);
            }
        }
    }
}
