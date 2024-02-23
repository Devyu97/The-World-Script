using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMessageUI : MonoBehaviour
{
    public static GameMessageUI instance;

    [Header("Set")]
    public UnityEngine.GameObject content = null;
    public UnityEngine.GameObject messagePrefab = null;

    private void Awake()
    {
        instance = this;
    }

    public void SystemMessage(string message, Color color, string system = "System")
    {
        //메세지 형식 : [ system ] message(color) 

        //#.Rich Text
        string colorHex = UIManager.instance.ColorToHex(color);
        string colorTag = "<color=" + colorHex + ">";

        string temp = $"[ <b>" + colorTag + $"{system}</b></color> ] {message}";

        if (instance == null) return;

        UnityEngine.GameObject gameMessage = Instantiate(messagePrefab, content.transform);
        StartCoroutine(gameMessage.GetComponent<GameMessage>().Message(temp));
    }
}
