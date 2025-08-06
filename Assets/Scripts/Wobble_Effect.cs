using UnityEngine;
using UnityEngine.UI;

public class Wobble_Effect : MonoBehaviour
{
    public Image image;
    public float speed = 1.0f;
    public float intensity = 0.01f;

    private Material mat;

    void Start()
    {
        if (image != null)
        {
            mat = new Material(image.material);  // 새로운 Material 생성
            image.material = mat;  // 기존 Image에 적용
        }
    }

    void Update()
    {
        if (mat != null)
        {
            float offsetX = Mathf.Sin(Time.time * speed) * intensity;
            float offsetY = Mathf.Cos(Time.time * speed * 0.8f) * intensity;
            mat.mainTextureOffset += new Vector2(offsetX, offsetY) * Time.deltaTime;
        }
    }
}
