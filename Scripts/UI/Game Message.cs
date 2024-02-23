using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMessage : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public RectTransform rectTransform;
    private UnityEngine.GameObject content;

    private void Init(string message)
    {
        textMeshPro.text = message;

        float hight = textMeshPro.preferredHeight;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, hight);
        content = transform.parent.gameObject;

        foreach (Transform child in content.transform)
        {
            if (child == null) return;
            if (child == this.transform) return;

            //메세지가 추가될때 새메시지의 preferredHight만큼 원래있던 메시지의 PosY 에 더하기
            Vector3 temp = child.GetComponent<RectTransform>().localPosition;
            child.GetComponent<RectTransform>().localPosition = new Vector3(temp.x, temp.y + hight, 0);
        }
    }

    public IEnumerator Message(string message)
    {
        Init(message);

        yield return new WaitForSeconds(1f);

        while(textMeshPro.color.a > 0)
        {
            yield return null;

            Color currentColor = textMeshPro.color;
            currentColor.a -= Time.deltaTime * 0.3f;
            textMeshPro.color = currentColor;
        }

        Destroy(gameObject);
    }
}
