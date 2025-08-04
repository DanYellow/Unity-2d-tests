using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 moveInput = Vector3.zero;

    private Vector2 nextPosition;

    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        nextPosition = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
        // rb.linearVelocity = nextPosition;
        rb.MovePosition(transform.position + (Vector3) nextPosition * Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }
}
