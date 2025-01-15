using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int currency = 0; // Player's total currency
    public float currencyPerSecond = 1f; // Base earnings per second
    public TextMeshProUGUI currencyText; // Reference to the TextMeshPro UI element

    public GameObject carPrefab;  // Car prefab
    public Transform[] waypoints; // Waypoints for the cars to follow
    public int carCount = 1;      // Number of cars to spawn

    void Start()
    {
        SpawnCars();
        InvokeRepeating("GenerateCurrency", 1f, 1f); // Start currency generation
        UpdateCurrencyUI();
    }

    void SpawnCars()
    {
        for (int i = 0; i < carCount; i++)
        {
            // Spawn the car at the position of a waypoint
            Vector3 spawnPosition = waypoints[i % waypoints.Length].position;
            GameObject car = Instantiate(carPrefab, spawnPosition, Quaternion.identity);

            CarMovement carMovement = car.GetComponentInChildren<CarMovement>();
            if (carMovement != null)
            {
                Debug.Log("CarMovement script found on child of car prefab.");
                carMovement.AssignWaypoints(waypoints);
                carMovement.SetStartWaypointIndex(i % waypoints.Length); // Assign a unique start index

                // Randomize speed
                UnityEngine.AI.NavMeshAgent agent = car.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
                if (agent != null)
                {
                    agent.speed = Random.Range(3.0f, 5.0f); // Adjust range for more noticeable differences
                }
            }
            else
            {
                Debug.LogError("CarMovement script is missing in the child GameObjects of the car prefab!");
            }
        }
    }

    void GenerateCurrency()
    {
        currency += Mathf.RoundToInt(currencyPerSecond);
        UpdateCurrencyUI();
    }

    void UpdateCurrencyUI()
    {
        currencyText.text = "Currency: " + currency.ToString();
    }
}
