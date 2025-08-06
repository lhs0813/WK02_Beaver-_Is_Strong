using UnityEngine;

public class Turn_Off_Btn : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;  // 이미지 색을 변경할 SpriteRenderer

    private Color originalColor;  // 원래 이미지 색

    private void Start()
    {
        // SpriteRenderer 컴포넌트를 가져옵니다.
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            // 원래 색 저장
            originalColor = spriteRenderer.color;
        }
    }

    public void TurnOFF_Click()
    {
        GameManager.Instance.Text_Cal_Turn();
    }

    // 마우스를 클릭하면 회색으로 바꿈
    private void OnMouseDown()
    {
        Debug.Log("턴종료 클릭");
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray;  // 회색으로 변경
        }
        GameManager.Instance.Text_Cal_Turn();
        
    }

    // 마우스를 떼면 원래 색으로 돌아옴
    private void OnMouseUp()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;  // 원래 색으로 돌아옴
        }
    }

    
}
