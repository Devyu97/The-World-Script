using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using static UnityEngine.UI.Image;

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter instance;

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Dictionary<string, Sprite> spriteSheet;

    private void Awake()
    {
        instance = this;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteSheet = new Dictionary<string, Sprite>();
        SaveSpriteSheet();
    }

    private void Start()
    {
        animator.SetFloat("speed", 0);
        animator.SetInteger("orientation", 0);
    }

    void FixedUpdate()
    {
        AnimationUpdate();
    }
    
    private void LateUpdate()
    {
        spriteRenderer.sprite = spriteSheet[spriteRenderer.sprite.name];

        ChangeSprite();
    }

    public void AnimationUpdate()
    {
        if (animator.runtimeAnimatorController.name != "Chara_animation") return;

        //Movement
        animator.SetFloat("speed", Mathf.Abs(PlayerMove.instance.movement.x) + Mathf.Abs(PlayerMove.instance.movement.y));
        if (PlayerMove.instance.movement.y < 0)
            animator.SetInteger("orientation", 0);
        if (PlayerMove.instance.movement.y > 0)
            animator.SetInteger("orientation", 9);
        if (PlayerMove.instance.movement.x < 0)
            animator.SetInteger("orientation", 3);
        if (PlayerMove.instance.movement.x > 0)
            animator.SetInteger("orientation", 6);

        //Attack
        if (PlayerManager.instance.isAttack == true && animator.GetInteger("orientation") == 0)
            animator.SetInteger("orientation", 1);
        if (PlayerManager.instance.isAttack == true && animator.GetInteger("orientation") == 3)
            animator.SetInteger("orientation", 4);
        if (PlayerManager.instance.isAttack == true && animator.GetInteger("orientation") == 6)
            animator.SetInteger("orientation", 7);
        if (PlayerManager.instance.isAttack == true && animator.GetInteger("orientation") == 9)
            animator.SetInteger("orientation", 10);
        //Attack End
        if (PlayerManager.instance.isAttack == false && animator.GetInteger("orientation") == 1)
            animator.SetInteger("orientation", 0);
        if (PlayerManager.instance.isAttack == false && animator.GetInteger("orientation") == 4)
            animator.SetInteger("orientation", 3);
        if (PlayerManager.instance.isAttack == false && animator.GetInteger("orientation") == 7)
            animator.SetInteger("orientation", 6);
        if (PlayerManager.instance.isAttack == false && animator.GetInteger("orientation") == 10)
            animator.SetInteger("orientation", 9);

        //Slash
        if (PlayerManager.instance.isSlash == true && animator.GetInteger("orientation") == 0)
            animator.SetInteger("orientation", 2);
        if (PlayerManager.instance.isSlash == true && animator.GetInteger("orientation") == 3)
            animator.SetInteger("orientation", 5);
        if (PlayerManager.instance.isSlash == true && animator.GetInteger("orientation") == 6)
            animator.SetInteger("orientation", 8);
        if (PlayerManager.instance.isSlash == true && animator.GetInteger("orientation") == 9)
            animator.SetInteger("orientation", 11);
        //Slash End
        if (PlayerManager.instance.isSlash == false && animator.GetInteger("orientation") == 2)
            animator.SetInteger("orientation", 0);
        if (PlayerManager.instance.isSlash == false && animator.GetInteger("orientation") == 5)
            animator.SetInteger("orientation", 3);
        if (PlayerManager.instance.isSlash == false && animator.GetInteger("orientation") == 8)
            animator.SetInteger("orientation", 6);
        if (PlayerManager.instance.isSlash == false && animator.GetInteger("orientation") == 11)
            animator.SetInteger("orientation", 9);
    }

    private void LoadSpriteSheet(string spriteName)
    {
        // var : c++ ���� auto �� ����  
        string spriteFilePath;
        spriteFilePath = "Characters/chara_0" + "/" + spriteName;

        //���� ��ΰ� �߸�C�Ȱ�� ����ó��
        if (File.Exists(spriteFilePath))
        {
            Debug.Log("Sprite file does not exist: " + spriteName);
            return;
        }

        Sprite[] additionalSprites = Resources.LoadAll<Sprite>(spriteFilePath);
        foreach (Sprite sprite in additionalSprites)
        {
            if (!spriteSheet.ContainsKey(sprite.name))
            {
                spriteSheet.Add(sprite.name, sprite);
            }
        }
    }

    private void SaveSpriteSheet()
    {
        //�����̸��� Characters/chrara_(ĳ���͹�ȣ) �ȿ� �ִ� ��������Ʈ �̸����� ����
        //��� ĳ������ ��������Ʈ�鵵 ������ �̸����� ����
        LoadSpriteSheet("character");
        LoadSpriteSheet("walk_shields_1");
        LoadSpriteSheet("walk_shields_2");
        LoadSpriteSheet("walk_shields_3");
        LoadSpriteSheet("walk_shields_4");
        LoadSpriteSheet("walk_shields_5");
        LoadSpriteSheet("srw_arpg_slash01_0");
        LoadSpriteSheet("srw_arpg_slash01_1");
        LoadSpriteSheet("srw_arpg_slash01_2");
        LoadSpriteSheet("srw_arpg_slash01_3");
        LoadSpriteSheet("srw_arpg_slash01_4");
        LoadSpriteSheet("srw_arpg_slash01_5");
        LoadSpriteSheet("srw_arpg_sword01_0");
        LoadSpriteSheet("srw_arpg_sword01_1");
        LoadSpriteSheet("srw_arpg_sword01_2");
        LoadSpriteSheet("srw_arpg_sword01_3");
        LoadSpriteSheet("srw_arpg_sword01_4");
        LoadSpriteSheet("srw_arpg_sword01_5");
        LoadSpriteSheet("srw_arpg_sword02_0");
        LoadSpriteSheet("srw_arpg_sword02_1");
        LoadSpriteSheet("srw_arpg_sword02_2");
        LoadSpriteSheet("srw_arpg_sword02_3");
        LoadSpriteSheet("srw_arpg_sword02_4");
        LoadSpriteSheet("srw_arpg_sword02_5");
        LoadSpriteSheet("srw_arpg_sword03_0");
        LoadSpriteSheet("srw_arpg_sword03_1");
        LoadSpriteSheet("srw_arpg_sword03_2");
        LoadSpriteSheet("srw_arpg_sword03_3");
        LoadSpriteSheet("srw_arpg_sword03_4");
        LoadSpriteSheet("srw_arpg_sword03_5");
    }
    private void ChangeSprite()
    {      
        string swordLevel;
        PlayerManager.instance.equipments.TryGetValue(EquipmentType.MainHand, out ItemData mainHand);
        if(mainHand != null)
        {
            Match sword = Regex.Match(mainHand.name, @"(\d+)$");
            if (sword.Success)
                swordLevel = sword.Value;
            else
                swordLevel = "0";
        }
        else
        {
            swordLevel = "0";
        }

        string shieldLevel;
        PlayerManager.instance.equipments.TryGetValue(EquipmentType.OffHand, out ItemData offHand);
        if(offHand != null)
        {
            Match shield = Regex.Match(offHand.name, @"(\d+)$");
            if (shield.Success)
                shieldLevel = shield.Value;
            else
                shieldLevel = "0";
        }
        else
        {
            shieldLevel = "0";
        }

        PlayerManager.instance.equipments.TryGetValue(EquipmentType.OffHand, out ItemData offHand_);
        if (offHand != null)
        {         
            //���� ��µǴ� ��������Ʈ�� �̸� 
            string curSpriteName = spriteRenderer.sprite.name;
            string[] parts = curSpriteName.Split('_');

            //�⺻ ������ ����� ���
            if (parts[0] == "character")
            {
                //���Խ� ǥ�� @"(\d+)$" : �ϳ��̻��� ����(d+)�� ���ڿ� ��($)�� ������ �κ��� ã�ƶ�
                Match match = Regex.Match(curSpriteName, @"(\d+)$");
                if (match.Success)
                {
                    string extracted = match.Value;
                    string temp = "walk_shields_" + shieldLevel + "_" + extracted;
                    spriteRenderer.sprite = spriteSheet[temp];
                }
            }
        }

        //Slash
        if (PlayerManager.instance.isSlash == true)
        {        
            //���� ��µǴ� ��������Ʈ�� �̸� 
            string curSpriteName = spriteRenderer.sprite.name;
            string[] parts = curSpriteName.Split('_');

            if (parts[0] != "srw") return;

            //slash ����� ���
            if (parts[2] == "slash01")
            {
                Match match = Regex.Match(curSpriteName, @"(\d+)$");
                if (match.Success)
                {
                    string extracted = match.Value;

                    //���и� ����������
                    if (offHand != null)
                    {
                        string temp = "srw_arpg_slash01_" + shieldLevel + "_" + extracted;
                        spriteRenderer.sprite = spriteSheet[temp];
                    }
                    else
                    {
                        string temp = "srw_arpg_slash01_0" + "_" + extracted;
                        spriteRenderer.sprite = spriteSheet[temp];
                    }                 
                }
            }
        }    
      
        //Attack
        if (PlayerManager.instance.isAttack == true)
        {
            string curSpriteName = spriteRenderer.sprite.name;
            string[] parts = curSpriteName.Split("_");

            if (parts[0] != "srw") return;

            Match match = Regex.Match(curSpriteName, @"(\d+)$");
            if(match.Success)
            {
                string extracted = match.Value;

                //���⸦ ����������
                if (PlayerManager.instance.CheckEquip(EquipmentType.MainHand))
                {
                    if (swordLevel == "0") return;

                    string temp = "srw_arpg_sword0" + swordLevel + "_" + shieldLevel + "_" + extracted;
                    spriteRenderer.sprite = spriteSheet[temp];
                }
                else
                {
                    if (swordLevel == "0") return;
                    string temp = "srw_arpg_sword0" + swordLevel + "_0" + "_" + extracted;
                    spriteRenderer.sprite = spriteSheet[temp];
                }
            }
        }
    }
}
