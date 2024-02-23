using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarSp : MonoBehaviour
{
    public Slider bar;
    private float max;
    private float cur;

    private void Start()
    {
        max = PlayerManager.instance.maxSp;
        cur = PlayerManager.instance.curSp;
        bar.value = cur / max;
    }

    private void Update()
    {
        if (cur != PlayerManager.instance.curSp || max != PlayerManager.instance.maxSp)
        {
            cur = PlayerManager.instance.curSp;
            max = PlayerManager.instance.maxSp;
            bar.value = cur / max;
        }
    }
}
