using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{

    public GameObject BotPrefab;

    public List<SpawnerVolume> spawnerVolumes;


    // Start is called before the first frame update
    void Start()
    {
        foreach (var spawner in spawnerVolumes)
        {
            SpawnBot(spawner);
        }
    }


    /// <summary>
    /// Spawn Bots
    /// </summary>
    private void SpawnBot(SpawnerVolume spawnerVolume)
    {
        Instantiate(BotPrefab, spawnerVolume.GetPositionInBounds(), spawnerVolume.transform.rotation);
        Instantiate(BotPrefab, spawnerVolume.GetPositionInBounds(), spawnerVolume.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
