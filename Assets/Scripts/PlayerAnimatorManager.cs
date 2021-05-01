using UnityEngine;
using Photon.Pun;

public class PlayerAnimatorManager : MonoBehaviourPun
{
    private Animator animator;

    [SerializeField]
    private float directionDampTime = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (!animator)
        {
            Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;

        if (!animator) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        animator.SetFloat("Speed", v);
        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
    }
}
