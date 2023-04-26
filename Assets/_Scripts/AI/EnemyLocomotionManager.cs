using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyLocomotionManager : MonoBehaviour
{
    EnemyManager enemy;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;

    public LayerMask detectionLayer;

    private void Awake()
    {
        enemy = GetComponent<EnemyManager>();
    }

    private void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

}
