using UnityEngine;
using UnityEngine.UI;

// TODO: Make this a ScriptableObject?
public class Store : MonoBehaviour
{
    [Header("Store cost options")]
    public float baseStoreCost = 1.5f;
    public float increaseCostPercentage = 0.01f;

    public float automationCost = 50.0f;

    bool automated = false;

    public float storeIncome = 0.5f;

    [Header("Store income time options")]
    // TODO: Improve this with coroutines
    public float timer = 4f;
    float currentTimer = 0f;
    bool startTimer = false;

    int storeCount = 0;

    // TODO: Move this to UIManager
    [Header("UI Elements")]
    public Button automateButton;
    public Button buyStoreButton;
    public Button buy10StoreButton;
    public Button buy100StoreButton;

    public Button storeButton;
    public Text storeCountText;

    Text buyStoreText;
    Text buy10StoreText;
    Text buy100StoreText;

    public Slider progressSlider;

    private void Start()
    {
        InitializeTexts();
    }
    private void Update()
    {
        UpdateCostButtons();
        SetBuyingButtonState();
        if (storeCount > 0)
        {
            RunTimer();

            RunAutomation();
        }
    }

    void InitializeTexts()
    {
        buyStoreText = buyStoreButton.GetComponentInChildren<Text>();
        buy10StoreText = buy10StoreButton.GetComponentInChildren<Text>();
        buy100StoreText = buy100StoreButton.GetComponentInChildren<Text>();
    }

    void UpdateCostButtons()
    {
        buyStoreText.text = "Buy Store: " + CostOf(1).ToString("C2");
        buy10StoreText.text = "Buy Store: " + CostOf(10).ToString("C2");
        buy100StoreText.text = "Buy Store: " + CostOf(100).ToString("C2");
    }

    void SetBuyingButtonState()
    {
        SetButtonState(buy100StoreButton, GameManager.Instance.CanBuy(CostOf(100)));
        SetButtonState(buy10StoreButton, GameManager.Instance.CanBuy(CostOf(10)));
        SetButtonState(buyStoreButton, GameManager.Instance.CanBuy(CostOf(1)));
    }

    void SetButtonState(Button button, bool state)
    {
        button.gameObject.SetActive(state);
    }

    float CostOf(int count)
    {
        if (count == 1)
            return baseStoreCost;

        float total = 0;

        for (int i = 1; i <= count; i++)
        {
            total += baseStoreCost * Mathf.Pow(1 + increaseCostPercentage, i);
        }

        return total;
    }
    void RunTimer()
    {
        if (startTimer)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= timer)
            {
                startTimer = false;
                currentTimer = .0f;

                GameManager.Instance.AddCash(storeIncome * storeCount);
            }
            progressSlider.value = currentTimer / timer;
        }
    }
    void RunAutomation()
    {
        if (!automated)
        {
            if (GameManager.Instance.CanBuy(automationCost))
            {
                automateButton.gameObject.SetActive(true);
            }
            else if (automateButton.gameObject.activeInHierarchy)
            {
                automateButton.gameObject.SetActive(false);
            }
        }
        else
        {
            if (currentTimer == 0)
            {
                StoreOnClick();
            }
        }
    }

    public void BuyStoreOnClick()
    {
        BuyStore(1);
    }

    public void Buy10StoreOnClick()
    {
        BuyStore(10);
    }

    public void Buy100StoreOnClick()
    {
        BuyStore(100);
    }

    void BuyStore(int count)
    {
        float total = CostOf(count);
        if (!GameManager.Instance.CanBuy(total))
            return;

        GameManager.Instance.ExpendCash(total);

        if (storeCount == 0)
            storeButton.gameObject.SetActive(true);

        storeCount += count;

        storeCountText.text = storeCount.ToString();

        UpdateBaseStoreCost(count);
        UpdateCostButtons();
    }

    void UpdateBaseStoreCost(int count)
    {
        // The base cost will be the value of the next increased cost
        baseStoreCost = baseStoreCost * Mathf.Pow(1 + increaseCostPercentage, count);
    }

    public void BuyAutomationButtonOnClick()
    {
        if (!GameManager.Instance.CanBuy(automationCost))
            return;

        GameManager.Instance.ExpendCash(automationCost);

        automated = true;
        automateButton.gameObject.SetActive(false);
        storeButton.gameObject.SetActive(false);
    }

    public void StoreOnClick()
    {
        if (!startTimer && storeCount > 0)
            startTimer = true;

    }
}
