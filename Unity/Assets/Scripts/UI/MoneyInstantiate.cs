using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.ARFoundation;

public class MoneyInstantiate : MonoBehaviour
{
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] GameObject moneyFallingPrefab;
    [SerializeField] GameObject player;
    [SerializeField] float spawnRadius = 100f;

    List<GameObject> moneyPool;
    List<GameObject> moneyFallingPool;
    // Start is called before the first frame update
    void Awake()
    {
        Assert.IsNotNull(moneyPrefab);
        Assert.IsNotNull(player);
    }

    void Start()
    {
        moneyPool = new List<GameObject>();
        moneyFallingPool = new List<GameObject>();
        StartCoroutine(GenerateObject()); // generate 100 money objects
        StartCoroutine(ActivateObject()); // start playing the money flying animation
    }


    IEnumerator GenerateObject()
    {
        for (int i = 0; i < 100; i++)
        {
            // 在随机位置生成物体
            Vector3 spawnPosition = GetRandomPosition(-20f);
            var money = Instantiate(moneyPrefab, spawnPosition, Quaternion.identity);
            moneyPool.Add(money);
            money.AddComponent<ARAnchor>();
            money.SetActive(false);

            spawnPosition = GetRandomPosition(200f);
            // double the number of money objects
            var fallingMoney = Instantiate(moneyFallingPrefab, spawnPosition, Quaternion.identity);
            moneyFallingPool.Add(fallingMoney);
            fallingMoney.AddComponent<ARAnchor>();
            fallingMoney.SetActive(false);

            spawnPosition = GetRandomPosition(200f);
            fallingMoney = Instantiate(moneyFallingPrefab, spawnPosition, Quaternion.identity);
            moneyFallingPool.Add(fallingMoney);
            fallingMoney.AddComponent<ARAnchor>();
            fallingMoney.SetActive(false);
        }
        yield return null;
    }

    IEnumerator ActivateObject()
    {
        while (true)
        {
            for (int i = 0; i < moneyFallingPool.Count; i++)
            {
                if (i < moneyPool.Count)
                {
                    if (moneyPool[i].activeSelf)
                    {
                        yield return null;
                        continue; // if the money is already active, skip it
                    }
                    // initialize the money position and play the animation
                    moneyPool[i].transform.position = GetRandomPosition(-20f);
                    moneyPool[i].SetActive(true);
                    moneyPool[i].GetComponent<MoneyFlying>()?.Play();
                    yield return new WaitForSeconds(0.1f);
                }

                if (moneyFallingPool[i].activeSelf)
                {
                    yield return null;
                    continue; // if the money is already active, skip it
                }
                // initialize the money position and play the animation
                moneyFallingPool[i].transform.position = GetRandomPosition(200f);
                moneyFallingPool[i].SetActive(true);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private Vector3 GetRandomPosition(float height)
    {
        // random position in a circle
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = player.transform.position + new Vector3(randomCircle.x, height, randomCircle.y);
        return spawnPosition;
    }
}
