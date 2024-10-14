using UnityEngine;

public class TestPool : MonoBehaviour
{
    public static TestPool Instance { get; private set; }

    [SerializeField] private TestPoolConfig config;
    private SpawnableObjectPool<TestObject> _pool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _pool = new SpawnableObjectPool<TestObject>(config, transform);
        }
        else
        {
            Destroy(gameObject);
        }
        
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


[CreateAssetMenu(fileName="TestPool Config", menuName="Configs/Spawn System/Pools/TestPool")]
public class TestPoolConfig : SpawnableObjectPoolConfig
{

}
