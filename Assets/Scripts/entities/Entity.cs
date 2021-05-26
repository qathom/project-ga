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

    public virtual void AttachTo(Transform parent)
    {
        transform.SetParent(parent);
    }

    public virtual bool CanInteract(PlayerManager playerManager)
    {
        return true;
    }

    public virtual bool CanPickUp(PlayerManager playerManager)
    {
        return false;
    }

    public virtual string GetInteractionHint(PlayerManager playerManager)
    {
        return "Press 'e' to interact!";
    }

    public virtual string GetDescription(PlayerManager playerManager)
    {
        return "Interactable Entity";
    }
}
