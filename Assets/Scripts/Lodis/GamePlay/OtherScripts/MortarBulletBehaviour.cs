﻿using Lodis;
using Lodis.GamePlay.GridScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarBulletBehaviour : BulletBehaviour {

    public override void ResolveCollision(GameObject other)
    {
        HealthBehaviour healthScript = other.GetComponent<HealthBehaviour>();

        if (healthScript)
            healthScript.takeDamage(DamageVal);
        else if (other.CompareTag("Panel"))
            BlackBoard.grid.ExplodePanel(other.GetComponent<PanelBehaviour>(), true, 5);

        Destroy(TempObject);
    }
}
