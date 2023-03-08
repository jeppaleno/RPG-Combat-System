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

    public int pendingCriticalDamage;
}
