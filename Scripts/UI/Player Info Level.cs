using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoLevel : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;

    public int level;

    private void Start()
    {
        level = PlayerManager.instance.level;
        textMeshProUGUI.text = level.ToString();
    }

    private void Update()
    {
        if(level != PlayerManager.instance.level)
        {
            level = PlayerManager.instance.level;
            textMeshProUGUI.text = level.ToString();
        }
    }
}
