using TMPro;
using UnityEngine;

public class STR_Screen : MonoBehaviour
{
    private int Str = 0;

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
        if (Str != Player_HP.GetComponent<Player_HP>().STR)
        {
            Str = Player_HP.GetComponent<Player_HP>().STR;
            GetComponent<TextMeshProUGUI>().text = "STR : " + Str.ToString();
        }
    }
}
