using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBuffConsumableItem : MonoBehaviour
{
    [Header("Effect")]
    [SerializeField] WeaponBuffEffect weaponBuffEffect;

    [Header("Buff SFX")]
    [SerializeField] AudioClip buffTriggerSound;
}
