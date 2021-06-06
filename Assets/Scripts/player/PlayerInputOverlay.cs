using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputOverlay : MonoBehaviour
{
    [SerializeField]
    private Text inputText;

    public void SendInput()
    {
        print("send input");
        Entity entity = PlayerManager.LocalInstance.SelectedEntity;
        if (entity != null && entity is InputEntity)
        {
            InputEntity inputEntity = (InputEntity) entity;
            if (inputEntity.SendInput(PlayerManager.LocalInstance, inputText.text))
            {
                print("close");
                PlayerManager.LocalInstance.CloseMenu();
            }
            else
            {
                print("fail");
                inputText.color = new Color(1.0f, 0.2f, 0.2f);
            }
        }
    }

    public void ClearColor()
    {
        inputText.color = Color.black;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        ClearColor();
    }
}
