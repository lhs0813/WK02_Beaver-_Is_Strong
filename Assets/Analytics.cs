using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }
    private bool _isInitialized = false;
    private void Awake()
    {
        // 싱글톤 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        _isInitialized = true;
    }

    public void BuyShieldCard(bool BuyShield)
    {
        if (!_isInitialized)
            return;


        CustomEvent buyShield = new CustomEvent("TestEvent")
        {
            {"Buy_Shield", BuyShield}
        };

        AnalyticsService.Instance.RecordEvent(buyShield);
        AnalyticsService.Instance.Flush();

        Debug.Log("쉴드카드 구매!");

    }
}
