using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public delegate void UIControl();
    public static event UIControl OnUI;

    [Header("Set")]
    public UnityEngine.GameObject npcInteractionUI;
    public UnityEngine.GameObject playerInventoryUI;
    public UnityEngine.GameObject playerQuestListUI;
    public UnityEngine.GameObject dialogUI;
     

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        #endregion

        //#.Npc Interation UI
        npcInteractionUI.gameObject.SetActive(false);
    }

    private void Start()
    {
        OnUI += OpenInventoryUI;
        OnUI += OpenNpcInteractionUI;
        OnUI += OpenPlayerQuestListUI;
        OnUI += OpenDialogUI;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Title Screen")
            return;

        OnUI();
    }

    #region OpenUI  

    private void OpenNpcInteractionUI()
    {
        if (playerInventoryUI.activeSelf) return;
        if (playerQuestListUI.activeSelf) return;
        if (GameManager.curInteractingNpc == null) return;

        if (npcInteractionUI.activeSelf)
        {
            PlayerManager.instance.playerControl = false;
            PlayerCharacter.instance.animator.SetFloat("speed", 0);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                npcInteractionUI.SetActive(!npcInteractionUI.activeSelf);
                PlayerManager.instance.playerControl = !npcInteractionUI.activeSelf;
            }
        }
    }

    private void OpenInventoryUI()
    {
        if (npcInteractionUI.activeSelf) return;
        if (playerQuestListUI.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            playerInventoryUI.SetActive(!playerInventoryUI.activeSelf);
            PlayerManager.instance.playerControl = !playerInventoryUI.activeSelf;

            //수정
            if (playerInventoryUI.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    playerInventoryUI.SetActive(!playerInventoryUI.activeSelf);
                    PlayerManager.instance.playerControl = !playerInventoryUI.activeSelf;
                }
                AnimatorController idle = Resources.Load<AnimatorController>("Characters/Animations/Player/IDLE_S");
                PlayerCharacter.instance.animator.runtimeAnimatorController = idle;
            }
            if (!playerInventoryUI.activeSelf)
            {
                AnimatorController origin = Resources.Load<AnimatorController>("Characters/Animations/Player/Chara_animation");
                PlayerCharacter.instance.animator.runtimeAnimatorController = origin;
            }
        }
    }

    private void OpenPlayerQuestListUI()
    {
        if (playerInventoryUI.activeSelf) return;
        if (npcInteractionUI.activeSelf) return;

        if(Input.GetKeyDown(KeyCode.L))
        {
            playerQuestListUI.SetActive(!playerQuestListUI.activeSelf);
            PlayerManager.instance.playerControl = !playerQuestListUI.activeSelf;
        }

        if (playerQuestListUI.activeSelf)
        {
            PlayerManager.instance.playerControl = false;
            PlayerCharacter.instance.animator.SetFloat("speed", 0);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                playerQuestListUI.SetActive(!playerQuestListUI.activeSelf);
                PlayerManager.instance.playerControl = !playerQuestListUI.activeSelf;
            }
        }
    }

    private void OpenDialogUI()
    {
        if (dialogUI.activeSelf)
        {
            PlayerManager.instance.playerControl = false;
            PlayerCharacter.instance.animator.SetFloat("speed", 0);
        }
    }
    #endregion


    #region Funtion

    public void ButtonInit(UnityEngine.GameObject gameObject)
    {
        //#.버튼 선택
        Selectable[] buttons = gameObject.GetComponentsInChildren<Selectable>();
        if (buttons.Length > 0)
        {
            buttons[0].Select();
        }
    }

    public string ColorToHex(Color color)
    {
        Color32 color32 = (Color32)color;
        return $"#{color32.r:X2}{color32.g:X2}{color32.b:X2}{color32.a:X2}";
    }

    #endregion
}
