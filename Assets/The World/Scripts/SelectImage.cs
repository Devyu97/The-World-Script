using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectImage : MonoBehaviour
{
    public UnityEngine.GameObject selectImage;

    private void OnEnable()
    {
        UIManager.OnUI += ToggleSelectImage;
    }
    private void OnDisable()
    {
        UIManager.OnUI -= ToggleSelectImage;   
    }

    private void ToggleSelectImage()
    {
        selectImage.SetActive(EventSystem.current.currentSelectedGameObject == gameObject);
    }
}
