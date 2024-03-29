using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public List<GameObject> activeEnemies;

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
        InvokeRepeating("Spawn", 1, 3);
    }
    void Update()
    {
        
    }

    void Spawn()
    {
        GameObject currentGameObject;

        if (activeBombsCount < bombs.Count)
        {
            int i;
            bool terminate;
            GameObject temp;

            // move currentGameObject to the back of the list
            currentGameObject = bombs[0];
            bombs.RemoveAt(0);
            bombs.Add(currentGameObject);

            activeBombsCount++;

            currentGameObject.transform.position = new Vector2(Random.Range(-horizontalBound, horizontalBound), yUpperBound);

            // insert with sorting
            activeEnemies.Add(currentGameObject);
            terminate = false;
            i = activeEnemies.Count - 1;

            while (i > 0 && !terminate)
            {
                if (activeEnemies[i].transform.position.y < activeEnemies[i - 1].transform.position.y)
                {
                    temp = activeEnemies[i];

                    activeEnemies[i] = activeEnemies[i - 1];
                    activeEnemies[i - 1] = temp;
                    i--;
                }

                else
                {
                    terminate = true;

                }
            }

            currentGameObject.SetActive(true);
        }
    }
}
