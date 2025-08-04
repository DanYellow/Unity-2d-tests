using UnityEngine;

[CreateAssetMenu(fileName = "New Vector3 Value", menuName = "ScriptableObjects/Variables/Vector3Variable", order = 0)]
public class Vector3Variable : ScriptableObject
{
    public Vector3 CurrentValue;

    #pragma warning disable 0414
    [Multiline, SerializeField]
    private string DeveloperDescription = "";
    #pragma warning restore 0414
}
