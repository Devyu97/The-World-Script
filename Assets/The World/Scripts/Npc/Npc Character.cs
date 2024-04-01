using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCharacter : MonoBehaviour
{
    [Header("Set")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Sprite[] sprites;

    private void Awake()
    {
        string[] part = spriteRenderer.sprite.name.Split('_');
        string npcName = part[0];
        sprites = Resources.LoadAll<Sprite>("Characters/Npcs/" + npcName);
    }

    private void LateUpdate()
    {
        //#.스프라이트 업데이트
        string curSpriteName = spriteRenderer.sprite.name;
        string[] parts = curSpriteName.Split("_");
        int temp = int.Parse(parts[1]);
        spriteRenderer.sprite = sprites[temp];
    }
}
