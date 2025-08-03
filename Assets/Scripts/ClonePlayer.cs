using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using System.Collections;

public class ClonePlayer : MonoBehaviour
{
    [SerializeField] private VoidEventChannel OnCloneAttackReady;
    [SerializeField] private GameObject player;

    [SerializeField] FloatVariable loadCloneProgression;
    [SerializeField] ListVector listClonesPosition;

    [SerializeField] FloatVariable numberClonesCreated;
    [SerializeField] float loadSpeed = 2.25f;

    [SerializeField] private VoidEventChannel OnCloneAttackReset;

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
    }

    // Update is called once per frame
    void CreateClones()
    {
        foreach (var position in listClonesPosition.CurrentValue)
        {
            var clonedPlayer = Instantiate(player, position, Quaternion.identity);
            clonedPlayer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.75f);


            var cloneInstance = clonedPlayer.GetComponent<Clone>();
            cloneInstance.target = gameObject.transform;
            cloneInstance.offsetWithTarget = position - gameObject.transform.position;
        }


        ResetState();
    }

    IEnumerator FillLoadAttack()
    {
        float timeElapsed = 0;
        loadCloneProgression.CurrentValue = timeElapsed * loadSpeed;

        while (loadCloneProgression.CurrentValue < 1)
        {
            timeElapsed += Time.deltaTime;
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
            timeElapsed -= Time.deltaTime;
            loadCloneProgression.CurrentValue = Mathf.Clamp01(timeElapsed / loadCloneDuration);

            yield return null;
        }
        isUnloading = false;

    }

    public void OnLoadAttack(InputAction.CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Performed:
                // {
                //     Debug.Log("ctx.phase " + ctx.phase);
                // }
                break;
            case InputActionPhase.Canceled:
                {
                    ResetState();
                }
                break;
            case InputActionPhase.Started:
                {
                    // isLoadingAttack = true;
                    if (isUnloading)
                    {
                        return;
                    }
                    Debug.Log("start");
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

    private void OnDisable()
    {
        OnCloneAttackReady.OnEventRaised -= CreateClones;
    }
}
