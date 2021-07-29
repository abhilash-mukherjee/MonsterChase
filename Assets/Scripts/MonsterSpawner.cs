using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] monsterList;
    [SerializeField]
    private float minTime, maxTime;
    [SerializeField]
    private float minMonsterSpeed, maxMonsterSpeed;
    private List<GameObject> spawnerLeftRight = new List<GameObject>();
    private int spawnLeftOrRight;
    void Start()
    {
        spawnerLeftRight.Add(gameObject.transform.GetChild(0).gameObject);
        spawnerLeftRight.Add(gameObject.transform.GetChild(1).gameObject);
        minTime = GameManager.Instance.MinTimeGapBetweenTwoMonsterSpawnsForLevel;
        maxTime = GameManager.Instance.MaxTimeGapBetweenTwoMonsterSpawnsForLevel;
        minMonsterSpeed = GameManager.Instance.MinMonsterSpeedForLevel;
        maxMonsterSpeed = GameManager.Instance.MaxMonsterSpeedForLevel;
        StartCoroutine(SpawnMonstors());
    }

    IEnumerator SpawnMonstors()
    {
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        spawnLeftOrRight = Random.Range(0, 2);
        GameObject spawnedMonster = Instantiate(monsterList[Random.Range(0, 3)], spawnerLeftRight[spawnLeftOrRight].transform);
        if(spawnLeftOrRight == 0)
        {
            spawnedMonster.GetComponent<Monster>().Monsterspeed = Random.Range(minMonsterSpeed, maxMonsterSpeed);
        }
        else
        {
            spawnedMonster.GetComponent<Monster>().Monsterspeed = -Random.Range(minMonsterSpeed,maxMonsterSpeed );
        }
        StartCoroutine(SpawnMonstors());
    }
   
}
