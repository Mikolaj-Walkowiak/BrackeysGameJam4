using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.XR;
using Yarn.Unity;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class NPC : MonoBehaviour
{
    public bool isPatrolling = true;
    public bool canBeHostile;
    private NPCManager npcManager;
    
    private bool isInDialogue = false;
    public string dialogueNodeName;
    private bool inRange;

    private Collider2D collider2D;
   
    public DialogueRunner dialogueRunner;
    private DialogueUI dialogueUI;

    private float timeToDisappear = 21f;
    public Sprite aliveSprite;
    public Sprite angrySprite;
    public Sprite deadSprite;
    private bool isDead;
    private bool wasDead;

    public bool hasAGun = false;
    public bool isAngry;

    private bool wasAngry;

    private Health health;
    private Rigidbody2D rb2d;
    public Transform shootPosition;
    public Item weapon;
    public GameObject weaponGameObject;
    private AudioSource shotAudioSource;
    private Light2D weaponLight;
    public GameObject muzzleFlash;
    public GameObject bulletPrefab;
    public PlayerController player;
    public GameObject Ground;

    public RewindManager rewind;
    public float moveSpeed = 1f;
    public Path path;
    
    private Vector2 currentPathTarget;
    public int currentPathTargetIndex;
    

    public List<Item> items = new List<Item>();

    private Animator anim;

    private bool animMove;
    private float animRotation;
    public float idleRotation;

    public bool dropsOnDeath;
    public Item itemToDropOnDeath;
    void Start()
    {
        npcManager = NPCManager.instance;
        Ground = GroundFind.instance.gameObject;
        rewind = RewindManager.instance;
        player = PlayerController.instance;
        
        rb2d = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        dialogueUI = GetComponentInChildren<DialogueUI>();
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        
        currentPathTargetIndex = 0;
        if(path != null)
            currentPathTarget = path.positions[currentPathTargetIndex];
        if (canBeHostile)
        {
            weaponGameObject = Instantiate(items.Find(x => x.itemType == ItemType.Gun).gameObject, rb2d.position,
                Quaternion.identity);
            weaponGameObject.GetComponent<PickupObject>().canPickup = false;
            weaponGameObject.GetComponent<SpriteRenderer>().enabled = false;
            weaponGameObject.transform.parent = Ground.transform;
            weapon = weaponGameObject.GetComponent<Item>();


            shotAudioSource = GetComponent<AudioSource>();
            weaponLight = GetComponentInChildren<Light2D>();
            
        }


        
    }
    public void HandleOffDialogue()
    {
        isInDialogue = false;
    }

    void HandleNoAmmo(){
        if(hasAGun){
            if(weapon.currentAmmo == 0){
                npcManager.GetComponent<InMemoryVariableStorage>().SetValue("$hasNoAmmo", 1);
                moveSpeed = 0;
            }
        }
    }
    void Update()
    {
        HandleNoAmmo();
        UpdateShootPosition();
        UpdateAnimRotation();
        UpdateAnimMove();
        HandleStartDialogue();
        HandleSkipDialogue();
        SetAnims();
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
    
    
    void SetAnims()
    {
        if (anim == null)
            return;
        if (!isInDialogue && !rewind.IsRewinding)
        {
            anim.SetBool("move", animMove);
            anim.SetFloat("mouseRotation", animRotation);
        }

        else if (isInDialogue && !rewind.IsRewinding)
        {
            anim.SetBool("move", false);
            anim.SetFloat("mouseRotation", animRotation);
        }

    }
    private void FixedUpdate()
    {
        if (isDead || isInDialogue || rewind.IsRewinding)
            return;
        HandleMovement();
        HandleShooting();
        
    }

    public void ForceDialogue(){
        isInDialogue = true;
        dialogueRunner.StartDialogue(dialogueNodeName);
    }
    void HandleStartDialogue()
    {
        if (inRange && Input.GetKeyDown("f"))
        {
            isInDialogue = true;
            dialogueRunner.StartDialogue(dialogueNodeName);
        }
    }

    void HandleSkipDialogue()
    {
        if(isInDialogue && Input.GetButtonDown("Fire1")){
            dialogueUI.MarkLineComplete();
        }
    }

    void HandleMovement()
    {
        if (isAngry)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb2d.position, player.GetComponent<Transform>().position, Time.fixedDeltaTime * moveSpeed);
            rb2d.MovePosition(newPosition);
        }
        else
        {
            if (path == null)
                return;
            if (rb2d.position != currentPathTarget)
            {
                Vector2 newPosition = Vector2.MoveTowards(rb2d.position, currentPathTarget, Time.fixedDeltaTime * moveSpeed);
                rb2d.MovePosition(newPosition);
            }
            else
            {
               
                if (currentPathTargetIndex + 1 > path.positions.Count - 1 && isPatrolling)
                    currentPathTargetIndex = 0;
                else if (currentPathTargetIndex + 1 > path.positions.Count - 1 && !isPatrolling)
                {
                    moveSpeed = 0;
                }
                else
                    currentPathTargetIndex += 1;
                currentPathTarget = path.positions[currentPathTargetIndex];
            }
        }
    }
    void HandleShooting()
    {
        if (!isAngry && weapon != null)
        {
            return;
        }

        if (weapon != null && weapon.currentAmmo > 0)
        {
            Shoot();
        }

    }

    void UpdateAnimRotation()
    {
        if (path == null)
        {
            if(!isAngry)
                animRotation = idleRotation;
            else
            {
                Vector2 relativeTargetPos = player.GetComponent<Transform>().position - transform.position;
                float angleRadians = Mathf.Atan2(relativeTargetPos.y, relativeTargetPos.x);

                float animAngle = angleRadians * Mathf.Rad2Deg;
                animRotation = animAngle;
            }
        }
        else
        {
            if (isAngry)
            {
                Vector2 relativeTargetPos = player.GetComponent<Transform>().position - transform.position;
                float angleRadians = Mathf.Atan2(relativeTargetPos.y, relativeTargetPos.x);

                float animAngle = angleRadians * Mathf.Rad2Deg;
                animRotation = animAngle;
            }
            else
            {
                Vector2 relativeTargetPos = currentPathTarget - (Vector2) transform.position;
                float angleRadians = Mathf.Atan2(relativeTargetPos.y, relativeTargetPos.x);

                float animAngle = angleRadians * Mathf.Rad2Deg;
                animRotation = animAngle;
            }
        }

    }

    void UpdateAnimMove()
    {
        animMove = moveSpeed != 0;
    }
    void UpdateShootPosition()
    {
        if (weapon == null)
            return;
        Vector2 relativeTargetPos = player.GetComponent<Transform>().position - transform.position + (Vector3)player.GetComponent<PlayerMovement>().movement;
        float angleRadians = Mathf.Atan2(relativeTargetPos.y, relativeTargetPos.x);

        float shootAngle = angleRadians * Mathf.Rad2Deg;
        shootPosition.localEulerAngles = new Vector3(0, 0, shootAngle+180);
        weaponLight.transform.localEulerAngles = new Vector3(0, 0, shootAngle - 90);
    }
    
    public void Shoot()
    {

        if (weapon.shot)
            return;
        
        StartCoroutine(weapon.ShootDelay());
        GameObject muzzle = Instantiate(muzzleFlash, shootPosition.position, shootPosition.rotation);
        StartCoroutine(Flash());
        Destroy(muzzle, 0.15f);
        shotAudioSource.clip = weapon.shotSound;
        shotAudioSource.Play();
        GameObject bullet = Instantiate(bulletPrefab, shootPosition.position, shootPosition.rotation);
        bullet.GetComponent<EnemyBullet>().item = weapon;
        bullet.GetComponent<EnemyBullet>().shotBy = gameObject;
        weapon.currentAmmo -= 1;
    }

    IEnumerator Flash()
    {
        weaponLight.intensity = 8f;
        yield return new WaitForSeconds(0.02f);
        weaponLight.intensity = 0;
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
        if(dropsOnDeath)
            DropItem(itemToDropOnDeath);
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
    }
    
    public void DropItem(Item item)
    {
        //GameObject droppedItem = Instantiate(Resources.Load(item.itemName + "_OnGround")) as GameObject;
        //droppedItem.transform.parent = Ground.transform;
        GameObject droppedItem;
        
        if (Ground.transform.Find(item.gameObject.name) == null)
        {
            droppedItem = Instantiate(item.gameObject,
                new Vector3(transform.position.x, transform.position.y-1.2f, 0), Quaternion.identity);
            droppedItem.transform.parent = Ground.transform;
            
        }
        else
        {
            droppedItem = item.gameObject;
            droppedItem.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            droppedItem.SetActive(true);
        }
        droppedItem.GetComponent<PickupObject>().item = item;
        //necessary or it will add up

    }

}
