using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private Text nameText;

    public void UpdateInfo(string name, PlayerEra? era)
    {
        nameText.text = name + (era != null ? " [" + era.ToString() + "]" : "");
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        //transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
