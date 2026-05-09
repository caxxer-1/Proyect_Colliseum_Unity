using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NoticerSO")]
public class NoticerSO : ScriptableObject
{
    public event EventHandler<OnMessageSentBasicBuild> OnMessageSent;
    public void Call(OnMessageSentBasicBuild onMessageSentBasicBuild)
    {
        OnMessageSent?.Invoke(this, onMessageSentBasicBuild);
    }
}
