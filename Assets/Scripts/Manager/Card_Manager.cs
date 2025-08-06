using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private Transform handTransform; // 핸드 위치
    [SerializeField]
    private Transform deckTransform; // 덱 위치

    

    public static CardManager Instance { get; private set; }
    public List<GameObject> cardPrefabs; // 카드 프리팹 리스트
    public List<GameObject> All_Cards_Prefab; // 카드 프리팹 리스트
    private List<GameObject> currentHand = new List<GameObject>();

    public float spacing = 1.0f; // 카드 간격
    public float angleStep = -10f; // 카드 회전량
    public float flyDuration = 0.5f; // 날아오는 시간
    public float startScale = 0.1f; // 카드 시작 크기
    public float endScale = 1.3f; // 카드 최종 크기
    public float startRotationRange = 30f; // 시작 회전 각도 범위

    private int lastChildCount = 0; // 핸드의 자식 개수 추적

    private bool Draw_End = false;

    private int Card_Priority = 0;

    void Awake()
    {
        handTransform = FindAnyObjectByType<Hand>().transform;
        deckTransform = FindAnyObjectByType<Deck>().transform;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        // handTransform의 자식 개수가 변하면 재정렬
        if (handTransform.childCount != lastChildCount && Draw_End)
        {
            lastChildCount = handTransform.childCount;
            RearrangeHand();
        }
    }

    



    public void DrawCards(int count)
    {
        Draw_End = false;
        ClearHand(); // 기존 핸드 삭제
        StartCoroutine(DrawCardsCoroutine(count));
    }

    IEnumerator DrawCardsCoroutine(int count)
    {
        if (cardPrefabs.Count < 2)
        {
            Debug.LogError("cardPrefabs 리스트에 최소 2개의 카드가 필요합니다.");
            yield break;
        }

        // 첫 번째 카드 한 장 추가
        GameObject firstCard = Instantiate(cardPrefabs[0], deckTransform.position, Quaternion.identity);
        firstCard.transform.SetParent(handTransform);
        firstCard.transform.localScale = Vector3.one * startScale; // 작은 크기로 시작
        currentHand.Add(firstCard);
        yield return StartCoroutine(MoveCardToHand(firstCard, 0, count));
        Sound_Manager.Instance.PlaySoundEffect(1);
        // 두 번째 카드 한 장 추가
        GameObject secondCard = Instantiate(cardPrefabs[1], deckTransform.position, Quaternion.identity);
        secondCard.transform.SetParent(handTransform);
        secondCard.transform.localScale = Vector3.one * startScale; // 작은 크기로 시작
        currentHand.Add(secondCard);
        yield return StartCoroutine(MoveCardToHand(secondCard, 1, count));
        Sound_Manager.Instance.PlaySoundEffect(1);
        // 나머지 카드는 랜덤 추가 (이미 두 장을 추가했으므로 count - 2)
        for (int i = 2; i < count; i++)
        {
            Sound_Manager.Instance.PlaySoundEffect(1);
            int randomIndex = Random.Range(2, cardPrefabs.Count); // 2번 인덱스부터 랜덤 선택
            GameObject card = Instantiate(cardPrefabs[randomIndex], deckTransform.position, Quaternion.identity);
            card.transform.SetParent(handTransform);
            card.transform.localScale = Vector3.one * startScale; // 작은 크기로 시작
            currentHand.Add(card);

            yield return StartCoroutine(MoveCardToHand(card, i, count));
        }

        // 모든 카드의 콜라이더 활성화
        for (int i = 0; i < currentHand.Count; i++)
        {
            currentHand[i].GetComponent<BoxCollider2D>().enabled = true;
        }

        Draw_End = true;
        GameManager.Instance.Draw_Turn_end();
        Btn_Manager.Instance.Hand_Clicked();
    }


    IEnumerator MoveCardToHand(GameObject card, int index, int total)
    {
        Vector3 startPos = deckTransform.position;
        Vector3 endPos = handTransform.position + new Vector3((index - (total - 1) / 2f) * spacing, 0, 0);
        Quaternion startRot = Quaternion.Euler(0, 0, Random.Range(-startRotationRange, startRotationRange));
        Quaternion endRot = Quaternion.Euler(0, 0, (index - (total - 1) / 2f) * angleStep);
        Card_Priority = 0;
        float elapsedTime = 0;
        while (elapsedTime < flyDuration)
        {
            float t = elapsedTime / flyDuration;
            endPos.z = Card_Priority;
            card.transform.position = Vector3.Lerp(startPos, endPos, t);
            card.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            card.transform.localScale = Vector3.Lerp(Vector3.one * startScale, Vector3.one * endScale, t);
            elapsedTime += Time.deltaTime;
            Card_Priority++;
            yield return null;
        }

        card.transform.position = endPos;
        card.transform.rotation = endRot;
        card.transform.localScale = Vector3.one * endScale;
    }

    public void ClearHand()
    {
        foreach (GameObject card in currentHand)
        {
            Destroy(card);
        }
        currentHand.Clear();
        Debug.Log("손패 버리기");
    }

    public void RearrangeHand()
    {
        // 현재 handTransform의 자식 오브젝트들을 다시 리스트에 저장
        currentHand.Clear();
        foreach (Transform child in handTransform)
        {
            currentHand.Add(child.gameObject);
        }

        int cardCount = currentHand.Count;
        if (cardCount == 0) return; // 카드가 없으면 정렬할 필요 없음

        // 카드 위치와 회전만 업데이트 (다시 날아오는 애니메이션 없음)
        for (int i = 0; i < cardCount; i++)
        {
            Vector3 newPosition = handTransform.position + new Vector3((i - (cardCount - 1) / 2f) * spacing, 0, i);
            Quaternion newRotation = Quaternion.Euler(0, 0, (i - (cardCount - 1) / 2f) * angleStep);

            currentHand[i].transform.position = newPosition;
            currentHand[i].transform.rotation = newRotation;
        }
    }

    
    





}
