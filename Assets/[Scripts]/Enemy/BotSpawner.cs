using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{

    public GameObject BotPrefab;

    public List<SpawnerVolume> spawnerVolumes;

    public float spawnDelay = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        foreach (var spawner in spawnerVolumes)
        {
            SpawnBot(spawner);
        }

        foreach (var spawner in spawnerVolumes)
        {
            StartCoroutine(SpawnBotCoroutine(spawner));
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


    /// <summary>
    /// Spawn Bot
    /// </summary>
    /// <param name="spawnerVolume"></param>
    /// <returns></returns>
    IEnumerator SpawnBotCoroutine(SpawnerVolume spawnerVolume)
    {
        while (GameManager.GetInstance().maxTimer > 0f)
        {
            if (!GameManager.GetInstance().isPaused)
            {
                yield return new WaitForSeconds(spawnDelay);

                if (!(GameManager.GetInstance().maxTimer <= 0f))
                {
                    Instantiate(BotPrefab, spawnerVolume.GetPositionInBounds(), spawnerVolume.transform.rotation);
                }
            }
        }
    }
}
