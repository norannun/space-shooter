using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_ : MonoBehaviour
{
    public SpawnManager_ Instance { get; private set; }

    [SerializeField] private TestPool testPool;
    [SerializeField] private TestPool2 testPool2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(Test2Spawn(testPool2));
    }

    private void Update()
    {
        TestSpawn(testPool);
    }

    private void TestSpawn<T>(T pool)
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TestObject obj = testPool.GetObject();
            Vector2 positon = SpawnPositionManager_.Instance.GetPosition(obj);
            obj.transform.position = positon;
        }
    }

    private IEnumerator Test2Spawn(TestPool2 pool)
    {
        while (true)
        {
            TestObject2 obj = testPool2.GetObject();
            Vector2 position = SpawnPositionManager_.Instance.GetPosition(obj);
            obj.transform.position = position;


            yield return new WaitForSeconds(3.5f);
        }
    }
}