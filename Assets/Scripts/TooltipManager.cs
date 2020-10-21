using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TooltipManager : MonoBehaviour
{
    #region Singleton
    public static TooltipManager instance;
    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion

    public Text tooltipText;
    public GameObject tooltipPanel;

    void Start()
    {
        tooltipPanel.SetActive(false);
    }
    public void OpenTooltipPanel()
    {
        tooltipPanel.SetActive(true);
        
    }

    public void CloseTooltipPanel()
    {
        tooltipPanel.SetActive(false);
    }

    public void SetTooltipText(string text)
    {
        tooltipText.text = text;
    }
}
