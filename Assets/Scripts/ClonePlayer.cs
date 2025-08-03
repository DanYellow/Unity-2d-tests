using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using System.Collections;
using System.Collections.Generic;

public class ClonePlayer : MonoBehaviour
{
    [SerializeField] private VoidEventChannel OnCloneAttackReady;
    [SerializeField] private GameObject player;

    [SerializeField] FloatVariable loadCloneProgression;
    [SerializeField] ListVector listClonesPosition;

    [SerializeField] FloatVariable numberClonesCreated;
    [SerializeField] float loadSpeed = 2.25f;

    private bool cloneCreated = false;

    [SerializeField] private VoidEventChannel OnCloneAttackReset;
    [SerializeField] private VoidEventChannel OnCloneContact;

    private List<GameObject> listClones = new();

    private float loadCloneDuration;
    private bool isUnloading = false;

    private void Awake()
    {
        loadCloneProgression.CurrentValue = 0;
        listClonesPosition.CurrentValue.Clear();
    }

    private void OnEnable()
    {
        OnCloneAttackReady.OnEventRaised += CreateClones;
        OnCloneContact.OnEventRaised += CloneContact;
    }

    // Update is called once per frame
    void CreateClones()
    {
        cloneCreated = true;
        foreach (var position in listClonesPosition.CurrentValue)
        {
            var clonedPlayer = Instantiate(player, position, Quaternion.identity);
            clonedPlayer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.75f);
            listClones.Add(clonedPlayer);

            var cloneInstance = clonedPlayer.GetComponent<Clone>();
            cloneInstance.target = gameObject.transform;
            cloneInstance.offsetWithTarget = position - gameObject.transform.position;
        }

        ResetState();
    }

    void DestroyClones()
    {
        cloneCreated = false;
        foreach (var clone in listClones)
        {
            Destroy(clone);
        }
        listClones.Clear();
    }

    IEnumerator FillLoadAttack()
    {
        float timeElapsed = 0;
        loadCloneProgression.CurrentValue = timeElapsed;

        while (loadCloneProgression.CurrentValue < 1)
        {
            timeElapsed += Time.deltaTime * loadSpeed;
            loadCloneProgression.CurrentValue = Mathf.Clamp01(timeElapsed / loadCloneDuration);

            yield return null;
        }
    }

    IEnumerator ClearLoadAttack()
    {
        if (isUnloading)
        {
            yield return null;
        }

        isUnloading = true;
        float timeElapsed = loadCloneProgression.CurrentValue * loadCloneDuration;

        while (timeElapsed > 0)
        {
            float speedFactor = cloneCreated ? 0.35f : 1.5f;
            timeElapsed -= Time.deltaTime * speedFactor;
            loadCloneProgression.CurrentValue = Mathf.Clamp01(timeElapsed / loadCloneDuration);
            // loadCloneProgression.CurrentValue -= 1.0f / loadCloneDuration * Time.deltaTime;;

            yield return null;
        }
        isUnloading = false;

        DestroyClones();
    }

    public void OnLoadAttack(InputAction.CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Canceled:
                {
                    ResetState();
                }
                break;
            case InputActionPhase.Started:
                {
                    if (isUnloading)
                    {
                        return;
                    }

                    var holdInteraction = ctx.interaction as HoldInteraction;
                    loadCloneDuration = holdInteraction.duration;

                    StartCoroutine(FillLoadAttack());
                }
                break;
            default:
                break;
        }
    }

    private void ResetState()
    {
        StopAllCoroutines();
        listClonesPosition.CurrentValue.Clear();
        OnCloneAttackReset.Raise();
        StartCoroutine(ClearLoadAttack());
        numberClonesCreated.CurrentValue = 0;
    }

    private void CloneContact()
    {
        DestroyClones();
        ResetState();
    }

    private void OnDisable()
    {
        OnCloneAttackReady.OnEventRaised -= CreateClones;
        OnCloneContact.OnEventRaised -= CloneContact;
    }
}
