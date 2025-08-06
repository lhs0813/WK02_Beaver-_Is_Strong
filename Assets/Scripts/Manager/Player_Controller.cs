﻿using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{

    public static Player_Controller Instance { get; private set; }
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
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Btn_Manager.Instance.Hand_Clicked();
        }
        
        
    }
}
