using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject employeePrefab;
    public GameObject squarePrefab;
    public GameObject trianglePrefab;
    public GameObject circlePrefab;
    public float spawnInterval = 10f;
    public float spawnHeight = 0.24f; // Height at which obstacles spawn
    public float removeOffset = 2.0f; // Offset for removing obstacles when they're off-screen

    public GameObject pointSystem;
    private PointSystem pointSystemScript;
    private Camera mainCamera;
    private float nextSpawnTime = 0.0f;
    private List<GameObject> obstacles = new List<GameObject>();
    private List<GameObject> employees = new List<GameObject>();

    private List<Shape> shapes = new List<Shape> { Shape.Down, Shape.Up, Shape.Suspended };
    private int index = 0;

    private int count = 0;
    public bool tutorial = true;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    private void GameManagerOnOnGameStateChanged (GameState state)
    {
        // Making sure objects won't spawn inside of eachother
        if(state == GameState.Paused)
        {
            Time.timeScale = 0;
        }

        if (state == GameState.Playing)
        {
            Time.timeScale = 1;
        }

        if (state == GameState.MainMenu)
        {
            if (obstacles.Count > 0)
            {
                foreach (var obstacle in obstacles.ToList())
                {
                    obstacles.Remove(obstacle);
                    Destroy(obstacle);
                }
            }

            if (employees.Count > 0)
            {
                foreach (var employee in employees.ToList())
                {
                    employees.Remove(employee);
                    Destroy(employee);
                }
            }
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        pointSystemScript = pointSystem.GetComponent<PointSystem>();
    }


    private void Update()
    {

        if (GameManager.Instance.gameState == GameState.Playing)
        {
            // Check if it's time to spawn a new obstacle
            if (Time.time >= nextSpawnTime)
            {
                

                // Instantiate the obstacle
                SpawnObstacle();

                // TODO rewrite so this fits lol, general ObjectSpawner script?
                SpawnEmployee();

                // Schedule the next spawn
                nextSpawnTime = Time.time + spawnInterval;
            }
        }

        // Remove obstacles that are off-screen
        RemoveOffScreenObstacles();
        RemoveOffScreenEmployees();
    }

    public void RemoveOffScreenObstacles()
    {
        //GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle"); // Assuming obstacles have a "Obstacle" tag

        foreach (var obstacle in obstacles.ToList())
        {
            Vector3 screenPoint = mainCamera.WorldToViewportPoint(obstacle.transform.position);
            
            // If the obstacle is off-screen to the left, remove it
            if (screenPoint.x < -.1f)
            {
                obstacles.Remove(obstacle);
                Destroy(obstacle);
            }
        }
    }

    public void RemoveOffScreenEmployees()
    {
        //GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle"); // Assuming obstacles have a "Obstacle" tag

        foreach (var employee in employees.ToList())
        {
            Vector3 screenPoint = mainCamera.WorldToViewportPoint(employee.transform.position);

            // If the obstacle is off-screen to the left, remove it
            if (screenPoint.x < -.1f)
            {
                employees.Remove(employee);
                Destroy(employee);
            }
        }
    }

    private void SpawnObstacle()
    {
        Debug.Log("New Obstacle");
        // Calculate the spawn position based on the camera's right edge and the desired height
        Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(1.1f, spawnHeight, 4.4f));

        Shape objectShape = shapes[index];

        // Spawning the obstacle and assigning shape
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        obstacle.GetComponent<Obstacle>().shape = objectShape;

        // Adding new obstacle to list
        obstacles.Add(obstacle);

        // Use next shape for next object spawning
        index++;

        // Checking if the current shuffled shape list has been used completely
        if (index >= shapes.Count)
        {
            // Shuffle list and begin using again
            shapes.Sort((a, b) => 1 - 2 * Random.Range(0, 1));
            index = 0;
        }

        count++;

        if (tutorial)
        {
            TutorialSpawn(obstacle);
        }
        
        if (count > 3)
        {
            //Disable tutorial
            tutorial = false;
        }

        
    }

    private void TutorialSpawn(GameObject obstacle)
    {
        // Calculate the spawn position for the prefab exactly above the obstacle
        Vector3 prefabSpawnPosition = obstacle.transform.position + Vector3.up * obstacle.GetComponent<Renderer>().bounds.extents.y;

        // Set the prefab's position to the calculated position
        GameObject prefabToSpawn = null;

        // Determine which prefab to spawn based on the count
        if (count == 1)
        {
            prefabToSpawn = squarePrefab;
            
            //lift this one up a bit
            prefabSpawnPosition = obstacle.transform.position + Vector3.up * obstacle.GetComponent<Renderer>().bounds.extents.y + Vector3.up * 4.5f;
        }
        else if (count == 2)
        {
            prefabToSpawn = trianglePrefab;
        }
        else if (count == 3)
        {
            prefabToSpawn = circlePrefab;
        }

        if (prefabToSpawn != null)
        {
            // Instantiate the prefab
            GameObject spawnedPrefab = Instantiate(prefabToSpawn, prefabSpawnPosition, Quaternion.identity);
            
            // Set the sorting order of the prefab's renderer to be higher than the obstacle's renderer
            Renderer prefabRenderer = spawnedPrefab.GetComponent<Renderer>();
            Renderer obstacleRenderer = obstacle.GetComponent<Renderer>();
            
            if (prefabRenderer != null && obstacleRenderer != null)
            {
                prefabRenderer.sortingOrder = obstacleRenderer.sortingOrder + 1;
            }
        }
    }

    private void SpawnEmployee()
    {
        // Calculate the spawn position based on the camera's right edge and the desired height
        Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(1.1f, 0.72f, 4.8f));
        GameObject employee = Instantiate(employeePrefab, spawnPosition, Quaternion.identity);

        employees.Add(employee);
    }

}
