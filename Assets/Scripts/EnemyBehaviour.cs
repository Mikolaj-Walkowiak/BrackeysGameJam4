using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
public class EnemyBehaviour : MonoBehaviour
{
    /*private NPCManager npcManager;
    
    private bool isInDialogue = false;
    public string dialogueNodeName;
    private bool inRange;

    private Collider2D collider2D;
   
    public DialogueRunner dialogueRunner;
    public DialogueUI dialogueUI;

    private float timeToDisappear = 7f;
    public Sprite aliveSprite;
    public Sprite angrySprite;
    public Sprite deadSprite;
    private bool isDead;
    private bool wasDead;
    private bool isAngry;
    private bool wasAngry;

    private Health health;
    void Start()
    {
        npcManager = NPCManager.instance;
        
        collider2D = GetComponent<Collider2D>();
        dialogueUI = GetComponentInChildren<DialogueUI>();
        health = GetComponent<Health>();
    }
    public void HandleOffDialogue()
    {
        isInDialogue = false;
    }
    void Update()
    {
        if (inRange && Input.GetKeyDown("f"))
        {
            isInDialogue = true;
            dialogueRunner.StartDialogue(dialogueNodeName);
        }
        if(isInDialogue && Input.GetButtonDown("Fire1")){
            dialogueUI.MarkLineComplete();
        }
        if (isAngry){
            //Debug.Log("MAX" + health.MaxHealth);
            //Debug.Log("CURR" + health.CurrentHealth);
            if(health.MaxHealth == health.CurrentHealth){
                isAngry = false;
                wasAngry = true;
                gameObject.GetComponent<SpriteRenderer>().sprite = aliveSprite;
            }
        }
        if(isDead){
         timeToDisappear -= Time.deltaTime;
         if(timeToDisappear < 0)
         {
             npcManager.npcs.Remove(gameObject);
             Destroy(gameObject);
         }
         else if (health.CurrentHealth > 0){
             Ressurect();
         }
        }
    }
    public void TakeDamage(int damage)
    {

        health.CurrentHealth -= damage;
        isAngry = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = angrySprite;
        if (health.CurrentHealth <= 0 && !isDead)
        {
            Die();
        }
    }
    void Die()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = deadSprite;
        collider2D.enabled = false;
        isDead = true;
    }
    
    void Ressurect(){
        isDead = false;
        wasDead = true;
        timeToDisappear = 7f;
        gameObject.GetComponent<SpriteRenderer>().sprite = angrySprite;
        collider2D.enabled = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }*/
}
