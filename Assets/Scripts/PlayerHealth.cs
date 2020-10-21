using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public override int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            UpdateUI();
            CheckHealth();
        }
    }

    public Transform healthParent;
    public HealthSlot[] healthSlots;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 3;
        currentHealth = maxHealth;
        healthSlots = healthParent.GetComponentsInChildren<HealthSlot>();
    }

    public void TakeDamage(int value)
    {
        CurrentHealth -= value;
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            CurrentHealth -= 1;
        }

        if (Input.GetKeyDown(KeyCode.N))
            CurrentHealth += 1;
    }*/
    
    void UpdateUI()
    {
        //Debug.Log("Updating Health UI");
        for (int i = 0; i < maxHealth; i++)
        {
            if (i < currentHealth)
            {
                healthSlots[i].AddHealth();
            }
            else
            {
                healthSlots[i].ClearHealth();
            }
        }
    }

}
