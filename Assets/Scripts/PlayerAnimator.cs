using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Vector3Variable playerMoveInput;

    // public G isClone = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMoveInput.CurrentValue != Vector3.zero)
        {
            animator.SetFloat("LinearVelocityX", playerMoveInput.CurrentValue.x);
            animator.SetFloat("LinearVelocityY", playerMoveInput.CurrentValue.y);
        }

        animator.SetBool("IsMoving", playerMoveInput.CurrentValue != Vector3.zero);
    }
}
