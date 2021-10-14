using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public int crateMaxInstances = 50;

    [SerializeField]
    private GameObject botPrefab;
    [SerializeField]
    GameObject cratePrefab;

    [SerializeField]
    Transform spawnParent;

    [SerializeField]
    Tilemap groundTilemap;

    [Header("Crate rate (1 new every X seconds)")]
    public float crateCreationRate;
    float spawnTimer = 0;
    GameObject[] cratePool;

    void Awake()
    {
        cratePool = new GameObject[crateMaxInstances];
        spawnTimer = crateCreationRate;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            AddBot();
        }
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0)
        {
            SpawnCrate();
            spawnTimer = crateCreationRate;
        }
    }

    void SpawnCrate()
    {
        Vector3 spawnPosition = GetRandomTilemapPosition();
        for (int i = 0; i < cratePool.Length; i++)
        {
            if (!cratePool[i])
            {
                cratePool[i] = Instantiate(cratePrefab, spawnPosition, Quaternion.identity, spawnParent);
                return;
            }
            else if (!cratePool[i].activeSelf)
            {
                cratePool[i].transform.position = spawnPosition;
                Health health = cratePool[i].GetComponent<Health>();
                if (health)
                    cratePool[i].GetComponent<Health>().ResetHealth();
                cratePool[i].SetActive(true);
                return;
            }

        }
    }

    void AddBot()
    {
        Vector3 spawnPosition = GetRandomTilemapPosition();
        GameObject bot = Instantiate(botPrefab, spawnPosition, Quaternion.identity);
    }

    Vector3 GetRandomTilemapPosition()
    {
        Vector3 tilemapOffset = groundTilemap.transform.position;
        BoundsInt tilemapBounds = groundTilemap.cellBounds;

        float minX = tilemapBounds.min.x + tilemapOffset.x;
        float maxX = tilemapBounds.max.x + tilemapOffset.x;
        float minY = tilemapBounds.min.y + tilemapOffset.y;
        float maxY = tilemapBounds.max.y + tilemapOffset.y;
        return new Vector3(Random.Range(minX + 1, maxX - 1), Random.Range(minY + 1, maxY - 1), 0f);
    }
}