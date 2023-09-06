using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnInterval = 2.0f;
    public float spawnHeight = 0.24f; // Height at which obstacles spawn
    public float removeOffset = 2.0f; // Offset for removing obstacles when they're off-screen

    private Camera mainCamera;
    private float nextSpawnTime = 0.0f;
    private List<GameObject> obstacles = new List<GameObject>();

    private List<Shape> shapes = new List<Shape> { Shape.Horizontal, Shape.Vertical };
    private int index = 0;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check if it's time to spawn a new obstacle
        if (Time.time >= nextSpawnTime)
        {

            // Instantiate the obstacle
            SpawnObstacle();

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

    private void SpawnObstacle()
    {

        // Calculate the spawn position based on the camera's right edge and the desired height
        Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(1.1f, spawnHeight, 10.0f));

        Shape objectShape = shapes[index];

        // Spawning the obstacle and assigning shape
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        obstacle.GetComponent<Obstacle>().shape = objectShape;

        // Adding new obstacle to list
        obstacles.Add(obstacle);

        // Use next shape for next object spawning
        index++;

        // Checking if current shuffled shape list has been used completely
        if (index >= shapes.Count)
        {
            // Shuffle list and begin using again
            shapes.Sort((a, b) => 1 - 2 * Random.Range(0, 1));
            Debug.Log(shapes[0]);
            index = 0;
        }
    }

}
