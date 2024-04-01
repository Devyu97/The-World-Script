using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;


public class DialogUI : MonoBehaviour
{
    public static DialogUI instance;

    public UnityEngine.GameObject main;
    public UnityEngine.GameObject dialog;
    public TextMeshProUGUI playerDialogText;
    public TextMeshProUGUI npcDialogText;

    private Queue<string> sentence;
    private DataManager.NpcDialog npc = new DataManager.NpcDialog();

    public enum Npc_Dialog
    {None,first,second,third}
    private Npc_Dialog npcA;
    private Npc_Dialog npcB;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        npcA = Npc_Dialog.first;
        npcB = Npc_Dialog.None;

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        main.SetActive(true);
        dialog.SetActive(false);

        if(UIManager.instance != null )
        {
            UIManager.instance.ButtonInit(main);
        }        
    }

    private void OnDisable()
    {        
        if (PlayerManager.instance != null)
        {
            PlayerManager.instance.playerControl = true;
        }
    }

    private void NpcA()
    {
        List<string> temp = new List<string>();

        switch (npcA)
        {
            case Npc_Dialog.None:
                if (npc.dialog.TryGetValue("None", out temp))
                    OnDialog(temp);
                break;
            case Npc_Dialog.first:
                if (npc.dialog.TryGetValue("First", out temp))
                    OnDialog(temp);
                npcA = Npc_Dialog.second;
                npcB = Npc_Dialog.first;
                break;
            case Npc_Dialog.second:
                if (npc.dialog.TryGetValue("Second", out temp))
                    OnDialog(temp);
                break;
            case Npc_Dialog.third:
                if (npc.dialog.TryGetValue("Third", out temp))
                    OnDialog(temp);
                npcA = Npc_Dialog.None;
                break;
        }

        return;
    }

    private void NpcB()
    {
        List<string> temp = new List<string>();

        switch (npcB)
        {
            case Npc_Dialog.None:
                if (npc.dialog.TryGetValue("None", out temp))
                    OnDialog(temp);
                break;
            case Npc_Dialog.first:
                if (npc.dialog.TryGetValue("First", out temp))
                    OnDialog(temp);
                npcA = Npc_Dialog.third;
                npcB = Npc_Dialog.None;
                //아이템 지급
                PlayerInventory.instance.ItemAcquisition("Sword1");
                PlayerInventory.instance.ItemAcquisition("Shield1");
                break;
        }
    }


    private void OnDialog(List<string> dialog)
    {
        sentence = new Queue<string>();
        sentence.Clear();
        foreach (string s in dialog)
        {
            sentence.Enqueue(s);
        }
        StartCoroutine(DialogFollow());
    }

    private IEnumerator DialogFollow()
    {
        bool start = false;

        while (sentence.Count > 0)
        {
            if(start)
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                yield return new WaitForSeconds(0.2f);   
            }

            playerDialogText.gameObject.SetActive(false);
            npcDialogText.gameObject.SetActive(false);
            string curSentence = sentence.Dequeue();
            char speeker = curSentence[0];
            switch (speeker)
            {
                case 'P':
                    //#.RegexOptions.Singleline : 개행문자 포함
                    string playerDialog = Regex.Match(curSentence, @"P(.*)", RegexOptions.Singleline).Groups[1].Value;
                    playerDialogText.text = playerDialog;
                    playerDialogText.gameObject.SetActive(true);
                    start = true;
                    break;
                case 'N':
                    string npcDialog = Regex.Match(curSentence, @"N(.*)", RegexOptions.Singleline).Groups[1].Value;
                    npcDialogText.text = npcDialog;
                    npcDialogText.gameObject.SetActive(true);
                    start = true;
                    break;
            }   
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        start = false;
        main.SetActive(true);
        dialog.SetActive(false);
    }

    public void OnClickDialogButton()
    {
        main.SetActive(false);
        dialog.SetActive(true);

        if (DataManager.instance.npcDialog.TryGetValue(GameManager.curInteractingNpc.name, out npc))
        {
            switch (npc.name)
            {
                case "Npc A":
                    NpcA();
                    break;
                case "Npc B":
                    NpcB();
                    break;
            }
            npc.dialog = null;
            npc.name = null;
        }
        else
        {
            main.SetActive(true);
            dialog.SetActive(false);
            return;
        }          
    }

    public void OnClickExitButton()
    {
        GameManager.curInteractingNpc = null;
        gameObject.SetActive(false);
    }

    public void OnClickInteractionButton()
    {
        main.SetActive(false);
        dialog.SetActive(false);
        gameObject.SetActive(false);

        UIManager.instance.npcInteractionUI.SetActive(true);
    }
}
