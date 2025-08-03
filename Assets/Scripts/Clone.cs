using UnityEngine;

public class Clone : MonoBehaviour
{
    [SerializeField] private VoidEventChannel OnCloneAttackReset;

    private Rigidbody2D rb;

    public Vector3 offsetWithTarget;
    public Transform target;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // OnCloneAttackReset.OnEventRaised += ResetState;
    }

    void ResetState()
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        var nextPosition = target.transform.position + offsetWithTarget;

        rb.MovePosition(nextPosition);
    }

    private void OnDisable()
    {
        // OnCloneAttackReset.OnEventRaised -= ResetState;
    }
}
