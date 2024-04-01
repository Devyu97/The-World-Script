using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") 
            return;

        GameManager.instance.curerntCameraLimitName = gameObject.name;
    }
}
