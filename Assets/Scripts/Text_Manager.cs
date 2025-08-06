using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;


public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; }

    [SerializeField]
    public GameObject Text_Line;

    [SerializeField]
    public GameObject Player;

    [SerializeField]
    public GameObject Player_HP;

    [SerializeField]
    public GameObject Enemy;

    [SerializeField]
    public GameObject Enemy_Hit_Effect;

    [SerializeField]
    public GameObject Guard_Effect;

    [SerializeField]
    public GameObject Guard_Up;

    [SerializeField]
    public GameObject Strong_Effect;
    //private GameObject textPrefab; // 프리팹을 Inspector에서 할당

    public int Guard_Value = 0;
    public int Strong_Value = 0;

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

        Text_Line = FindAnyObjectByType<Text_Line>().gameObject;
        Player = FindAnyObjectByType<Player>().gameObject;
        Player_HP = FindAnyObjectByType<Player_HP>().gameObject;
        Enemy = FindAnyObjectByType<Enemy_HP>().gameObject;
        Enemy_Hit_Effect = FindAnyObjectByType<Attack_Event_Enemy>().gameObject;
        Guard_Effect = FindAnyObjectByType<Guard_Text>().gameObject;
        Strong_Effect = FindAnyObjectByType<Strong_Text>().gameObject;
        Guard_Up = FindAnyObjectByType<SOLID_UI>().gameObject;

        Enemy_Hit_Effect.GetComponent<ParticleSystem>().Stop();
    }
    private void Start()
    {
        Guard_Effect.SetActive(false);
        Strong_Effect.SetActive(false);
    }

    public void AddText(string newText, GameObject text_Prefab)
    {
        if (Text_Line == null)
        {
            Debug.LogError("Text_Line 오브젝트가 설정되지 않았습니다!");
            return;
        }


        // 새 텍스트를 프리팹을 부모(Text_Line)의 자식으로 생성
        GameObject newTextObj = Instantiate(text_Prefab, Text_Line.transform);

        // 자식들의 위치를 확인하여 새 텍스트의 위치 계산
        RectTransform lastChildRect = null;
        foreach (Transform child in Text_Line.transform)
        {
            // 마지막으로 추가된 텍스트 객체의 RectTransform을 찾기
            if (child != newTextObj.transform)
            {
                lastChildRect = child.GetComponent<RectTransform>();
            }
        }

        // 마지막 자식이 있다면, 그 텍스트 오른쪽으로 새 텍스트 배치
        if (lastChildRect != null)
        {
            float lastChildWidth = lastChildRect.rect.width; // 마지막 텍스트의 너비
            newTextObj.transform.localPosition = new Vector3((lastChildRect.localPosition.x + (lastChildWidth + text_Prefab.GetComponent<RectTransform>().rect.width) / 100), 0, 0);
        }
        else
        {
            // 첫 번째 텍스트라면 (부모의 중앙)
            newTextObj.transform.localPosition = new Vector3(-0.3f, 0, 0);
        }
    }

    public void Text_Decition()
    {
        if (Text_Line == null)
        {
            Debug.LogError("Text_Line 오브젝트가 설정되지 않았습니다!");
            return;
        }

        // ✅ 기대하는 카드 조합을 딕셔너리로 설정
        Dictionary<string[], System.Action> cardCombinations = new Dictionary<string[], System.Action>()
        {
            //노말 구문
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Attack_text(Clone)" }, () => ExecuteFunction1() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Guard_Text(Clone)" }, () => ExecuteFunction2() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Strong_Text(Clone)" }, () => ExecuteFunction3() },
            // Very구문
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Strong_Text(Clone)" }, () => ExecuteFunction4() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Guard_Text(Clone)" }, () => ExecuteFunction5() },
            
            //And 구문
            //Beaver is Attack 파생기
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Attack_text(Clone)", "And_Text(Clone)", "Attack_text(Clone)" }, () => ExecuteFunction6() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Attack_text(Clone)", "And_Text(Clone)", "Strong_Text(Clone)" }, () => ExecuteFunction7() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Attack_text(Clone)", "And_Text(Clone)", "Guard_Text(Clone)" }, () => ExecuteFunction8() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Attack_text(Clone)", "And_Text(Clone)", "Push_Text(Clone)" }, () => ExecuteFunction9() },

            //Beaver is Strong 파생기
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Strong_Text(Clone)", "And_Text(Clone)", "Strong_Text(Clone)" }, () => ExecuteFunction10() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Strong_Text(Clone)", "And_Text(Clone)", "Attack_text(Clone)" }, () => ExecuteFunction11() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Strong_Text(Clone)", "And_Text(Clone)", "Guard_Text(Clone)" }, () => ExecuteFunction12() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Strong_Text(Clone)", "And_Text(Clone)", "Push_Text(Clone)" }, () => ExecuteFunction13() }, 

            //Beaver is Solid(Guard) 파생기
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Guard_Text(Clone)", "And_Text(Clone)", "Guard_Text(Clone)" }, () => ExecuteFunction14() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Guard_Text(Clone)", "And_Text(Clone)", "Attack_text(Clone)" }, () => ExecuteFunction15() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Guard_Text(Clone)", "And_Text(Clone)", "Strong_Text(Clone)" }, () => ExecuteFunction16() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Guard_Text(Clone)", "And_Text(Clone)", "Push_Text(Clone)" }, () => ExecuteFunction17() },
            
            //Beaver is Push 파생기
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Push_Text(Clone)" }, () => ExecuteFunction18() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Push_Text(Clone)", "And_Text(Clone)", "Push_Text(Clone)" }, () => ExecuteFunction19() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Push_Text(Clone)", "And_Text(Clone)", "Attack_text(Clone)" }, () => ExecuteFunction20() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Push_Text(Clone)", "And_Text(Clone)", "Strong_Text(Clone)" }, () => ExecuteFunction21() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Push_Text(Clone)", "And_Text(Clone)", "Guard_Text(Clone)" }, () => ExecuteFunction22() },

            //Beaver is very very 파생
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Very_Text(Clone)", "Strong_Text(Clone)" }, () => ExecuteFunction23() }, //very very strong
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Very_Text(Clone)", "Very_Text(Clone)", "Strong_Text(Clone)" }, () => ExecuteFunction24() }, //very very very strong

            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Very_Text(Clone)", "Guard_Text(Clone)" }, () => ExecuteFunction25() }, // very very Solid
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Very_Text(Clone)", "Very_Text(Clone)", "Guard_Text(Clone)" }, () => ExecuteFunction26() }, // very very very Solid

            // Beaver is very Strong 파생
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Strong_Text(Clone)", "And_Text(Clone)", "Strong_Text(Clone)" }, () => ExecuteFunction27() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Strong_Text(Clone)", "And_Text(Clone)", "Attack_text(Clone)" }, () => ExecuteFunction28() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Strong_Text(Clone)", "And_Text(Clone)", "Guard_Text(Clone)" }, () => ExecuteFunction29() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Strong_Text(Clone)", "And_Text(Clone)", "Push_Text(Clone)" }, () => ExecuteFunction30() },

            // Beaver is very Solid(Guard) 파생
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Guard_Text(Clone)", "And_Text(Clone)", "Guard_Text(Clone)" }, () => ExecuteFunction31() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Guard_Text(Clone)", "And_Text(Clone)", "Attack_text(Clone)" }, () => ExecuteFunction32() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Guard_Text(Clone)", "And_Text(Clone)", "Strong_Text(Clone)" }, () => ExecuteFunction33() },
            { new string[] { "Beaver_Text(Clone)", "Copula_IS_Text(Clone)", "Very_Text(Clone)", "Guard_Text(Clone)", "And_Text(Clone)", "Push_Text(Clone)" }, () => ExecuteFunction34() },


        };

        // 현재 하위 오브젝트들의 이름을 리스트로 저장
        List<string> childNames = new List<string>();
        foreach (Transform child in Text_Line.transform)
        {
            childNames.Add(child.gameObject.name);
        }

        // ✅ 등록된 조합 중에서 일치하는 것이 있는지 확인
        foreach (var entry in cardCombinations)
        {
            if (CheckOrderMatch(entry.Key, childNames))
            {
                entry.Value.Invoke(); // 해당 조합에 맞는 함수 실행
                return;
            }
        }

        Debug.Log("일치하는 카드 조합이 없음.");
    }


    private bool CheckOrderMatch(string[] expectedOrder, List<string> actualOrder)
    {
        if (expectedOrder.Length != actualOrder.Count)
            return false;

        for (int i = 0; i < expectedOrder.Length; i++)
        {
            if (expectedOrder[i] != actualOrder[i])
                return false;
        }
        Sound_Manager.Instance.PlaySoundEffect(2);
        return true;
    }

    IEnumerator Hit_Enemy(float seconds)
    {
        Enemy_Hit_Effect.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(seconds);
        Enemy.GetComponent<Enemy_HP>().HP -= 10 + Player_HP.GetComponent<Player_HP>().STR;
        Enemy_Hit_Effect.GetComponent<ParticleSystem>().Stop();
    }

    IEnumerator Push_Enemy(float seconds)
    {
        Enemy_Hit_Effect.GetComponent<ParticleSystem>().Play();
        ParticleSystem.MainModule mainModule = Enemy_Hit_Effect.GetComponent<ParticleSystem>().main;
        mainModule.startColor = Color.blue;
        yield return new WaitForSeconds(seconds);

        if (Guard_Value == 0)
        {
            Enemy.GetComponent<Enemy_HP>().HP -= (Guard_Value + Guard_Up.GetComponent<SOLID_UI>().Guard_Plus) * 4;
        }
        else
        {
            Enemy.GetComponent<Enemy_HP>().HP -= (Guard_Value) * 4;
        }



        Enemy_Hit_Effect.GetComponent<ParticleSystem>().Stop();
        mainModule.startColor = Color.red;
    }

    IEnumerator Guard(float seconds)
    {

        Guard_Value = Guard_Value + 5 + Guard_Up.GetComponent<SOLID_UI>().Guard_Plus;

        GameObject Guard_Text = Guard_Effect.transform.GetChild(0).gameObject;

        TextMeshPro textComponent = Guard_Text.GetComponent<TextMeshPro>();
        if (textComponent == null)
        {
            Debug.LogError("첫 번째 자식 오브젝트에 TextMeshPro 컴포넌트가 없습니다!");
            yield break;
        }

        Guard_Effect.SetActive(true);
        textComponent.text = Guard_Value.ToString();
        yield return new WaitForSeconds(seconds);

        // 5️⃣ 텍스트 업데이트

    }

    IEnumerator Strong(float seconds)
    {
        Strong_Value = 0;
        Strong_Value += 3;

        GameObject Strong_Text = Strong_Effect.transform.GetChild(0).gameObject;

        TextMeshPro textComponent = Strong_Text.GetComponent<TextMeshPro>();
        if (textComponent == null)
        {
            Debug.LogError("첫 번째 자식 오브젝트에 TextMeshPro 컴포넌트가 없습니다!");
            yield break;
        }

        Strong_Effect.SetActive(true);
        textComponent.text = "+" + Strong_Value;
        Player_HP.GetComponent<Player_HP>().STR += Strong_Value;
        yield return new WaitForSeconds(seconds);

        // 5️⃣ 텍스트 업데이트

    }

    IEnumerator Delay_Hit_Enemy(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Hit_Enemy(seconds));

    }
    IEnumerator Delay_Push_Enemy(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Push_Enemy(seconds));

    }


    IEnumerator Delay_Guard(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Guard(seconds));

    }

    IEnumerator Delay_Strong(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Strong(seconds));

    }

    // ✅ 카드 조합별 실행할 함수들
    private void ExecuteFunction1()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();

        StartCoroutine(Hit_Enemy(0.2f));
        Debug.Log("비버 이즈 어택!!");
        // 여기에 원하는 동작 추가
    }



    private void ExecuteFunction2()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Guard(0.2f));
        // 여기에 원하는 동작 추가
    }

    private void ExecuteFunction3()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Strong(0.2f));
        // 여기에 원하는 동작 추가
    }

    private void ExecuteFunction4()
    {
        // Very_Strong

        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Strong(0.2f));
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Strong(0.2f));

        // 여기에 원하는 동작 추가
    }

    private void ExecuteFunction5()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Guard(0.2f));
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Guard(0.2f));

        // 여기에 원하는 동작 추가
    }

    private void ExecuteFunction6()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Hit_Enemy(0.2f));

        StartCoroutine(Delay_Hit_Enemy(0.2f));



        // 여기에 원하는 동작 추가
    }

    private void ExecuteFunction7()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Hit_Enemy(0.2f));

        StartCoroutine(Delay_Strong(0.2f));
        // 여기에 원하는 동작 추가
    }

    private void ExecuteFunction8()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Strong(0.2f));

        StartCoroutine(Delay_Guard(0.2f));
        // 여기에 원하는 동작 추가
    }
    private void ExecuteFunction9()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Hit_Enemy(0.2f));

        StartCoroutine(Delay_Push_Enemy(0.2f));
        // 여기에 원하는 동작 추가
    }

    private void ExecuteFunction10()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Strong(0.2f));

        StartCoroutine(Delay_Strong(0.2f));
    }

    private void ExecuteFunction11()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Strong(0.2f));

        StartCoroutine(Delay_Hit_Enemy(0.2f));
    }

    private void ExecuteFunction12()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Strong(0.2f));

        StartCoroutine(Delay_Guard(0.2f));
    }

    private void ExecuteFunction13()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Strong(0.2f));

        StartCoroutine(Delay_Push_Enemy(0.2f));
    }

    private void ExecuteFunction14()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Guard(0.2f));

        StartCoroutine(Delay_Guard(0.2f));
    }

    private void ExecuteFunction15()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Guard(0.2f));

        StartCoroutine(Delay_Hit_Enemy(0.2f));
    }
    private void ExecuteFunction16()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Guard(0.2f));

        StartCoroutine(Delay_Strong(0.2f));
    }

    private void ExecuteFunction17()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Guard(0.2f));

        StartCoroutine(Delay_Push_Enemy(0.2f));
    }

    private void ExecuteFunction18()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Push_Enemy(0.2f));
    }

    private void ExecuteFunction19()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Push_Enemy(0.2f));

        StartCoroutine(Delay_Push_Enemy(0.2f));
    }

    private void ExecuteFunction20()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Push_Enemy(0.2f));

        StartCoroutine(Delay_Hit_Enemy(0.2f));
    }

    private void ExecuteFunction21()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Push_Enemy(0.2f));

        StartCoroutine(Delay_Strong(0.2f));
    }

    private void ExecuteFunction22()
    {
        Player.GetComponent<Character_Animation>().Spin_Attack();
        StartCoroutine(Push_Enemy(0.2f));

        StartCoroutine(Delay_Guard(0.2f));
    }

    private void ExecuteFunction23()
    {
        ExecuteFunction4();
        ExecuteFunction4();
    }

    private void ExecuteFunction24()
    {
        ExecuteFunction4();
        ExecuteFunction4();
        ExecuteFunction4();
    }
    private void ExecuteFunction25()
    {
        ExecuteFunction5();
        ExecuteFunction5();
    }

    private void ExecuteFunction26()
    {
        ExecuteFunction5();
        ExecuteFunction5();
        ExecuteFunction5();
    }

    private void ExecuteFunction27()
    {
        ExecuteFunction4();
        ExecuteFunction3();
    }

    private void ExecuteFunction28()
    {
        ExecuteFunction4();
        ExecuteFunction1();
    }

    private void ExecuteFunction29()
    {
        ExecuteFunction4();
        ExecuteFunction2();
    }

    private void ExecuteFunction30()
    {
        ExecuteFunction4();
        ExecuteFunction18();
    }

    private void ExecuteFunction31()
    {
        ExecuteFunction5();
        ExecuteFunction2();
    }

    private void ExecuteFunction32()
    {
        ExecuteFunction5();
        ExecuteFunction1();
    }

    private void ExecuteFunction33()
    {
        ExecuteFunction5();
        ExecuteFunction3();
    }

    private void ExecuteFunction34()
    {
        ExecuteFunction5();
        ExecuteFunction18();
    }
}
