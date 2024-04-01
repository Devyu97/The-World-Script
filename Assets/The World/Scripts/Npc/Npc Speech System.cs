using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcSpeechSystem : MonoBehaviour
{
    public TextMeshPro curSentence;
    public UnityEngine.GameObject backGround;

    private Queue<string> sentence; 
    
    public void OnSpeech(List<string> dialog)
    {
        sentence = new Queue<string>();
        sentence.Clear();
        foreach (string s in dialog)
        {
            sentence.Enqueue(s);
        }
        StartCoroutine(SpeechFlow());
    }

    IEnumerator SpeechFlow() //�����ʿ�
    {   
        Transform speechPoint = transform.parent;
        Transform npc = speechPoint.transform.parent;
        RectTransform rectTransform = GetComponent<RectTransform>();

        while (sentence.Count > 0)
        { 
            curSentence.text = sentence.Dequeue();

            //Dialog width ����
            rectTransform.sizeDelta = new Vector2(curSentence.preferredWidth, 1);
            //������� ����ũ�⿡ �°� ����
            backGround.transform.localScale = new Vector2(curSentence.preferredWidth, curSentence.preferredHeight); 

            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + curSentence.preferredHeight * 0.5f);         
            transform.localPosition = Vector2.zero;

            yield return new WaitForSeconds(2f);
        }

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
