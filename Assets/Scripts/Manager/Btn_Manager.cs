using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Btn_Manager : MonoBehaviour
{
    public static Btn_Manager Instance { get; private set; }

    [SerializeField]
    public Transform Hand; // 이동시킬 오브젝트

    public Vector3 original_Position;
    public Vector3 targetPosition = new Vector3(0, -5.5f, 0); // 목표 위치
    //public Vector3 Change;
    public float moveDuration = 2.0f; // 이동 시간
    public bool Now_Position = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    void Start()
    {
        Hand = FindAnyObjectByType<Hand>().transform;
        original_Position = Hand.transform.position;
    }

    public void Hand_Clicked()
    {
        Debug.Log("호출됨");
        // 버튼 클릭 시 오브젝트 이동 시작
        if (Hand != null)
        {
            if(Now_Position == true)
            {
                StartCoroutine(MoveObject(original_Position, targetPosition));
            }
            else
            {

                StartCoroutine(MoveObject(targetPosition, original_Position));
            }
            
        }
        else
        {
            Debug.LogError("이동시킬 오브젝트가 할당되지 않았습니다.");
        }
    }

    



    IEnumerator MoveObject(Vector3 startPos, Vector3 endPos)
    {
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            // 시간에 따라 보간
            float t = elapsedTime / moveDuration;
            Hand.position = Vector3.Lerp(startPos, endPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 위치 설정
        Hand.position = endPos;
        if(Now_Position == true)
        {
            Now_Position = false;
        }
        else
        {
            Now_Position = true;
        }
    }



}