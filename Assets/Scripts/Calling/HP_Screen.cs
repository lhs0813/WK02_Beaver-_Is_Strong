using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HP_Screen : MonoBehaviour
{

    private int MAX_Hp = 0;
    private int Hp = 0;

    [SerializeField]
    public GameObject Player_HP;

    private void Awake()
    {
        Player_HP = FindAnyObjectByType<Player_HP>().gameObject;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MAX_Hp != Player_HP.GetComponent<Slider>().maxValue || Hp != Player_HP.GetComponent<Player_HP>().HP)
        {
            MAX_Hp = (int)Player_HP.GetComponent<Slider>().maxValue;
            Hp = (int)Player_HP.GetComponent<Player_HP>().HP;
            GetComponent<TextMeshProUGUI>().text = "HP : " + Hp.ToString() + " / " + MAX_Hp.ToString();
        }

        

    }
}
