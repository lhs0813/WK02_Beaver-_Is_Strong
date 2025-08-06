using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Shop_Screen : MonoBehaviour
{
    public static Shop_Screen Instance { get; private set; }
    public List<GameObject> cardPrefabs = new List<GameObject>(); // 구매된 카드 리스트
    public List<GameObject> All_Cards_Prefab; // 모든 카드 프리팹 리스트

    [SerializeField]
    public GameObject Player_HP;



    public int STR_Value = 1;
    public int HP_Value = 1;
    public int Guard_Value = 1;

    private void Awake()
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

        Player_HP = FindAnyObjectByType<Player_HP>().gameObject;
    }
    private void Start()
    {
        // Card_Manager 싱글톤에서 All_Cards_Prefab 가져오기
        if (CardManager.Instance != null)
        {
            All_Cards_Prefab = CardManager.Instance.All_Cards_Prefab;
        }
        else
        {
            Debug.LogError("Card_Manager instance not found!");
        }
    }

    

    public void Purchase_Strong()
    {
        Sound_Manager.Instance.PlaySoundEffect(3);
        if (GameManager.Instance.Gold >= 200)
        {
            GameManager.Instance.Gold -= 200;
            if (All_Cards_Prefab != null && All_Cards_Prefab.Count > 4)
            {
                CardManager.Instance.cardPrefabs.Add(All_Cards_Prefab[3]); // 4번째(인덱스 3) 프리팹 추가
                Debug.Log("Purchased Strong card added!");
                DeleteClickedButton();
            }
            else
            {
                Debug.LogError("All_Cards_Prefab list is not properly initialized or does not contain enough elements.");
            }
        }
        else
        {
            Debug.Log("돈이 부족해 ㅠㅠ");
        }

    }

    public void Purchase_Guard()
    {
        Sound_Manager.Instance.PlaySoundEffect(3);
        if (GameManager.Instance.Gold >= 200)
        {
            AnalyticsManager.Instance.BuyShieldCard(true);
            GameManager.Instance.Gold -= 200;
            if (All_Cards_Prefab != null && All_Cards_Prefab.Count > 5)
            {
                CardManager.Instance.cardPrefabs.Add(All_Cards_Prefab[4]); // 4번째(인덱스 3) 프리팹 추가
                Debug.Log("Purchased Guard Card added!");
                DeleteClickedButton();
            }
            else
            {
                Debug.LogError("All_Cards_Prefab list is not properly initialized or does not contain enough elements.");
            }
        }
        else
        {
            Debug.Log("돈이 부족해 ㅠㅠ");
        }
    }


    public void Purchase_Very()
    {
        Sound_Manager.Instance.PlaySoundEffect(3);
        if (GameManager.Instance.Gold >= 500)
        {
            GameManager.Instance.Gold -= 500;
            if (All_Cards_Prefab != null && All_Cards_Prefab.Count > 5)
            {
                CardManager.Instance.cardPrefabs.Add(All_Cards_Prefab[5]); // 4번째(인덱스 3) 프리팹 추가
                Debug.Log("Purchased Strong card added!");
                DeleteClickedButton();
            }
            else
            {
                Debug.LogError("All_Cards_Prefab list is not properly initialized or does not contain enough elements.");
            }
        }
        else
        {
            Debug.Log("돈이 부족해 ㅠㅠ");
        }
    }

    public void Purchase_And()
    {
        if (GameManager.Instance.Gold >= 1000)
        {
            Sound_Manager.Instance.PlaySoundEffect(3);
            GameManager.Instance.Gold -= 1000;
            if (All_Cards_Prefab != null && All_Cards_Prefab.Count > 5)
            {
                CardManager.Instance.cardPrefabs.Add(All_Cards_Prefab[6]); // 4번째(인덱스 3) 프리팹 추가
                Debug.Log("Purchased Strong card added!");
                DeleteClickedButton();
            }
            else
            {
                Debug.LogError("All_Cards_Prefab list is not properly initialized or does not contain enough elements.");
            }
        }
        else
        {
            Debug.Log("돈이 부족해 ㅠㅠ");
        }
    }

    public void Purchase_Push()
    {
        if (GameManager.Instance.Gold >= 300)
        {
            Sound_Manager.Instance.PlaySoundEffect(3);
            GameManager.Instance.Gold -= 300;
            if (All_Cards_Prefab != null && All_Cards_Prefab.Count > 5)
            {
                CardManager.Instance.cardPrefabs.Add(All_Cards_Prefab[7]); // 4번째(인덱스 3) 프리팹 추가
                Debug.Log("Purchased Guard Card added!");
                DeleteClickedButton();
            }
            else
            {
                Debug.LogError("All_Cards_Prefab list is not properly initialized or does not contain enough elements.");
            }
        }
        else
        {
            Debug.Log("돈이 부족해 ㅠㅠ");
        }
    }

    public void Stats_Str()
    {
        if (GameManager.Instance.Gold - STR_Value * 10 >= 0)
        {
            Sound_Manager.Instance.PlaySoundEffect(3);
            Player_HP.GetComponent<Player_HP>().STR += 1;
            GameManager.Instance.Gold -= STR_Value * 10;
            STR_Value += 1;
        }
        else
        {
            Debug.Log("돈이 부족해 ㅠㅠ");
        }
    }

    public void Stats_Guard()
    {
        if (GameManager.Instance.Gold - Guard_Value * 100 >= 0)
        {
            Sound_Manager.Instance.PlaySoundEffect(3);
            GameManager.Instance.Gold -= Guard_Value * 100;
            STR_Value += 1;
        }
        else
        {
            Debug.Log("돈이 부족해 ㅠㅠ");
        }

        
    }

    public void Stats_HP()
    {

        if (GameManager.Instance.Gold - HP_Value * 10 >= 0)
        {
            Sound_Manager.Instance.PlaySoundEffect(3);
            Player_HP.GetComponent<Slider>().maxValue += 10;
            Player_HP.GetComponent<Player_HP>().HP += 10;
            GameManager.Instance.Gold -= HP_Value * 10;
            HP_Value += 1;
        }
        

        
    }

    private void DeleteClickedButton()
    {
        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (clickedButton != null)
        {
            Destroy(clickedButton);
            Debug.Log("Button deleted: " + clickedButton.name);
        }
        else
        {
            Debug.LogError("No button detected.");
        }
    }
}
