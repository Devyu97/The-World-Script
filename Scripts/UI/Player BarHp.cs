using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarHp : MonoBehaviour
{
    public Slider bar;
    private float max;
    private float cur;

    private void Start()
    {
        max = PlayerManager.instance.maxHp;
        cur = PlayerManager.instance.curHp;
        bar.value = cur / max;
    }

    private void Update()
    {
        if (cur != PlayerManager.instance.curHp || max != PlayerManager.instance.maxHp)
        {
            cur = PlayerManager.instance.curHp;
            max = PlayerManager.instance.maxHp;
            bar.value = cur / max;
        }
    }
}
