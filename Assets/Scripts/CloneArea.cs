using UnityEngine;

public class CloneArea : MonoBehaviour
{
    [SerializeField] ListVector listClonesPosition;

    [SerializeField] GameObject cloneAreaActivated;

    [SerializeField] FloatVariable loadCloneProgression;

    private void Awake()
    {
        cloneAreaActivated.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (
            listClonesPosition.CurrentValue.Contains(transform.position) ||
            loadCloneProgression.CurrentValue < 1
        )
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            cloneAreaActivated.SetActive(true);
            listClonesPosition.CurrentValue.Add(transform.position);
        }
    }
}
