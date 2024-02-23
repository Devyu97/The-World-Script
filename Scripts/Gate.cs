using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    public GameObject transferGate;    //이동할 게이트 
    public string transferSceneName;   //이동할 씬 이름
    public string CameraLimitName;     //카메라 제한 범위 이름 
   

    IEnumerator GateRoutine()
    {
        //코루틴(Coroutine) : 비동기 작업을 수행하고 시간 지연을 관리하기 위한 기능
        //코루틴을 사용하면 게임루프를 차단하지 않고도 시간지연 작업을 수행하거나 복잡한 작업을 여러 프레임에 걸쳐 분산시킬 수 있다.
        yield return new WaitForSeconds(0.5f); //대기시간

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
            PlayerManager.instance.transform.position = transferGate.transform.position; //플레이어를 이동할 gate position으로  
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
