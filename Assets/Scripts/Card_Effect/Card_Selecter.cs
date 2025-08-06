using UnityEngine;
using System.Collections;
using TMPro;

public class Card_Selecter : MonoBehaviour
{
    public GameObject Text_Prefab;
    
    [SerializeField]
    private float scaleThreshold = 4.5f;  // 카드 크기가 줄어드는 Y축 값
    [SerializeField]
    private float scaleFactor = 0.5f;    // 크기 감소 비율
    



    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Quaternion initialRotation;

    private bool isDragging = false;
    private bool isUse = false;

    Vector3 mousePosition;

    // 카드별 표시할 텍스트 (Inspector에서 설정 가능)
    [SerializeField] private string cardText;

    private void OnMouseDown()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        initialRotation = transform.rotation;
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
            transform.rotation = Quaternion.Euler(0, 0, 0);

            if (mousePosition.y >= scaleThreshold)
            {
                transform.localScale = initialScale * scaleFactor;
            }
            else
            {
                transform.localScale = initialScale;
            }
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        if (mousePosition.y >= scaleThreshold)
        {
            isUse = true;
            StartCoroutine(UseCardAnimation());
        }
        else
        {
            transform.position = initialPosition;
            transform.localScale = initialScale;
            transform.rotation = initialRotation;
            StartCoroutine(Return_Position());
        }
    }

    IEnumerator UseCardAnimation()
    {
        float duration = 0.3f;
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        Vector3 targetScale = Vector3.zero;

        float time = 0;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, time / duration);
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // 카드 삭제 전에 중앙 텍스트 추가
        TextManager.Instance.AddText(cardText, Text_Prefab);
        Sound_Manager.Instance.PlaySoundEffect(0);
        Destroy(gameObject);
    }

    IEnumerator Return_Position()
    {
        yield return null;
        CardManager.Instance.RearrangeHand();
    }
}
