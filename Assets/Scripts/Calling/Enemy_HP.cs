using TMPro;
using UnityEngine;
using UnityEngine.UI;  // ✅ UGUI의 Slider를 사용하기 위해 추가

public class Enemy_HP : MonoBehaviour
{
    public float HP = 100;
    public int STR = 0;
    private Slider hpSlider;  // ✅ UGUI용 Slider 변수 추가
    private TextMeshPro HP_TEXT;

    private bool Enemy_Die = false;

    void Start()
    {
        hpSlider = GetComponent<Slider>();  // ✅ Slider 컴포넌트 가져오기
        HP_TEXT = GetComponentInChildren<TextMeshPro>(); // 자식에서 TextMeshPro를 찾기

        if (HP_TEXT == null) // 오류 예방: TextMeshPro가 자식에서 발견되지 않으면 로그 출력
        {
            Debug.LogError("HP_TEXT가 자식 오브젝트에서 할당되지 않았습니다!");
        }
    }

    void Update()
    {
        hpSlider.value = HP;  // ✅ HP 값을 Slider에 적용
        if (HP_TEXT != null)
        {
            HP_TEXT.text = "HP : " + HP.ToString(); // ✅ HP 값을 TextMeshPro UI에 표시
        }

        /*if (HP <= 0 && Enemy_Die == false)
        {
            Enemy_Die = true;
            GameManager.Instance.Kill_Enemy();
        }*/
    }

    public void Purchase_HP()
    {
        hpSlider.maxValue += 10;
        HP += 10;
    }

    public void Purchase_STR()
    {
        STR += 1;
    }

}
