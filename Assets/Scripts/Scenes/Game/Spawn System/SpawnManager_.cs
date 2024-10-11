using System.Collections;
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

    private void Update()
    {
        TestSpawn(testPool);
    }

    private void TestSpawn<T>(T pool)
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TestObject obj = testPool.GetObject();
            Vector2 positon = SpawnPositionManager_.Instance.GetRandomPosition(obj);
            obj.transform.position = positon;
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            TestObject2 obj = testPool2.GetObject();
            Vector2 positon = SpawnPositionManager_.Instance.GetRandomPosition(obj);
            obj.transform.position = positon;
        }
    }
}