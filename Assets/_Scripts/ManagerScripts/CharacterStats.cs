using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public int staminaLevel = 10;
    public int maxStamina;
    public int currentStamina;

    public int soulCount = 0;

    public int focusLevel = 10;
    public int maxFocusPoints;
    public int currentFocusPoints;

    public bool isDead;

    public virtual void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {

    }
}
