using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;
public class StartScript : MonoBehaviour
{
    public NPC startNPC;
    // Start is called before the first frame update
        void OnEnable()
        {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }
    
        void OnDisable()
        {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        IEnumerator StartDialogue(){
            yield return new WaitForSeconds(0.5f);
            startNPC.ForceDialogue();
        }
        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Level Loaded");
            Debug.Log(scene.name);
            Debug.Log(mode);
            StartCoroutine(StartDialogue());
        }
}
