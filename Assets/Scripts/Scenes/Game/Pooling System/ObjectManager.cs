using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectManager : MonoBehaviour
{
    [SerializeField] private ObjectManagerConfig _configs;

    private Dictionary<string, ObjectPool> _pools;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        // Initialize pools using configs
        // Configs has data for an object abstraction & its pool

        _pools = new Dictionary<string, ObjectPool>();

        foreach (PoolableObjectConfig config in _configs.configs)
        {
            GameObject pool = new();
            ObjectPool comp = pool.AddComponent<ObjectPool>();
            comp.Initialize(config);
            pool.transform.parent = transform;
            pool.name = $"{config.objectName} Pool";

            _pools.Add(config.objectName, comp);
        }
    }

    public PoolableObject GetObject(string name)
    {
        return _pools[name].Get();
    }

    public void ReturnObject(string name, PoolableObject comp)
    {
        _pools[name].Return(comp);
    }
}

[CreateAssetMenu(fileName = "ObjectManager Config", menuName = "Configs/Pooling System/ObjectManager")]
public class ObjectManagerConfig : ScriptableObject
{
    public PoolableObjectConfig[] configs;
}
