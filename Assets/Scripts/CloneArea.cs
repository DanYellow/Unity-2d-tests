using UnityEngine;

public class CloneArea : MonoBehaviour
{
    [SerializeField] ListVector listClonesPosition;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (listClonesPosition.CurrentValue.Contains(transform.position))
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            listClonesPosition.CurrentValue.Add(transform.position);
        }
    }
}
