using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    public override int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

   public override int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            CheckHealth();
        }
    }

}
