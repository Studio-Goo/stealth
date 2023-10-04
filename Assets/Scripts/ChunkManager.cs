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
    public GameObject tutorialChunk;

    [SerializeField]
    //public List<List<GameObject>> chunkPrefabs = new List<List<GameObject>>();
    public List<GameObject>[] chunkPrefabs = new List<GameObject>[7];

    [Header("Level chunks")]
    [SerializeField]
    public List<GameObject> chunkPrefabs1 = new List<GameObject>();
    public List<GameObject> chunkPrefabs2 = new List<GameObject>();
    public List<GameObject> chunkPrefabs3 = new List<GameObject>();
    public List<GameObject> chunkPrefabs4 = new List<GameObject>();
    public List<GameObject> chunkPrefabs5 = new List<GameObject>();
    public List<GameObject> chunkPrefabs6 = new List<GameObject>();
    public List<GameObject> chunkPrefabs7 = new List<GameObject>();

    private List<GameObject> currentChunks = new List<GameObject>();

    private List<GameObject> chunks = new List<GameObject>();

    private float nextSpawnPosition;
    private float levelPositionY;
    private Vector3 startPositionCameraBounds;
    private bool useEmptyChunk = false;
    private float tutorial;

    private PointSystem pointSystem;
    private float oldChunkWidth = 80;

    void Awake()
    {
        nextSpawnPosition = startChunk.transform.position.x + startChunk.GetComponent<BoxCollider2D>().size.x;
        levelPositionY = startChunk.transform.position.y;
        startPositionCameraBounds = cameraBounds.transform.position;
        GameManager.OnGameStateChanged += OnGameStateChanged;

        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            Debug.Log("Doesnt have tutorial");
            PlayerPrefs.SetInt("Tutorial", 1);
            tutorial = 1;
        }
        else
        {
            // Disable this in build!!
            PlayerPrefs.SetInt("Tutorial", 1);
        }
    }

    private void Start()
    {
        //chunkPrefabs[0] = chunkPrefabs1;
        //chunkPrefabs[1] = chunkPrefabs2;
        //chunkPrefabs[2] = chunkPrefabs3;
        //chunkPrefabs[3] = chunkPrefabs4;
        //chunkPrefabs[4] = chunkPrefabs5;
        //chunkPrefabs[5] = chunkPrefabs6;
        //chunkPrefabs[6] = chunkPrefabs7;
        currentChunks = chunkPrefabs1;

        //pointSystem = PointSystem.Instance;
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

                if (player.transform.position.x > nextSpawnPosition - 40)
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
                        if (tutorial == 1)
                        {
                            // Spawn tutorial chunk
                            //int randomChunkIndex = UnityEngine.Random.Range(0, chunkPrefabs.Count);
                            GameObject newChunk = InstanciateChunk(tutorialChunk, newPosition);
                            newChunkWidth = newChunk.GetComponent<BoxCollider2D>().size.x;
                            tutorial = 0;
                        } else
                        {
                            // Spawn normal chunk
                            //int randomChunkIndex;
                            GameObject newChunk;

                            if (currentChunks.Count > 0)
                            {
                                int randomChunkIndex = UnityEngine.Random.Range(0, currentChunks.Count);
                                newChunk = InstanciateChunk(currentChunks[randomChunkIndex], newPosition);
                            } else
                            {
                                Debug.LogError("Chunk level doesn't have any chunks instanciated.");
                                newChunk = InstanciateChunk(emptyChunk, newPosition);
                            }
                            //GameObject newChunk = InstanciateChunk(chunkPrefabs[currentChunkLevel][randomChunkIndex], newPosition);
                            newChunkWidth = newChunk.GetComponent<BoxCollider2D>().size.x;
                        }

                    }

                    cameraBounds.transform.position = new Vector3(cameraBounds.transform.position.x + oldChunkWidth, cameraBounds.transform.position.y, cameraBounds.transform.position.z);
                    nextSpawnPosition += newChunkWidth;
                    useEmptyChunk = !useEmptyChunk;
                    oldChunkWidth = newChunkWidth;

                    // Removing the old chunks
                    // Keeping 4 loaded at the same time avoids chunks despawning while still in view
                    if (chunks.Count > 4)
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

        if (pointSystem)
        {
            int points = pointSystem.GetPoints();

            if (points < 5)
            {
                currentChunks = chunkPrefabs1;
            // } else if (points < 15)
            // {
            //     currentChunks = chunkPrefabs2;
            } else if (points < 20)
            {
                currentChunks = chunkPrefabs3;
            // } else if (points < 40)
            // {
            //     currentChunks = chunkPrefabs4;
            } else if (points < 60)
            {
                currentChunks = chunkPrefabs5;
            // } else if (points < 80)
            // {
            //     currentChunks = chunkPrefabs6;
            } else
            {
                currentChunks = chunkPrefabs7;
            }
        } else
        {
            pointSystem = PointSystem.Instance;
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
