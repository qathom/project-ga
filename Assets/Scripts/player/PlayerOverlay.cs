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
}
