using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    public Slider slider;

    private AsyncOperation async;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            LoadScene();
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneCoroutine("SampleScene"));
    }

    IEnumerator LoadSceneCoroutine(string name)
    {
        async = SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            slider.value = async.progress;
            if (async.progress == 0.9f)
            {
                slider.value = 1f;
                async.allowSceneActivation = true;
                yield return null;
            }
        }
    }
}
