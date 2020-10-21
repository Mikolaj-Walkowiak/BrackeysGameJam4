using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewindUI : MonoBehaviour
{
    public Text remainingRewindsText;

    public Animator stopwatchAnimator;

    public RewindLogic rewindLogic;

    private RewindManager rewind;
    // Start is called before the first frame update
    void Start()
    {
        rewind = RewindManager.instance;
        remainingRewindsText.text = rewind.rewindsRemaining.ToString();
        rewindLogic.onRewindUsedCallback += UpdateUI;
    }


    void UpdateUI()
    {
        Debug.Log("Callback invoke rewindUI");
        remainingRewindsText.text = rewind.rewindsRemaining.ToString();
        stopwatchAnimator.SetTrigger("rewindUsed");
        stopwatchAnimator.SetInteger("rewindNumber", rewind.rewindsRemaining);
    }
}
