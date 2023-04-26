using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Aim Action")]
public class AimAction : ItemAction
{
    public override void PerformAction(PlayerManager player)
    {
        if (player.isAiming)
            return;

        player.uiManager.crossHair.SetActive(true);
        player.isAiming = true;
    }
}
