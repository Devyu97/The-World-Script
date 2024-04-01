using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CameraMovement : MonoBehaviour
{
    private float height; //월드상의 카메라 세로의 절반 크기
    private float width; //월드상의 카메라 가로
    public float speed;//카메라 이동속도

    private Transform cameraLimitRange; //현재 선택된 카메라 제한 범위

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

    //LateUpdate : Update 다음 프레임에 호출되는 함수
    //플레이어가 움직인 다음에 업데이트
    private void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Title Screen")
            return;

        if (GameManager.instance == null) return;

        ChangeLimit(GameManager.instance.curerntCameraLimitName);

        //#.Vector3 Lerp(Vector3 A, Vector3 B, float t) : A 와 B 사이의 값을 t 값에 따라 반환
        transform.position = Vector3.Lerp(transform.position, PlayerManager.instance.transform.position, Time.deltaTime * speed);

        //#.Mathf.Clamp(value, min, max) : value값이 min과 max사이면 value를 반환하고 min보다 작으면 min을 max보다 크면 max반환
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
