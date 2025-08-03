using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Float Value", menuName = "ScriptableObjects/Variables/ListVector", order = 0)]
public class ListVector : ScriptableObject
{
    public List<Vector3> CurrentValue;

    #pragma warning disable 0414
    [Multiline, SerializeField]
    private string DeveloperDescription = "";
    #pragma warning restore 0414
}
