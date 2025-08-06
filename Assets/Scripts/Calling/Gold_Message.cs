using TMPro;
using UnityEngine;

public class Gold_Message : MonoBehaviour
{
    private int Gold = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Gold != GameManager.Instance.Gold)
        {
            Gold = GameManager.Instance.Gold;
            GetComponent<TextMeshProUGUI>().text = "Gold : " + Gold.ToString();
        }
    }
}
