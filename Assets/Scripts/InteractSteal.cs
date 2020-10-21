using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
public class InteractSteal : Interactable
{
    public bool isVital;
    public string globalVariable ="";
    public InMemoryVariableStorage inMemoryVariableStorage;
    private bool itemStolen;
    public Item itemToSteal;
    private AudioSource audio;
    public GameObject Ground;
    void Start()
    {
        Ground = GroundFind.instance.gameObject;
        if (GetComponent<AudioSource>() == null)
            audio = gameObject.AddComponent<AudioSource>();
        else
            audio = GetComponent<AudioSource>();
        audio.playOnAwake = false;
    }
    void Update()
    {
        if (otherObject != null && otherObject.GetComponent<PlayerController>().IsBusy())
            return;
        
        if (inRange && Input.GetKeyDown("e"))
        {
            if (!itemStolen)
            {
                Debug.Log("Stealing " + itemToSteal.itemName + " from " + gameObject.name);
                StartCoroutine(Steal());
            }
        }
    }

    IEnumerator Steal()
    {
        GameObject chleb = (GameObject)Instantiate(itemToSteal.gameObject, otherObject.transform.position, Quaternion.identity);
        chleb.GetComponent<SpriteRenderer>().enabled = false;
        chleb.transform.parent = Ground.transform;
        chleb.GetComponent<Item>().shot = false;
        if(isVital){
            inMemoryVariableStorage.SetValue("$" + globalVariable,1);
        }

        itemStolen = true;
        audio.clip = Resources.Load<AudioClip>("SFX/GunPickup");
        audio.Play();
        while (audio.isPlaying)
            yield return null;

        if (!otherObject.GetComponent<Inventory>().Add(chleb.GetComponent<Item>()))
            itemStolen = false;
        else
        {
            chleb.SetActive(false);
            chleb.GetComponent<SpriteRenderer>().enabled = true;
        }

    }
}
