using TMPro;
using UnityEngine;
using System.Collections.Generic;


public class Rabbit_Is_Not_Sell : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void Rabbit_Say()
    {
        GetComponent<TextMeshProUGUI>().text = "I'm not selling.";

    }


    /*IEnumerator Return_Text()
    {
        yield return null;
    }*/
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
