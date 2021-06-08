using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOverlay : MonoBehaviour
{
    [SerializeField]
    private Text eraText;

    [SerializeField]
    private GameObject selectionPanel;

    [SerializeField]
    private Text selectionDescriptionText;

    [SerializeField]
    private Text interactHintText;

    [SerializeField]
    private GameObject gameEndedPanel;

    [SerializeField]
    private Text leaveCooldownText;

    private void Start()
    {
        gameEndedPanel.SetActive(false);
    }

    public void UpdateEra(int era)
    {
        eraText.text = era < 0
            ? "No Era Selected"
            : PlayerEra2.Name(era) + " ERA";
    }

    public void UpdateSelectionPanel(bool canInteract, string hint, string description)
    {
        interactHintText.enabled = canInteract;
        interactHintText.text = hint;
        selectionDescriptionText.text = description;
    }

    public void ToggleSelectionPanel(bool show)
    {
        selectionPanel.SetActive(show);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    internal void UpdateGameEnded(bool hasGameEnded, float leaveCooldown)
    {
        gameEndedPanel.SetActive(hasGameEnded);
        if (hasGameEnded)
        {
            int cooldown = (int) Math.Ceiling(leaveCooldown);
            leaveCooldownText.text = "Leaving in " + cooldown.ToString() + " seconds.";
        }
    }
}
