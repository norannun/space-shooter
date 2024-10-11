using UnityEngine;

public class TestPool2 : MonoBehaviour
{
    [SerializeField] private TestPool2Config config;
    private SpawnableObjectPool<TestObject2> _pool;

    private void Awake()
    {
        _pool = new SpawnableObjectPool<TestObject2>(config, transform);
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
