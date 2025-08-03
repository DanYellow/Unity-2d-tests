using UnityEngine;

public class CloneArea : MonoBehaviour
{
    [SerializeField] ListVector listClonesPosition;

    [SerializeField] GameObject cloneAreaActivated;

    [SerializeField] FloatVariable loadCloneProgression;
    [SerializeField] FloatVariable numberMaxClones;
    [SerializeField] FloatVariable numberClonesCreated;

    [SerializeField] private VoidEventChannel OnCloneAttackReady;
    [SerializeField] private VoidEventChannel OnCloneAttackReset;

    private void Awake()
    {
        cloneAreaActivated.SetActive(false);
        numberClonesCreated.CurrentValue = 0;
    }

    private void OnEnable()
    {
        OnCloneAttackReset.OnEventRaised += ResetState;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        print("enter");
        if (
            listClonesPosition.CurrentValue.Contains(transform.position) ||
            loadCloneProgression.CurrentValue < 1
        )
        {
            print("enter 2");

            return;
        }

        if (collision.CompareTag("Player"))
        {
            if (numberMaxClones.CurrentValue == numberClonesCreated.CurrentValue)
            {
                collision.gameObject.transform.position = transform.position;
                OnCloneAttackReady.Raise();
            }
            else
            {
                numberClonesCreated.CurrentValue++;
                cloneAreaActivated.SetActive(true);
                listClonesPosition.CurrentValue.Add(transform.position);
            }
        }
    }

    void ResetState()
    {
        cloneAreaActivated.SetActive(false);
        numberClonesCreated.CurrentValue = 0;
    }

    private void OnDisable()
    {
        OnCloneAttackReset.OnEventRaised -= ResetState;
    }
}
