using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LevelUpUI : MonoBehaviour
{
    public PlayerStatsManager playerStatsManager;

    [Header("Player Level")]
    public int currentPlayerLevel; //THE CURRENT LEVEL WE ARE BEFORE LEVELING UP
    public int projectedPlayerLevel; //THE POSSIBLE LEVEL WE WILL BE IF WE ACCEPT LEVELING UP
    public TextMeshProUGUI currentPlayerLevelText; //THE UI TEXT FOR THE NUMBER OF THE CURRENT PLAYER LEVEL
    public TextMeshProUGUI projectedPlayerLevelText; //THE UI TEXT FOR THE PROJECTED PLAYER LEVEL NUMBER

    [Header("Souls")]
    public TextMeshProUGUI currentSouls;
    public TextMeshProUGUI soulsRequiredToLevelUp;

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
        currentPlayerLevel = playerStatsManager.playerLevel;
        currentPlayerLevelText.text = currentPlayerLevel.ToString();

        projectedPlayerLevel = playerStatsManager.playerLevel;
        projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

        healthSlider.value = playerStatsManager.healthLevel;
        healthSlider.minValue = playerStatsManager.healthLevel;
        healthSlider.maxValue = 99;
        currentHealthLevelText.text = playerStatsManager.healthLevel.ToString();
        projectedHealthLevelText.text = playerStatsManager.healthLevel.ToString();
        
        staminaSlider.value = playerStatsManager.staminaLevel;
        staminaSlider.minValue = playerStatsManager.staminaLevel;
        staminaSlider.maxValue = 99;
        currentStaminaLevelText.text = playerStatsManager.staminaLevel.ToString();
        projectedStaminaLevelText.text = playerStatsManager.staminaLevel.ToString();

        focusSlider.value = playerStatsManager.focusLevel;
        focusSlider.minValue = playerStatsManager.focusLevel;
        focusSlider.maxValue = 99;
        currentFocusLevelText.text = playerStatsManager.focusLevel.ToString();
        projectedFocusLevelText.text = playerStatsManager.focusLevel.ToString();

        poiseSlider.value = playerStatsManager.poiseLevel;
        poiseSlider.minValue = playerStatsManager.poiseLevel;
        poiseSlider.maxValue = 99;
        currentPoiseLevelText.text = playerStatsManager.poiseLevel.ToString();
        projectedPoiseLevelText.text = playerStatsManager.poiseLevel.ToString();

        strengthSlider.value = playerStatsManager.strengthLevel;
        strengthSlider.minValue = playerStatsManager.strengthLevel;
        strengthSlider.maxValue = 99;
        currentStrengthLevelText.text = playerStatsManager.strengthLevel.ToString();
        projectedStrengthLevelText.text = playerStatsManager.strengthLevel.ToString();

        dexteritySlider.value = playerStatsManager.dexterityLevel;
        dexteritySlider.minValue = playerStatsManager.dexterityLevel;
        dexteritySlider.maxValue = 99;
        currentDexterityLevelText.text = playerStatsManager.dexterityLevel.ToString();
        projectedDexterityLevelText.text = playerStatsManager.dexterityLevel.ToString();

        intelligenceSlider.value = playerStatsManager.intelligenceLevel;
        intelligenceSlider.minValue = playerStatsManager.intelligenceLevel;
        intelligenceSlider.maxValue = 99;
        currentIntelligenceLevelText.text = playerStatsManager.intelligenceLevel.ToString();
        projectedIntelligenceLevelText.text = playerStatsManager.intelligenceLevel.ToString();

        faithSlider.value = playerStatsManager.faithLevel;
        faithSlider.minValue = playerStatsManager.faithLevel;
        faithSlider.maxValue = 99;
        currentFaithLevelText.text = playerStatsManager.faithLevel.ToString();
        projectedFaithLevelText.text = playerStatsManager.faithLevel.ToString();
    }

    //Updates the projected player's total level, by adding up all the projected level up stats
    private void UpdateProjectedPlayerLevel()
    {
        projectedPlayerLevel = currentPlayerLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(healthSlider.value) - playerStatsManager.healthLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(staminaSlider.value) - playerStatsManager.staminaLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(focusSlider.value) - playerStatsManager.focusLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(poiseSlider.value) - playerStatsManager.poiseLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(strengthSlider.value) - playerStatsManager.strengthLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(dexteritySlider.value) - playerStatsManager.dexterityLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(intelligenceSlider.value) - playerStatsManager.intelligenceLevel;
        projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(faithSlider.value) - playerStatsManager.faithLevel;

        projectedPlayerLevelText.text = projectedPlayerLevel.ToString();
    }

    public void UpdateHealthLevelSlider()
    {
        projectedHealthLevelText.text = healthSlider.value.ToString();
        UpdateProjectedPlayerLevel();
    }
}
