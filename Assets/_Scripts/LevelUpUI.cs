using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LevelUpUI : MonoBehaviour
{
    public PlayerManager playerManager;
    public Button confirmLevelUpButton;

    [Header("Player Level")]
    public int currentPlayerLevel; //THE CURRENT LEVEL WE ARE BEFORE LEVELING UP
    public int projectedPlayerLevel; //THE POSSIBLE LEVEL WE WILL BE IF WE ACCEPT LEVELING UP
    public TextMeshProUGUI currentPlayerLevelText; //THE UI TEXT FOR THE NUMBER OF THE CURRENT PLAYER LEVEL
    public TextMeshProUGUI projectedPlayerLevelText; //THE UI TEXT FOR THE PROJECTED PLAYER LEVEL NUMBER

    [Header("Souls")]
    public TextMeshProUGUI currentSoulsText;
    public TextMeshProUGUI soulsRequiredToLevelUpText;
    private int soulsRequiredToLevelUp;
    public int baseLevelUpCost = 5;

    [Header("Health")]
    public Slider healthSlider;
    public TextMeshProUGUI currentHealthLevelText;
    public TextMeshProUGUI projectedHealthLevelText;

    [Header("Stamina")]
    public Slider staminaSlider;
    public TextMeshProUGUI currentStaminaLevelText;
    public TextMeshProUGUI projectedStaminaLevelText;

    [Header("Focus")]
    public Slider focusSlider;
    public TextMeshProUGUI currentFocusLevelText;
    public TextMeshProUGUI projectedFocusLevelText;

    [Header("Poise")]
    public Slider poiseSlider;
    public TextMeshProUGUI currentPoiseLevelText;
    public TextMeshProUGUI projectedPoiseLevelText;

    [Header("Strength")]
    public Slider strengthSlider;
    public TextMeshProUGUI currentStrengthLevelText;
    public TextMeshProUGUI projectedStrengthLevelText;

    [Header("Dexterity")]
    public Slider dexteritySlider;
    public TextMeshProUGUI currentDexterityLevelText;
    public TextMeshProUGUI projectedDexterityLevelText;

    [Header("Faith")]
    public Slider faithSlider;
    public TextMeshProUGUI currentFaithLevelText;
    public TextMeshProUGUI projectedFaithLevelText;

    [Header("Intelligence")]
    public Slider intelligenceSlider;
    public TextMeshProUGUI currentIntelligenceLevelText;
    public TextMeshProUGUI projectedIntelligenceLevelText;

    //Update all of the stats on the ui to the players current stats
    private void OnEnable()
    {
        currentPlayerLevel = playerManager.playerStatsManager.playerLevel;
        currentPlayerLevelText.text = currentPlayerLevel.ToString();

        projectedPlayerLevel = playerManager.playerStatsManager.playerLevel;
        projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

        healthSlider.value = playerManager.playerStatsManager.healthLevel;
        healthSlider.minValue = playerManager.playerStatsManager.healthLevel;
        healthSlider.maxValue = 99;
        currentHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();
        projectedHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();
        
        staminaSlider.value = playerManager.playerStatsManager.staminaLevel;
        staminaSlider.minValue = playerManager.playerStatsManager.staminaLevel;
        staminaSlider.maxValue = 99;
        currentStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();
        projectedStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();

        focusSlider.value = playerManager.playerStatsManager.focusLevel;
        focusSlider.minValue = playerManager.playerStatsManager.focusLevel;
        focusSlider.maxValue = 99;
        currentFocusLevelText.text = playerManager.playerStatsManager.focusLevel.ToString();
        projectedFocusLevelText.text = playerManager.playerStatsManager.focusLevel.ToString();

        poiseSlider.value = playerManager.playerStatsManager.poiseLevel;
        poiseSlider.minValue = playerManager.playerStatsManager.poiseLevel;
        poiseSlider.maxValue = 99;
        currentPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();
        projectedPoiseLevelText.text = playerManager.playerStatsManager.poiseLevel.ToString();

        strengthSlider.value = playerManager.playerStatsManager.strengthLevel;
        strengthSlider.minValue = playerManager.playerStatsManager.strengthLevel;
        strengthSlider.maxValue = 99;
        currentStrengthLevelText.text = playerManager.playerStatsManager.strengthLevel.ToString();
        projectedStrengthLevelText.text = playerManager.playerStatsManager.strengthLevel.ToString();

        dexteritySlider.value = playerManager.playerStatsManager.dexterityLevel;
        dexteritySlider.minValue = playerManager.playerStatsManager.dexterityLevel;
        dexteritySlider.maxValue = 99;
        currentDexterityLevelText.text = playerManager.playerStatsManager.dexterityLevel.ToString();
        projectedDexterityLevelText.text = playerManager.playerStatsManager.dexterityLevel.ToString();

        intelligenceSlider.value = playerManager.playerStatsManager.intelligenceLevel;
        intelligenceSlider.minValue = playerManager.playerStatsManager.intelligenceLevel;
        intelligenceSlider.maxValue = 99;
        currentIntelligenceLevelText.text = playerManager.playerStatsManager.intelligenceLevel.ToString();
        projectedIntelligenceLevelText.text = playerManager.playerStatsManager.intelligenceLevel.ToString();

        faithSlider.value = playerManager.playerStatsManager.faithLevel;
        faithSlider.minValue = playerManager.playerStatsManager.faithLevel;
        faithSlider.maxValue = 99;
        currentFaithLevelText.text = playerManager.playerStatsManager.faithLevel.ToString();
        projectedFaithLevelText.text = playerManager.playerStatsManager.faithLevel.ToString();

        currentSoulsText.text = playerManager.playerStatsManager.currentSoulCount.ToString();
        
        UpdateProjectedPlayerLevel();
    }

    //Updates the players stats to the projected stats, providing they have enough souls(xp)
    public void ConfirmPlayerLevelUpStats()
    {
        playerManager.playerStatsManager.playerLevel = projectedPlayerLevel;
        playerManager.playerStatsManager.healthLevel = Mathf.RoundToInt(healthSlider.value);
        playerManager.playerStatsManager.staminaLevel = Mathf.RoundToInt(staminaSlider.value);
        playerManager.playerStatsManager.focusLevel = Mathf.RoundToInt(focusSlider.value);
        playerManager.playerStatsManager.poiseLevel = Mathf.RoundToInt(poiseSlider.value);
        playerManager.playerStatsManager.strengthLevel = Mathf.RoundToInt(strengthSlider.value);
        playerManager.playerStatsManager.dexterityLevel = Mathf.RoundToInt(dexteritySlider.value);
        playerManager.playerStatsManager.faithLevel = Mathf.RoundToInt(faithSlider.value);
        playerManager.playerStatsManager.intelligenceLevel = Mathf.RoundToInt(intelligenceSlider.value);

        playerManager.playerStatsManager.maxHealth = playerManager.playerStatsManager.SetMaxHealthFromHealthLevel();
        playerManager.playerStatsManager.maxStamina = playerManager.playerStatsManager.SetMaxStaminaFromStaminaLevel();
        playerManager.playerStatsManager.maxFocusPoints = playerManager.playerStatsManager.SetMaxFocusPointsFromFocusLevel();

        playerManager.playerStatsManager.currentSoulCount = playerManager.playerStatsManager.currentSoulCount - soulsRequiredToLevelUp;
        playerManager.uiManager.soulCount.text = playerManager.playerStatsManager.currentSoulCount.ToString();

        gameObject.SetActive(false);
    }

    private void CalculateSoulCostToLevelUp()
    {
        for (int i = 0; i < projectedPlayerLevel; i++)
        {
            soulsRequiredToLevelUp = soulsRequiredToLevelUp + Mathf.RoundToInt((projectedPlayerLevel * baseLevelUpCost) * 1.5f);
        }
    }

    //Updates the projected player's total level, by adding up all the projected level up stats
    private void UpdateProjectedPlayerLevel()
    {
        soulsRequiredToLevelUp = 0;

        projectedPlayerLevel = currentPlayerLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(healthSlider.value) - playerManager.playerStatsManager.healthLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(staminaSlider.value) - playerManager.playerStatsManager.staminaLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(focusSlider.value) - playerManager.playerStatsManager.focusLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(poiseSlider.value) - playerManager.playerStatsManager.poiseLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(strengthSlider.value) - playerManager.playerStatsManager.strengthLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(dexteritySlider.value) - playerManager.playerStatsManager.dexterityLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(intelligenceSlider.value) - playerManager.playerStatsManager.intelligenceLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(faithSlider.value) - playerManager.playerStatsManager.faithLevel;
        projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

        CalculateSoulCostToLevelUp();
        soulsRequiredToLevelUpText.text = soulsRequiredToLevelUp.ToString();

        if (playerManager.playerStatsManager.currentSoulCount < soulsRequiredToLevelUp)
        {
            confirmLevelUpButton.interactable = false;
        }
        else
        {
            confirmLevelUpButton.interactable = true;
        }
    }

    public void UpdateHealthLevelSlider()
    {
        projectedHealthLevelText.text = healthSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateStaminaLevelSlider()
    {
        projectedStaminaLevelText.text = staminaSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateFocusLevelSlider()
    {
        projectedFocusLevelText.text = focusSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdatePoiseLevelSlider()
    {
        projectedPoiseLevelText.text = poiseSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateStrengthLevelSlider()
    {
        projectedStrengthLevelText.text = strengthSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateDexterityLevelSlider()
    {
        projectedDexterityLevelText.text = dexteritySlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateFaithLevelSlider()
    {
        projectedFaithLevelText.text = faithSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }

    public void UpdateIntelligenceLevelSlider()
    {
        projectedIntelligenceLevelText.text = intelligenceSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }
}

