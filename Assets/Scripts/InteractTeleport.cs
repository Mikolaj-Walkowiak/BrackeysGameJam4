using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class InteractTeleport : Interactable
{
    public bool isGuarded;
    public NPC doorOwner;
    public Transform destination;

    public Item neededKey;
    public bool isOpen;
    private AudioClip closedClip;
    private AudioClip openedClip;
    private AudioSource audio;
    private bool isOpening;
    void Start()
    {
        isOpening = false;
        closedClip = Resources.Load<AudioClip>("SFX/KeyUnlock");
        openedClip = Resources.Load<AudioClip>("SFX/DoorOpening");
        audio = GetComponent<AudioSource>();

        if (!isOpen)
        {
            audio.clip = closedClip;
        }
        else
        {
            audio.clip = openedClip;
        }
    }

    IEnumerator Teleport()
    {
        while (audio.isPlaying)
            yield return null;
        if (isOpen)
            audio.clip = openedClip;
        otherObject.transform.position = new Vector3(destination.position.x, destination.position.y,
            otherObject.transform.position.z);
        yield return new WaitForSeconds(0.5f);
        if (isGuarded)
        {
            Debug.Log("GUARDED");
            if (doorOwner != null && doorOwner.GetComponent<EnemyHealth>().CurrentHealth>=0) StartEncounter();
        }
        

    }

    IEnumerator OpenAfter(float value)
    {
        isOpening = true;
        yield return new WaitForSeconds(value);
        isOpen = true;
    }
    
    public void UnlockDoors()
    {
        isOpen = true;
        isGuarded = false;
        audio.clip = openedClip;
    }

    void StartEncounter()
    {
        doorOwner.ForceDialogue();
    }
    void Update()
    {
        if (otherObject != null && otherObject.CompareTag("Player") && otherObject.GetComponent<PlayerController>().IsBusy())
            return;
        if (audio.isPlaying)
            return;
        if (inRange && Input.GetKeyDown("e") && isOpen)
        {
            audio.Play();
            StartCoroutine(Teleport());
        }
        else if (inRange && Input.GetKeyDown("e") && !isOpen)
        {
            Inventory otherInventory = otherObject.GetComponent<Inventory>();
            if (isGuarded)
            {
                if (doorOwner != null && neededKey == null)
                {
                    StartEncounter();
                }
                if (doorOwner != null && !isOpening && (otherInventory.items.Count - 1 >= otherInventory.activeItem &&
                                                        otherInventory.items[otherInventory.activeItem].itemName == neededKey.itemName)){ 
                    StartEncounter();
                    StartCoroutine(OpenAfter(5f));
                }
            }
            else
            {
                
                if ((otherInventory.items.Count - 1 >= otherInventory.activeItem &&
                     otherInventory.items[otherInventory.activeItem].itemName == neededKey.itemName))
                {
                    audio.Play();
                    isOpen = true;
                    StartCoroutine(Teleport());
                }
                else
                {
                    Debug.Log("No key :(");
                }
            }
        }
    }
}

