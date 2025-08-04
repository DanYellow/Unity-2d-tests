using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Vector3 moveInput = Vector3.zero;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveInput != Vector3.zero)
        {
            animator.SetFloat("LinearVelocityX", moveInput.x);
            animator.SetFloat("LinearVelocityY", moveInput.y);
        }

        animator.SetBool("IsMoving", moveInput != Vector3.zero);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }
}
