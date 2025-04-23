using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueInfo", menuName = "Scriptable Objects/DialogueInfo")]
public class DialogueInfo : ScriptableObject
{
    public List<int> ids;
    public List<string> lines;
}
