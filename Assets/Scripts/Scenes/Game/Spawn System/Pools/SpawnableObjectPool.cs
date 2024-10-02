using System.Collections.Generic;
using UnityEngine;

public class SpawnableObjectPool<T> where T : SpawnableObject
{
    private GameObject _prefab;
    private Transform _parent;
    private int _expansionSize;

    private Queue<T> _pool = new Queue<T>();

    public SpawnableObjectPool(SpawnableObjectPoolConfig config, Transform parent)
    {
        Initialize(config, parent);
    }

    private T Expand(int toAdd)
    {
        T tmp = null;  // Used if there is no objects left

        for (int i = 0; i < toAdd; i++)
        {
            GameObject obj = GameObject.Instantiate(_prefab, _parent.transform);
            T comp = obj.GetComponent<T>();
            if (comp == null)
            {
                Debug.LogError($"ObjectPool: comp is NULL");
            }

            // SpawnableObject deactivates itself during initialization
            _pool.Enqueue(comp);
            if (i == toAdd - 1)
            {
                tmp = comp;
            }
        }

        return tmp;
    }

    private void Initialize<D>(D config, Transform parent) where D : SpawnableObjectPoolConfig
    {
        _prefab = config.prefab;
        _parent = parent;
        _expansionSize = config.expansionSize;

        Expand(config.initialSize);
    }

    public T Get()
    {
        T comp;

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

    public void Return(T comp)
    {
        comp.Deactivate();
        _pool.Enqueue(comp);
    }
}

public class SpawnableObjectPoolConfig : ScriptableObject
{
    public GameObject prefab;
    public int initialSize;
    public int expansionSize;
}
