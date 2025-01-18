using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Currency Settings")]
    public int currency = 0; // Player's total currency
    public float currencyPerSecond = 1f; // Base earnings per second
    public TextMeshProUGUI currencyText; // Reference to the TextMeshPro UI element

    [Header("Car Purchase Settings")]
    public TextMeshProUGUI buyCarButtonText; // Button text for "Buy Car"
    public Button buyCarButton; // Button to buy a car
    public int carCost = 10; // Cost to purchase a new car
    public GameObject carPrefab; // Car prefab

    [Header("Upgrade Settings")]
    public TextMeshProUGUI upgradeCurrencyButtonText; // Button text for "Upgrade Currency"
    public Button upgradeCurrencyButton; // Button to upgrade currency per second
    public int upgradeCost = 20; // Cost to upgrade currency per second
    public float upgradeIncrement = 1f; // Increment for currency per second

    [Header("Waypoint Settings")]
    public Transform[] waypoints; // Waypoints for the cars to follow

    [Header("Game State")]
    public int carCount = 1; // Current number of cars

    void Start()
    {
        SpawnCars(carCount); // Spawn initial cars
        InvokeRepeating("GenerateCurrency", 1f, 1f); // Start currency generation
        UpdateCurrencyUI();
    }

    void GenerateCurrency()
    {
        currency += Mathf.RoundToInt(currencyPerSecond);
        UpdateCurrencyUI();
    }

    public void UpdateCurrencyUI()
    {
        currencyText.text = $"Currency: {currency}";
        buyCarButtonText.text = $"Buy Car: {carCost}";
        upgradeCurrencyButtonText.text = $"Upgrade Currency: {upgradeCost}";

        buyCarButton.interactable = currency >= carCost;
        upgradeCurrencyButton.interactable = currency >= upgradeCost;
    }

    public void BuyCar()
    {
        if (currency >= carCost)
        {
            currency -= carCost; // Deduct cost
            carCost += 5; // Increment the cost of the next car (optional scaling)
            SpawnCars(1); // Spawn one additional car
            UpdateCurrencyUI();
        }
        else
        {
            Debug.Log("Not enough currency to buy a car!");
        }
    }

    public void UpgradeCurrency()
    {
        if (currency >= upgradeCost)
        {
            currency -= upgradeCost; // Deduct cost
            currencyPerSecond += upgradeIncrement; // Increase currency per second
            upgradeCost += 10; // Increment the cost of the next upgrade (optional scaling)
            UpdateCurrencyUI();
        }
        else
        {
            Debug.Log("Not enough currency to upgrade!");
        }
    }

    void SpawnCars(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Spawn the car at a waypoint or a default position
            Vector3 spawnPosition = waypoints[0].position; // Adjust as needed
            GameObject car = Instantiate(carPrefab, spawnPosition, Quaternion.identity);

            // Assign waypoints to the car
            CarMovement carMovement = car.GetComponentInChildren<CarMovement>();
            if (carMovement != null)
            {
                carMovement.AssignWaypoints(waypoints);
            }
        }
    }
}
