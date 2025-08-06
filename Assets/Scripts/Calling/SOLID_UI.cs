using TMPro;
using UnityEngine;

public class SOLID_UI : MonoBehaviour
{
    public int Guard_Plus = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void Guard_UP()
    {
        if (GameManager.Instance.Gold - Shop_Screen.Instance.Guard_Value * 100 >= 0)
        {

            GameManager.Instance.Gold -= Shop_Screen.Instance.Guard_Value * 100;
            Shop_Screen.Instance.Guard_Value += 1;
            Guard_Plus += 1;
            GetComponent<TextMeshProUGUI>().text = "Solid :+" + Guard_Plus.ToString();
        }
        else
        {
            Debug.Log("돈이 부족해 ㅠㅠ");
        }
        
        // Update is called once per frame
    }

}
