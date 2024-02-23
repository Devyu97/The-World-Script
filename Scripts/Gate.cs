using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    public GameObject transferGate;    //�̵��� ����Ʈ 
    public string transferSceneName;   //�̵��� �� �̸�
    public string CameraLimitName;     //ī�޶� ���� ���� �̸� 
   

    IEnumerator GateRoutine()
    {
        //�ڷ�ƾ(Coroutine) : �񵿱� �۾��� �����ϰ� �ð� ������ �����ϱ� ���� ���
        //�ڷ�ƾ�� ����ϸ� ���ӷ����� �������� �ʰ� �ð����� �۾��� �����ϰų� ������ �۾��� ���� �����ӿ� ���� �л��ų �� �ִ�.
        yield return new WaitForSeconds(0.5f); //���ð�

        if (transferSceneName != SceneManager.GetActiveScene().name)
        {
            GameManager.instance.beforeSceneName = SceneManager.GetActiveScene().name;
            GameManager.instance.currentSceneName = transferSceneName;    
            SceneManager.LoadScene(transferSceneName);
        }

        if(transferGate != null)
        {
            Camera.main.GetComponent<CameraMovement>().ChangeLimit(CameraLimitName);
            Camera.main.GetComponent<CameraMovement>().SetPos(transferGate.transform.position);
            PlayerManager.instance.transform.position = transferGate.transform.position; //�÷��̾ �̵��� gate position����  
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(GateRoutine());  
        }
    }
}
