using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private PoolableObjectConfig _config;
    private int _expansionSize;

    private Queue<PoolableObject> _pool;

    public void Initialize(PoolableObjectConfig config)
    {
        _pool = new Queue<PoolableObject>();

        _config = config;
        _expansionSize = config.poolExpSize;

        Expand(config.poolInitSize);
    }

    private PoolableObject Expand(int toAdd)
    {
        PoolableObject tmp = null;  // Used if there is no objects left

        for (int i = 0; i < toAdd; i++)
        {
            PoolableObject comp = InitializeObject(_config);

            _pool.Enqueue(comp);
            if (i == toAdd - 1)
            {
                tmp = comp;
            }
        }

        return tmp;
    }

    private PoolableObject InitializeObject(PoolableObjectConfig config)
    {
        GameObject obj = Instantiate(config.prefab, transform);
        PoolableObject comp = obj.GetComponent<PoolableObject>();
        Unclassified.NullCheckComponent(comp);
        comp.Initialize(config);

        return comp;
    }

    public PoolableObject Get()
    {
        PoolableObject comp;

        if (_pool.Count > 0)
        {
            comp = _pool.Dequeue();
        }
        else
        {
            comp = Expand(_expansionSize);
        }

        comp.Activate();
        return comp;
    }

    public void Return(PoolableObject comp)
    {
        comp.Deactivate();
        _pool.Enqueue(comp);
    }
}
