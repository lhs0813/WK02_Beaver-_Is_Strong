using UnityEngine;
using System.Collections;
using TMPro;

public class Effect_Manager : MonoBehaviour
{
    private TextMeshProUGUI textMesh; // UI용 TextMeshPro
    private SpriteRenderer imageRen; // SpriteRenderer를 위한 변수
    private float fadeDuration = 0.5f; // 페이드 지속 시간
    private Color originalColor;

    void Awake()
    {
        // TextMeshProUGUI 컴포넌트 초기화
        textMesh = GetComponent<TextMeshProUGUI>();

        // SpriteRenderer 컴포넌트 초기화
        imageRen = GetComponent<SpriteRenderer>();

        if (textMesh != null)
        {
            originalColor = textMesh.color;
            // 초기 투명도를 0으로 설정 (보이지 않음)
            originalColor.a = 0f;
            textMesh.color = originalColor;
        }

        if (imageRen != null)
        {
            originalColor = imageRen.color;
            // 초기 투명도를 0으로 설정 (보이지 않음)
            originalColor.a = 1f;
            imageRen.color = originalColor;
        }
    }

    public void FadeOut()
    {
        if (textMesh != null)
            StartCoroutine(FadeToAlpha(0f)); // TextMeshPro 완전히 투명해짐

        if (imageRen != null)
            StartCoroutine(FadeToAlpha2(0f)); // SpriteRenderer 완전히 투명해짐
    }

    public void FadeIn()
    {
        if (textMesh != null)
            StartCoroutine(FadeToAlpha(1f)); // TextMeshPro 완전 불투명 (1)

        if (imageRen != null)
            StartCoroutine(FadeToAlpha2(1f)); // SpriteRenderer 완전 불투명 (1)
    }

    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        float elapsedTime = 0f;
        Color startColor = textMesh.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startColor.a, targetAlpha, elapsedTime / fadeDuration);

            // TextMeshPro의 투명도 조절
            textMesh.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            yield return null;
        }

        // 최종 색상 고정
        textMesh.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
    }

    private IEnumerator FadeToAlpha2(float targetAlpha)
    {
        float elapsedTime = 0f;
        Color startColor = imageRen.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startColor.a, targetAlpha, elapsedTime / fadeDuration);

            // SpriteRenderer의 투명도 조절
            imageRen.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            yield return null;
        }

        // 최종 색상 고정
        imageRen.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
    }
}
