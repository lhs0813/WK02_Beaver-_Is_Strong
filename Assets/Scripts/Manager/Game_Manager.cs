using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject Text_Line;

    [SerializeField]
    public GameObject Player;

    [SerializeField]
    public GameObject Player_HP;

    [SerializeField]
    public GameObject Enemy;

    [SerializeField]
    public GameObject Enemy_HP;

    [SerializeField]
    public GameObject Player_Hit_Effect;

    [SerializeField]
    public GameObject Turn_Message;

    [SerializeField]
    public GameObject Game_Over_Screen;

    [SerializeField]
    public GameObject Choose_Screen;

    [SerializeField]
    public GameObject Shop_Screen;

    
    public static GameManager Instance { get; private set; }

    private Vector3 Original_Scale;

    private enum GameState { Draw_Turn, PlayerTurn, EnemyTurn, GameOver }
    private GameState currentState;

    public int Stage_num = 1;
    public float Stage_To_Hp;
    public int Stage_To_Str;
    public int Gold = 0;

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
        Enemy = FindAnyObjectByType<Enemy>().gameObject;
        Enemy_HP = FindAnyObjectByType<Enemy_HP>().gameObject;

        Player = FindAnyObjectByType<Player>().gameObject;
        Player_HP = FindAnyObjectByType<Player_HP>().gameObject;


        Player_Hit_Effect = FindAnyObjectByType<Attack_Event_Player>().gameObject;
        

        Player_Hit_Effect.GetComponent<ParticleSystem>().Stop();

        Turn_Message = FindAnyObjectByType<Turn_Message>().gameObject;

        Game_Over_Screen = FindAnyObjectByType<Lost_Screen>().gameObject;

        Choose_Screen = FindAnyObjectByType<Choose_Screen>().gameObject;

        Shop_Screen = FindAnyObjectByType<Shop_Screen>().gameObject;

        Original_Scale = Text_Line.GetComponent<RectTransform>().localScale;


        Sound_Manager.Instance.PlayMusic(0);
        Debug.Log(Original_Scale);
    }

    void Start()
    {
        Game_Over_Screen.SetActive(false);
        Choose_Screen.SetActive(false);
        Shop_Screen.SetActive(false);

        Draw_Turn();

        Stage_To_Hp = Enemy_HP.GetComponent<Enemy_HP>().HP;
    }

    void Draw_Turn()
    {
        TextManager.Instance.Guard_Value = 0;
        TextManager.Instance.Strong_Value = 0;
        currentState = GameState.Draw_Turn;
        CardManager.Instance.DrawCards(7);


        if (Stage_num % 7 == 0)
        {
            // 현재 재생 중인 음악이 music1이면 실행하지 않음
            if (Sound_Manager.Instance.musicSource.clip != Sound_Manager.Instance.musicClips[1])
            {
                Sound_Manager.Instance.StopMusic();
                Sound_Manager.Instance.PlayMusic(1); // 1번 트랙을 재생
            }
        }

        //Draw_Turn_end();
    }

    public void Draw_Turn_end()
    {
        Turn_Message.GetComponent<Effect_Manager>().FadeIn();
        
        // 페이드 인이 끝난 후, 1초 후 페이드 아웃을 호출하도록 설정
        StartCoroutine(WaitAndFadeOut());

    }

    private IEnumerator WaitAndFadeOut()
    {
        // 1초 기다린 후 페이드 아웃 실행
        yield return new WaitForSeconds(0.5f);  // FadeIn 시간 동안 기다림
        Turn_Message.GetComponent<Effect_Manager>().FadeOut();
    }

    void StartGame()
    {
        currentState = GameState.PlayerTurn;
        Debug.Log("Game Started - Player Turn");
    }

    public void EndPlayerTurn()
    {
        if (currentState != GameState.PlayerTurn) return;

        currentState = GameState.EnemyTurn;
        Debug.Log("Player Turn Ended - Enemy Turn");

        Invoke("EnemyTurn", 1f); // Simulating enemy turn delay
    }

    public void Text_Cal_Turn()
    {
        
        StartCoroutine(AnimateTextLineAndRemoveChildren());
        TextManager.Instance.Text_Decition();

        Turn_Off_Delay();
    }

    void Turn_Off_Delay()
    {
        StartCoroutine(Delay(1.0f));
        
    }

    void EnemyTurn()
    {
        TextManager.Instance.Strong_Effect.SetActive(false);

        if(Enemy_HP.GetComponent<Enemy_HP>().HP <= 0)
        {
            Kill_Enemy();
            return;
        }
        Debug.Log("Enemy is making a move...");
        StartCoroutine(Hit_Player(0.2f));
        // TODO: Implement enemy actions here
        EndEnemyTurn();
    }

    void EndEnemyTurn()
    {
        currentState = GameState.Draw_Turn;
        Draw_Turn();
        Debug.Log("Enemy Turn Ended - Draw_Turn");
    }

    public void GameOver()
    {
        CardManager.Instance.ClearHand();
        Game_Over_Screen.SetActive(true);
        

        //GameOverUI.SetActive(true);

        currentState = GameState.GameOver;
        StartCoroutine(Reload_Scene());
        Debug.Log("Game Over");
    }

    IEnumerator Reload_Scene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(0);
    }

    
    public void Kill_Enemy()
    {
        if(Stage_num % 7 == 0)
        {
            Sound_Manager.Instance.StopMusic();
            Sound_Manager.Instance.PlayMusic(0);
            CardManager.Instance.cardPrefabs.RemoveAt(CardManager.Instance.cardPrefabs.Count - 1);
        }
        

        Stage_num++;
        Stage_To_Hp += Random.Range(1,21);
        Stage_To_Str += Random.Range(1, 3);


        Gold += 100;
        CardManager.Instance.ClearHand();
        Enemy.GetComponent<Effect_Manager>().FadeOut();
        Choose_Screen.SetActive(true);
        Debug.Log("Kill_Enemy");
        TextManager.Instance.Guard_Effect.SetActive(false);
    }

    private void Next_Enemy_Spawn(float New_HP,int Stage_Number, int STR)
    {
        //Enemy.GetComponent<SpriteRenderer>().sprite = list뭐시기
        Enemy_HP.GetComponent<Slider>().maxValue = New_HP;
        Enemy_HP.GetComponent<Enemy_HP>().HP = New_HP;
        Enemy_HP.GetComponent<Enemy_HP>().STR = STR;

        Enemy.GetComponent<Enemy>().SetRandomSprite();
        Enemy.GetComponent<Effect_Manager>().FadeIn();
        

        Game_Over_Screen.SetActive(false);
        Choose_Screen.SetActive(false);
        Draw_Turn();

    }

    public void Fight_to_Continue()
    {
        Sound_Manager.Instance.PlaySoundEffect(1);
        Shop_Screen.SetActive(false);
        Next_Enemy_Spawn(Stage_To_Hp, Stage_num, Stage_To_Str);
    }

    public void Gold_Twice()
    {
        if (Gold * 2 > 9999)
        {
            Gold = 10000;
        }
        else
        {
            Gold *= 2;
        }

    }

    public void Rest_to_Continue()
    {
        Sound_Manager.Instance.PlaySoundEffect(1);
        int Rest_HP = (int)(Player_HP.GetComponent<Slider>().maxValue * 0.4f);

        Player_HP.GetComponent<Player_HP>().HP += Rest_HP;
        Fight_to_Continue();
    }

    public void Shop_to_Continue()
    {
        Sound_Manager.Instance.PlaySoundEffect(1);
        Shop_Screen.SetActive(true);
        //Shop_Screen.GetComponent<Effect_Manager>().FadeIn();
    }
    


    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        EnemyTurn();
    }

    IEnumerator Hit_Player(float seconds)
    {
        Sound_Manager.Instance.PlaySoundEffect(2);
        Enemy.GetComponent<Character_Animation>().Spin_Attack();

        Player_HP.GetComponent<Player_HP>().HP -= Enemy_HP.GetComponent<Enemy_HP>().STR + Enemy_HP.GetComponent<Enemy_HP>().STR - TextManager.Instance.Guard_Value;

        Player_Hit_Effect.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(seconds);
        Player_Hit_Effect.GetComponent<ParticleSystem>().Stop();

        TextManager.Instance.Guard_Effect.SetActive(false);

    }

    private IEnumerator AnimateTextLineAndRemoveChildren()
    {
        // 부모 오브젝트 크기 변화 (1초 동안 작아지기)
        Vector3 originalScale = Text_Line.transform.localScale;
        Vector3 targetScale = Vector3.zero;  // 0으로 작아지도록
        float animationTime = 0.3f;  // 애니메이션 시간

        float elapsedTime = 0f;
        while (elapsedTime < animationTime)
        {
            elapsedTime += Time.deltaTime;
            Text_Line.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / animationTime);
            yield return null;
        }

        // 하위 오브젝트 모두 제거
        foreach (Transform child in Text_Line.transform)
        {
            Destroy(child.gameObject);
        }

        // 크기 복원
        Text_Line.transform.localScale = Original_Scale;
    }


}
