using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Analytics;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpotState
    {
        public Transform location;
        public bool occupied;

        public void ToggleSpot()
        {
            occupied = !occupied;
        }
    }

    [SerializeField]
    private float spawnFrequency;

    [SerializeField]
    private Vector3 spawnOffset;

    [SerializeField]
    private List<GameObject> enemies;

    [SerializeField]
    private List<Transform> spots = new List<Transform>();

    private List<SpotState> spotStates = new List<SpotState>();
    private float spawnTime = 0;

    private void Awake()
    {
        spots.ForEach(i => spotStates.Add(new SpotState { location = i, occupied = false}));
    }

    private void Update()
    {
        if (spawnTime > 0f)
        {
            spawnTime -= Time.deltaTime;
            return;
        }

        for (int i = 0; i < spotStates.Count; i++)
        {
            SpotState state = spotStates[i];
            if (state.occupied || !state.location )
            {
                continue;
            }

            GameObject enemyPrefab = enemies[Random.Range(0, enemies.Count)];
            GameObject enemyObj = Instantiate(enemyPrefab, state.location.position + spawnOffset, Quaternion.identity);

            state.occupied = true;
            spotStates[i] = state;

            Enemy enemy = enemyObj.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.spawner = this;
                enemy.spotIndex = i;
                enemy.destination = state.location.position.y;
            }

            break;
        }

        spawnTime = spawnFrequency;
    }

    public void FreeSpot(int index)
    {
        if (index >= 0 && index < spotStates.Count)
        {
            SpotState state = spotStates[index];
            state.occupied = false;
            spotStates[index] = state;
        }
    }
}
