using TMPro;
using UnityEngine;

public class Stage_UI : MonoBehaviour
{
    private int Stage = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Stage != GameManager.Instance.Stage_num)
        {
            Stage = GameManager.Instance.Stage_num;
            GetComponent<TextMeshProUGUI>().text = "Stage : " + Stage.ToString();
        }

        
        
    }
}
