using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LevelUpUI : MonoBehaviour
{
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
    public Slider PoiseSlider;
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


}
