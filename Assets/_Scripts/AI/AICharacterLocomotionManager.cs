using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AICharacterLocomotionManager : MonoBehaviour
{
    AICharacterManager aiCharacter;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;

    public LayerMask detectionLayer;

    private void Awake()
    {
        aiCharacter = GetComponent<AICharacterManager>();
    }

    private void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

}
