using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerAttack : MonoBehaviour
{
    // [SerializeField] FloatVariable loadCloneDuration;
    [SerializeField] FloatVariable loadCloneProgression;
    private float loadCloneDuration;
    private bool isUnloading = false;
    private bool isLoadingAttack = false;

    private IEnumerator coroutine;

    private void Awake()
    {
        loadCloneProgression.CurrentValue = 0;
        coroutine = FillLoadAttack();
    }

    void Start()
    {

    }

    IEnumerator FillLoadAttack()
    {
        float timeElapsed = 0;
        loadCloneProgression.CurrentValue = timeElapsed;

        while (loadCloneProgression.CurrentValue <= 1)
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

    // https://www.youtube.com/watch?v=PDtRY99lD4Y
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
                    isLoadingAttack = false;
                    // Debug.Log("ctx.cancelled " + loadCloneProgression.CurrentValue);
                    // StopCoroutine(coroutine);
                    StopAllCoroutines();
                    StartCoroutine(ClearLoadAttack());
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

        // SlowTapInteraction
        // if (ctx.phase == InputActionPhase.Performed)
        // {
        //     Debug.Log("ctx.phase " + ctx.phase);

        // }
        // else if (ctx.phase == InputActionPhase.Canceled)
        // {
        //     Debug.Log("ctx.phase " + ctx.phase);
        // }
        // else if (ctx.phase == InputActionPhase.Started)
        // {
        //     Debug.Log("ctx.phase " + ctx.phase);
        //     var holdInteraction = ctx.interaction as HoldInteraction;
        //     Debug.Log("ctx.phase " + holdInteraction.duration);
        // }
    }
}
