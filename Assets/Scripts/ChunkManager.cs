using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public GameObject player;
    public GameObject cameraBounds;
    public GameObject startChunk;
    public GameObject emptyChunk;
    [SerializeField]
    public List<GameObject> chunkPrefabs = new List<GameObject>();

    private List<GameObject> chunks = new List<GameObject>();

    private float nextSpawnPosition;
    private float levelPositionY;
    private Vector3 startPositionCameraBounds;
    private bool useEmptyChunk = false;

    void Awake()
    {
        nextSpawnPosition = startChunk.transform.position.x + startChunk.GetComponent<BoxCollider2D>().size.x;
        levelPositionY = startChunk.transform.position.y;
        startPositionCameraBounds = cameraBounds.transform.position;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    void Update()
    {
        if (chunks.Count != 0)
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                float chunkWidth = chunks[i].GetComponent<BoxCollider2D>().size.x;

                if (player.transform.position.x > nextSpawnPosition - chunkWidth / 2)
                {
                    Vector3 newPosition = new Vector3(nextSpawnPosition, levelPositionY, 0);
                    float newChunkWidth;


                    if (useEmptyChunk)
                    {

                        // Spawning an empty 'rest' chunk
                        GameObject newChunk = InstanciateChunk(emptyChunk, newPosition);
                        newChunkWidth = newChunk.GetComponent<BoxCollider2D>().size.x;

                    }
                    else
                    {

                        // Spawning a chunk with obstacles
                        int randomChunkIndex = UnityEngine.Random.Range(0, chunkPrefabs.Count);
                        GameObject newChunk =  InstanciateChunk(chunkPrefabs[randomChunkIndex], newPosition);
                        newChunkWidth = newChunk.GetComponent<BoxCollider2D>().size.x;

                    }

                    cameraBounds.transform.position = new Vector3(cameraBounds.transform.position.x + newChunkWidth, cameraBounds.transform.position.y, cameraBounds.transform.position.z);
                    nextSpawnPosition += newChunkWidth;
                    useEmptyChunk = !useEmptyChunk;

                    // Removing the old chunks
                    if (chunks.Count > 2)
                    {
                        Destroy(chunks[0]);
                        chunks.Remove(chunks[0]);
                    }
                }


            }
        }
        else
        {
            chunks.Add(startChunk);
        }
    }

    private GameObject InstanciateChunk(GameObject chunk, Vector3 position)
    {
        GameObject newChunk = Instantiate(chunk, position, chunk.transform.rotation);
        chunks.Add(newChunk);
        return newChunk;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.MainMenu)
        {
            resetManager();
        }
    }

    private void resetManager()
    {
        nextSpawnPosition = startChunk.transform.position.x + startChunk.GetComponent<BoxCollider2D>().size.x;
        useEmptyChunk = false;
        cameraBounds.transform.position = startPositionCameraBounds;
        foreach(GameObject chunk in chunks)
        {
            Destroy(chunk);
        }

        chunks = new List<GameObject>();
        InstanciateChunk(startChunk, startChunk.transform.position);
    }
}
