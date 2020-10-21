using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private TooltipManager tooltip;

    public String tooltipText;

    private bool isTooltipVisible;
    // Start is called before the first frame update
    void Start()
    {
        isTooltipVisible = false;
        tooltip = TooltipManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isTooltipVisible)
        {
            isTooltipVisible = true;
            tooltip.SetTooltipText(tooltipText);
            tooltip.OpenTooltipPanel();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isTooltipVisible)
        {
            isTooltipVisible = false;
            tooltip.CloseTooltipPanel();
        }
    }
}
