using System.Threading;
using TMPro;
using UnityEngine;

public class Shop_STR : MonoBehaviour
{
    private int Event = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Event = Shop_Screen.Instance.STR_Value;
    }

    // Update is called once per frame
    void Update()
    {
        if(Event != Shop_Screen.Instance.STR_Value)
        GetComponent<TextMeshProUGUI>().text = (Shop_Screen.Instance.STR_Value * 10).ToString() + "G";

    }
}
