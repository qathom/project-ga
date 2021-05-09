using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    private bool highlighted = false;

    private Outline outline;

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

    private void Awake()
    {
        // Setup outline
        outline = GetComponent<Outline>();
        Debug.Log(outline);
        Highlighted = highlighted;
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public virtual void Interact(PlayerManager playerManager)
    {
        Debug.Log("INTERACT?");
    }
}
