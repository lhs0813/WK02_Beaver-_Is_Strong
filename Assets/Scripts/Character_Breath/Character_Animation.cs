using UnityEngine;
using System.Collections;

public class Character_Animation : MonoBehaviour
{
   
    public Vector3 initialScale;   // 원래 크기 저장
    private Vector3 initialPosition; // 원래 위치 저장
    public float breathAmount = 0.1f; // 숨쉬기 크기 변화량
    public float breathSpeed = 2f; // 숨쉬기 속도 (1초에 얼마나 변화할지)
    public float spinHeight = 1f;  // 공격 시 상승 높이
    public float spinDuration = 0.5f; // 회전 애니메이션 지속 시간

    private void Awake()
    {
        
    }
    void Start()
    {
        initialScale = transform.localScale; // 시작 시 원래 크기 저장
        initialPosition = transform.position; // 시작 시 원래 위치 저장
        StartCoroutine(BreathAnimation()); // 숨쉬기 애니메이션 시작
    }

    IEnumerator BreathAnimation()
    {
        while (true) // 반복문처럼 계속해서 애니메이션 실행
        {
            float time = Mathf.PingPong(Time.time * breathSpeed, 1); // 0과 1 사이를 오가게 만들어줌
            transform.localScale = initialScale + Vector3.one * Mathf.Sin(time * Mathf.PI) * breathAmount;

            yield return null; // 한 프레임 대기
        }
    }

    public void Spin_Attack()
    {
        StopCoroutine(BreathAnimation()); // 숨쉬기 애니메이션 중지
        StartCoroutine(SpinAttackAnimation());
    }

    IEnumerator SpinAttackAnimation()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = initialPosition; // 항상 같은 시작 위치 사용
        Vector3 targetPosition = startPosition + new Vector3(0, spinHeight, 0);

        while (elapsedTime < spinDuration)
        {
            float t = elapsedTime / spinDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // Z축 회전을 수동으로 증가시키기
            float currentZRotation = Mathf.Lerp(0, 360, t);
            transform.rotation = Quaternion.Euler(0, 0, currentZRotation);
              
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 원래 위치로 돌아오기
        transform.position = initialPosition;
        transform.rotation = Quaternion.Euler(0, 0, 0); // 최종적으로 회전값을 0으로 초기화
        StartCoroutine(BreathAnimation()); // 숨쉬기 애니메이션 다시 시작
    }


}
