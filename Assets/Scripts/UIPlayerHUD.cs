using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHUD : MonoBehaviour
{
    [SerializeField] FloatVariable loadCloneProgression;
    [SerializeField] Image loadAttack;
    
    private void Awake()
    {
        loadAttack.fillAmount = loadCloneProgression.CurrentValue;
    }

    void Update()
    {
        loadAttack.fillAmount = loadCloneProgression.CurrentValue;
    }
}
