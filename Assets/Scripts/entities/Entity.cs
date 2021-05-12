using System;
using UnityEngine;
using Photon.Pun;

public class Entity : MonoBehaviour
{
    [SerializeField]
    private bool highlighted = false;

    private Outline outline;

    protected PhotonView photonView;
    public bool IsPun
    {
        get { return photonView != null; }
    }
    public bool IsMine
    {
        get { return IsPun && photonView.IsMine;  }
    }

    public bool Highlighted
    {
        get
        {
            return highlighted;
        }
        set
        {
            if (outline != null)
            {
                outline.enabled = value;
            }

            highlighted = value;
        }
    }

    protected virtual void Awake()
    {
        photonView = GetComponent<PhotonView>();

        // Setup outline
        outline = GetComponent<Outline>();
        Debug.Log(outline);
        Highlighted = highlighted;
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public virtual void Interact(PlayerManager playerManager)
    {
    }

    public virtual bool CanInteract()
    {
        return true;
    }

    public virtual string GetDescription()
    {
        return "";
    }
}
