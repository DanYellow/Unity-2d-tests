using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 nextPosition;

    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3Variable playerMoveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        playerMoveInput.CurrentValue = Vector3.zero;
    }

    private void FixedUpdate()
    {
        nextPosition = new Vector2(playerMoveInput.CurrentValue.x * moveSpeed, playerMoveInput.CurrentValue.y * moveSpeed);
        // rb.linearVelocity = nextPosition;
        rb.MovePosition(transform.position + (Vector3)nextPosition * Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        playerMoveInput.CurrentValue = (Vector3)ctx.ReadValue<Vector2>();
    }
}
