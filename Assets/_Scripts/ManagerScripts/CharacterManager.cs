using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    [Header("Combat Colliders")]
    public BoxCollider backStabBoxCollider;
    public BackStabCollider backStabCollider;

    //Damage will be inflicted during an animation event
    // Used in backstab or riposte animations
    public int pendingCriticalDamage;
}
