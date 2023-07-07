using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    [Header("Item Quantity")]
    public int maxItemAmount;
    public int currentItemAmount;

    [Header("Item Model")]
    public GameObject itemModel;

    [Header("Animations")]
    public string consumeAnimation;
    public bool isInteracting;

    public virtual void AttemptToConsumeItem(PlayerManager player)
    {
        if (currentItemAmount > 0)
        {
            player.playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true, true);
        }
        else
        {
            player.playerAnimatorManager.PlayTargetAnimation("shrug", true);
        }
    }

    public virtual void SucessfullyToConsumeItem(PlayerManager player)
    {
        currentItemAmount = currentItemAmount - 1;
    }

    public virtual bool CanIUseThisItem(PlayerManager player)
    {
        return true;
    }
}
