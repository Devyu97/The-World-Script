using System.Collections.Generic;
using UnityEngine;

public class NpcSpeech : MonoBehaviour
{
    [Header("Set")]
    public List<string> hello;
    public Transform speechPoint;
    public UnityEngine.GameObject speechPrefab;

    private void Update()
    {
        Talk();
    }

    public void Talk()
    {
        Transform dialog = speechPoint.Find("Speech(Clone)");
        if (dialog != null) return;

        UnityEngine.GameObject dialogInstance = Instantiate(speechPrefab);
        dialogInstance.transform.SetParent(speechPoint, false);
        dialogInstance.GetComponent<NpcSpeechSystem>().OnSpeech(hello);
    }
}
