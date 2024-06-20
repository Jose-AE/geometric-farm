using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class FarmChunkGenerator : MonoBehaviour
{
    [SerializeField] GameObject chunk;
    [SerializeField] Transform player;
    [SerializeField] int generationRadius = 2;

    [Header("Chunk Settings")]
    [SerializeField] float chunkSize = 12f;
    [SerializeField] LayerMask plantLayer;
    [SerializeField] int plantsPerChunk = 4;
    [SerializeField] float plantSpacing = 1f;
    [SerializeField] GameObject[] plantObjects;
    [SerializeField] Material[] plantMaterials;

    private Grid grid;

    Dictionary<Vector3Int, GameObject> tilePlane = new Dictionary<Vector3Int, GameObject>();


    void Awake()
    {
        grid = GetComponent<Grid>();
    }



    void Update()
    {
        generateWorld();
    }

    private void generateWorld()
    {
        for (int x = -generationRadius; x < generationRadius; x++)
        {
            for (int z = -generationRadius; z < generationRadius; z++)
            {
                Vector3Int offset = grid.WorldToCell(player.position);


                Vector3Int cell = new Vector3Int(
                    x + offset.x,
                    0,
                    z + offset.z
                );

                if (!tilePlane.ContainsKey(cell))
                {
                    GameObject chunk = Instantiate(this.chunk, grid.CellToWorld(cell), Quaternion.identity);
                    tilePlane.Add(cell, chunk);
                    SpawnPlants(chunk);
                }


            }
        }
    }



    void SpawnPlants(GameObject chunk)
    {
        if (plantObjects.Length == 0) return;

        for (int i = 0; i < plantsPerChunk; i++)
        {

            for (int _ = 0; _ < 10; _++) //try 10 times to place a plant else dont
            {
                Vector3 randomPos = new Vector3(Random.Range(0f + chunk.transform.position.x, (chunkSize - plantSpacing) + chunk.transform.position.x), 0, Random.Range(0 + chunk.transform.position.z, (chunkSize - plantSpacing) + chunk.transform.position.z));

                if (!Physics.CheckSphere(randomPos, plantSpacing, plantLayer))
                {
                    int randomIndex = Random.Range(0, plantObjects.Length);
                    float randomRotationY = Random.Range(0, 360);

                    GameObject plant = Instantiate(plantObjects[randomIndex], randomPos, Quaternion.Euler(0f, randomRotationY, 0f));
                    plant.transform.GetChild(0).GetComponent<MeshRenderer>().material = plantMaterials[Random.Range(0, plantMaterials.Length)];
                    plant.transform.SetParent(gameObject.transform);
                    plant.layer = 6;
                    break;
                }

            }

        }
    }


}
