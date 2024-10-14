using UnityEngine;

public class TestPool2 : MonoBehaviour
{
    public static TestPool2 Instance { get; private set; }

    [SerializeField] private TestPool2Config config;
    private SpawnableObjectPool<TestObject2> _pool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _pool = new SpawnableObjectPool<TestObject2>(config, transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public TestObject2 GetObject()
    {
        return _pool.Get();
    }

    public void ReturnObject(TestObject2 comp)
    {
        _pool.Return(comp);
    }
}


[CreateAssetMenu(fileName = "TestPool2 Config", menuName = "Configs/Spawn System/Pools/TestPool2 Config")]
public class TestPool2Config : SpawnableObjectPoolConfig
{

}
