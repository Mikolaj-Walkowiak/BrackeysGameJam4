using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;

    protected int currentHealth;

    public virtual int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public virtual int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
        }
    }
    
    protected void Start()
    {
        currentHealth = maxHealth;
    }
    
    protected void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Debug.Log(gameObject.name + "ZDECHLES :((((((((((((((((((((((((((((");
            if (gameObject.CompareTag("Player"))
            {
                
                gameObject.GetComponentInChildren<Animator>().SetBool("isDead", true);
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                gameObject.GetComponent<Animator>().SetBool("isDead", true);
            }
        }
        else
        {
            if (gameObject.CompareTag("Player"))
            {
                gameObject.GetComponentInChildren<Animator>().SetBool("isDead", false);
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                gameObject.GetComponent<Animator>().SetBool("isDead", false);
            }
        }
    }
}
