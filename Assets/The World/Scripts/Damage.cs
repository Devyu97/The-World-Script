using System.Collections;
using TMPro;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField]
    private float speed; //�ؽ�Ʈ �̵��ӵ�
    [SerializeField]
    private float alphaSpeed; //�ؽ�Ʈ ���� ��ȯ�ӵ�
    [SerializeField]
    private float destroyTime = 2f;

    TextMeshPro text;
    Color alpha;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        alpha = text.color;
    }

    private void Start()
    {
        Invoke("DestroyObject", destroyTime);
    }
    private void Update()
    {
        StartCoroutine(UpdateText());
    }

    public IEnumerator UpdateText()
    {
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
        
        yield return null;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void SetDamageText(float damage = 1)
    {
        int temp = (int)damage;
        text.text = temp.ToString();
    }
}
