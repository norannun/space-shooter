using UnityEngine;

public class TestPool : MonoBehaviour
{
    [SerializeField] private TestPoolConfig config;
    private SpawnableObjectPool<TestObject> _pool;

    private void Awake()
    {
        _pool = new SpawnableObjectPool<TestObject>(config, transform);
    }

    public TestObject GetObject()
    {
        return _pool.Get();
    }

    public void ReturnObject(TestObject comp)
    {
        _pool.Return(comp);
    }
}


[CreateAssetMenu(fileName="TestPool Config", menuName="Configs/Spawn System/Pools/TestPool Config")]
public class TestPoolConfig : SpawnableObjectPoolConfig
{

}
