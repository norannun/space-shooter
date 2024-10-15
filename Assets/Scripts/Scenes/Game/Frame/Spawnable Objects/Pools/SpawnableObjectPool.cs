using System.Collections.Generic;
using UnityEngine;

public class SpawnableObjectPool: MonoBehaviour
{
    private SpawnableObjectConfig _spawnableObjectConfig;
    private int _expansionSize;

    private Queue<SpawnableObject> _pool = new Queue<SpawnableObject>();

    public void Initialize(SpawnableObjectConfig config)
    {
        _spawnableObjectConfig = config;
        _expansionSize = config.poolExpSize;

        Expand(config.poolInitSize);
    }

    private SpawnableObject Expand(int toAdd)
    {
        SpawnableObject tmp = null;  // Used if there is no objects left

        for (int i = 0; i < toAdd; i++)
        {
            SpawnableObject comp = InitializeObject(_spawnableObjectConfig);

            // SpawnableObject deactivates itself during initialization
            _pool.Enqueue(comp);
            if (i == toAdd - 1)
            {
                tmp = comp;
            }
        }

        return tmp;
    }

    private SpawnableObject InitializeObject(SpawnableObjectConfig config)
    {
        GameObject obj = Instantiate(config.prefab);
        SpawnableObject comp = obj.GetComponent<SpawnableObject>();
        Unclassified.NullCheckComponent(comp);
        comp.Initialize(config);

        return comp;
    }

    public SpawnableObject Get()
    {
        SpawnableObject comp;

        if (_pool.Count > 0)
        {
            comp = _pool.Dequeue();
        }
        else
        {
            comp = Expand(_expansionSize);
            _pool.Dequeue();
        }

        comp.Activate();
        return comp;
    }

    public void Return(SpawnableObject comp)
    {
        comp.Deactivate();
        _pool.Enqueue(comp);
    }
}
