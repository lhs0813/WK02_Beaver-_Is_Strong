using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public List<Sprite> enemySprites; // 외부에서 설정할 스프라이트 리스트
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale; // 원래 크기 저장

    public Sprite Boss_Sprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트 가져오기
        originalScale = transform.localScale; // 시작할 때 원래 크기 저장
    }

    // 랜덤한 스프라이트를 적용하는 함수 (외부에서 호출 가능)
    public void SetRandomSprite()
    {
        if (enemySprites == null || enemySprites.Count == 0)
        {
            Debug.LogWarning("Enemy sprite list is empty or not assigned!");
            return;
        }

        if (GameManager.Instance.Stage_num % 7 == 0)
        {
            spriteRenderer.sprite = Boss_Sprite;
            GetComponent<Character_Animation>().initialScale = new Vector3(1, 1, 1);
            transform.localScale = new Vector3(1, 1, 1); // 7라운드에서는 크기를 (1,1,1)로 설정

            CardManager.Instance.cardPrefabs.Add(CardManager.Instance.All_Cards_Prefab[8]);

        }
        else
        {
            Sprite randomSprite = enemySprites[Random.Range(0, enemySprites.Count)];
            spriteRenderer.sprite = randomSprite;
            GetComponent<Character_Animation>().initialScale = originalScale;
           transform.localScale = originalScale; // 원래 크기로 되돌리기
        }
    }
}
