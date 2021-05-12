using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOverlay : MonoBehaviour
{
    [SerializeField]
    private Text eraText;

    public void UpdateEra(int era)
    {
        eraText.text = era < 0
            ? "No Era Selected"
            : PlayerEra2.Name(era) + " ERA";
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
