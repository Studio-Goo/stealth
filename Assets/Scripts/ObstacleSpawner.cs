using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnInterval = 2.0f;
    public float spawnHeight = 2.0f; // Height at which obstacles spawn
    public float removeOffset = 2.0f; // Offset for removing obstacles when they're off-screen

    private Camera mainCamera;
    private float nextSpawnTime = 0.0f;
    private List<GameObject> obstacles = new List<GameObject>();

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check if it's time to spawn a new obstacle
        if (Time.time >= nextSpawnTime)
        {
            // Calculate the spawn position based on the camera's right edge and the desired height
            Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(1.1f, spawnHeight, 10.0f));

            // Instantiate the obstacle
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

            // Adding the obstacle to global array for easy access
            obstacles.Add(obstacle);

            // Schedule the next spawn
            nextSpawnTime = Time.time + spawnInterval;
        }

        // Remove obstacles that are off-screen
        RemoveOffScreenObstacles();
    }

    private void RemoveOffScreenObstacles()
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
}
