using UnityEngine;
using Mirror;

// Controls player's Animator and syncs animations accross network
public class PlayerAnimatorController : NetworkBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // called on all clients by server immediately set animation after a player spawns
    [ClientRpc]
    public void RpcForceAnimationState(int move)
    {
        if (animator == null)
        {
            Debug.LogWarning($"[{netId}] Animator is null");
            return;
        }

        Debug.Log($"[{netId}] RpcForceAnimationState({move})");
        animator.SetInteger("PlayerMove", move); // set int parameter PlayerMove in the Animator, triggers animation based on value
    }
}
