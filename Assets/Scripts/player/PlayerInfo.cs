using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private Text nameText;

    public void UpdateInfo(string name, int era)
    {
        nameText.text = name + (era >= 0 ? " [" + PlayerEra2.Name(era) + "]" : "");
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        //transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
