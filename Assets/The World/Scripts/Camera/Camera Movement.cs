using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CameraMovement : MonoBehaviour
{
    private float height; //������� ī�޶� ������ ���� ũ��
    private float width; //������� ī�޶� ����
    public float speed;//ī�޶� �̵��ӵ�

    private Transform cameraLimitRange; //���� ���õ� ī�޶� ���� ����

    private void Start()
    {
        height = Camera.main.orthographicSize ;
        width = height * Screen.width / Screen.height;
    }

    public void ChangeLimit(string name)
    {
        if (SceneManager.GetActiveScene().name == "Title Screen")
            return;

        Transform temp = GameManager.instance.cameraLimits[name];
        if (temp == null) return;

        cameraLimitRange = GameManager.instance.cameraLimits[name];    
    }

    //LateUpdate : Update ���� �����ӿ� ȣ��Ǵ� �Լ�
    //�÷��̾ ������ ������ ������Ʈ
    private void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Title Screen")
            return;

        if (GameManager.instance == null) return;

        ChangeLimit(GameManager.instance.curerntCameraLimitName);

        //#.Vector3 Lerp(Vector3 A, Vector3 B, float t) : A �� B ������ ���� t ���� ���� ��ȯ
        transform.position = Vector3.Lerp(transform.position, PlayerManager.instance.transform.position, Time.deltaTime * speed);

        //#.Mathf.Clamp(value, min, max) : value���� min�� max���̸� value�� ��ȯ�ϰ� min���� ������ min�� max���� ũ�� max��ȯ
        float lx = cameraLimitRange.localScale.x * 0.5f - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + cameraLimitRange.position.x, lx + cameraLimitRange.position.x);     
        float ly = cameraLimitRange.localScale.y * 0.5f - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + cameraLimitRange.position.y, ly + cameraLimitRange.position.y); 

        transform.position = new Vector3(clampX, clampY, -10f);
    }    

    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }
}
