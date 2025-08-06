using UnityEngine;

public class Card_Hover_Effect : MonoBehaviour
{
    public Color hoverColor = new Color(0.7f, 0.7f, 0.7f);  // hover 시 카드 색상 (기본값은 약간 어두운 회색)
    private Color originalColor;  // 카드의 원래 색상

    private Renderer cardRenderer;  // 카드의 Renderer

    void Start()
    {
        // 카드의 Renderer 가져오기
        cardRenderer = GetComponent<Renderer>();

        // 원래 색상 저장
        originalColor = cardRenderer.material.color;
    }

    void OnMouseEnter()
    {
        // 마우스를 올렸을 때 색상을 변경
        if (Btn_Manager.Instance.Now_Position == false)
        {
            //Btn_Manager.Instance.Hand_Clicked();
        }

        // 카드 색상 어두운 색으로 변경
        cardRenderer.material.color = hoverColor;
    }


    void OnMouseExit()
    {
        // 마우스를 뗄 때 원래 색상으로 복원
        cardRenderer.material.color = originalColor;
    }
}
