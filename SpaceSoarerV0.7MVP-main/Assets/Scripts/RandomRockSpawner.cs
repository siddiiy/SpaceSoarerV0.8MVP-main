using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRockSpawner : MonoBehaviour
{
    public GameObject [] rockPrefab;
    public GameObject plane;
    public float spawnInterval = 0f; // Time interval between spawns
    public float rockSpeed = 0.001f; // Speed at which rocks move from right to left
    public int maxRocks = 50; // Maximum number of rocks on screen at one time
    public Transform spawnPoint;
    public int startFrameCount = 0;//300; // Number of frames to wait before starting to spawn rocks
    public float questionSpawnInterval = 5.0f;
    public int theme; // The theme index to select the appropriate rock prefab


    private float previousRockPosition;
    //private float previousRockPosition = 10;


    //public MathsPuzzleManager PuzzleManage = Plane.FindObjectOfType(typeof(MathsPuzzleManager)) as MathsPuzzleManager;

    private float topViewPort;

    private float bottomViewPort;

    private float validPositionTop;

    private float validPositionBottom;

    private float questionSpawn = 0f;

    private List<GameObject> spawnedRocks = new List<GameObject>();
    private float timer = 0f;

    private bool rockSpawnAllow = true;



    void Start()
    {
        //previousRockPosition = (Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.transform.position.z)).y - Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z)).y) / 2;
        topViewPort = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.transform.position.z)).y;
        bottomViewPort = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z)).y;
        //Centre of the Y view port
        previousRockPosition = 0;
        // SpawnRock();

    }

    public void begin()
    {
        plane.GetComponent<MathsPuzzleManager>().enabled = false;
        rockSpawnAllow = true;
        //previousRockPosition = (Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.transform.position.z)).y - Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z)).y) / 2;
        topViewPort = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.transform.position.z)).y;
        bottomViewPort = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z)).y;
        //Centre of the Y view port
        previousRockPosition = 0;
        // SpawnRock();

    }

    void Update()
    {
        if (startFrameCount > 0)
        {
            startFrameCount--;
            return;
            //frame count down to start spawning rocks
        }

        // Timer to control spawn interval
        timer += Time.deltaTime;
        if (timer >= spawnInterval && spawnedRocks.Count < maxRocks && rockSpawnAllow)
        {

            if (questionSpawn >= questionSpawnInterval)
            {
                rockSpawnAllow = false;
                plane.GetComponent<MathsPuzzleManager>().enabled = true;
                plane.GetComponent<MathsPuzzleManager>().beginPuzzle();
                questionSpawn = 0f;
                //this.enabled = false;
                //thisPlane.MathsPuzzleManager.start();
                //puzzle.start();
                //Plane.FindObjectOfType(typeof(MathsPuzzleManager)).start();
            }
            else
            {
                SpawnRock();
                timer = 0f;
                questionSpawn++;
            }


        }

        // Move the spawned rocks from right to left
        foreach (GameObject rock in spawnedRocks)
        {
            if (rock != null)
            {
                rock.transform.Translate(Vector3.left * rockSpeed * Time.deltaTime);
            }
        }

        // Check if rocks should be destroyed when off-screen
        float screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z)).x;
        for (int i = spawnedRocks.Count - 1; i >= 0; i--)
        {
            if (spawnedRocks[i].transform.position.x < screenLeft)
            {
                Destroy(spawnedRocks[i]);
                spawnedRocks.RemoveAt(i);
            }
        }
    }

    public int spawnYValue()
    {
        return 0;
    }

    void SpawnRock()
    {
        // Determine a random spawn position off-screen to the right
        //float screenTop = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.transform.position.z)).y;
        //float screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z)).y;
        //Debug.Log("Previous rock position is... " + previousRockPosition); 

        float spawnY = Random.Range(/*screenBottom*/previousRockPosition - 50, /*screenTop*/ previousRockPosition + 50);
        while (spawnY < (bottomViewPort - (bottomViewPort / 100) * 80) || spawnY > (topViewPort - (topViewPort / 100) * 5))
        {
            spawnY = Random.Range(/*screenBottom*/previousRockPosition - 50, /*screenTop*/ previousRockPosition + 50);
        }
        previousRockPosition = spawnY;
        Vector3 spawnPosition = spawnPoint.position;
        Vector3 secondSpawnPosition = spawnPoint.position;
        spawnPosition.y = spawnY;
        secondSpawnPosition.y = spawnY - 200;

        //Debug.Log("Spawning rock at position: " + spawnPosition);
        GameObject spawnedRock = Instantiate(rockPrefab[theme], spawnPosition, Quaternion.identity);
        spawnedRock.tag = "Asteroid";
        spawnedRock.SetActive(true);
        spawnedRock.AddComponent<RockRotation>();

        GameObject underneathRock = Instantiate(rockPrefab[theme], secondSpawnPosition, Quaternion.identity);
        underneathRock.tag = "Asteroid";
        underneathRock.SetActive(true);
        underneathRock.AddComponent<RockRotation>(); // Attach the rotation script




        // Add the spawned rock to the list
        spawnedRocks.Add(spawnedRock);
        spawnedRocks.Add(underneathRock);


        //SpriteRenderer spriteRenderer = spawnedRock.GetComponent<SpriteRenderer>();
        //if (spriteRenderer != null)
        //{
        //  spriteRenderer.sortingLayerName = "Default"; // Ensure this matches a visible sorting layer
        //spriteRenderer.sortingOrder = 0; // Adjust the sorting order if needed
        //}
    }

    public void UpdateTheme(int newTheme)
    {
        theme = newTheme;
    }
}