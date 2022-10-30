using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;

    private List<GameObject> mobs = new List<GameObject>();
    private List<GameObject> bombs = new List<GameObject>();
    private List<GameObject> bosses = new List<GameObject>();

    public int activeMobsCount, activeBombsCount, activeBossesCount;

    private float yUpperBound = 8, horizontalBound = 10;

    private void Awake()
    {
        int count;
        Enemy.EnemyType enemyType;
        GameObject newGameObject;

        activeMobsCount = activeBombsCount = activeBossesCount = 0;

        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            enemyType = enemyPrefabs[i].GetComponent<Enemy>().enemyType;

            count = 0;

            if (enemyType == Enemy.EnemyType.mobs)
            {
                count = 40;
            }

            else if (enemyType == Enemy.EnemyType.bomb)
            {
                count = 20;
            }

            else if (enemyType == Enemy.EnemyType.boss)
            {
                count = 1;
            }

            for (int j = 0; j < count; j++)
            {
                newGameObject = Instantiate(enemyPrefabs[i], transform);
                newGameObject.SetActive(false);

                if (enemyType == Enemy.EnemyType.mobs)
                {
                    mobs.Add(newGameObject);
                }

                else if (enemyType == Enemy.EnemyType.bomb)
                {
                    bombs.Add(newGameObject);
                }

                else if (enemyType == Enemy.EnemyType.boss)
                {
                    bosses.Add(newGameObject);
                }

            }
        }
    }

    void Start()
    {
        InvokeRepeating("Spawn", 5, 5);
    }
    void Update()
    {
        
    }

    void Spawn()
    {
        GameObject currentGameObject;

        if (activeBombsCount < bombs.Count)
        {
            // move currentGameObject to the back of the list
            currentGameObject = bombs[0];
            bombs.RemoveAt(0);
            bombs.Add(currentGameObject);

            currentGameObject.SetActive(true);
            currentGameObject.transform.position = new Vector2(Random.Range(-horizontalBound, horizontalBound), yUpperBound);

            activeBombsCount++;
        }
    }
}
