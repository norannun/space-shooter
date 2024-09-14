using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{

    private Queue<T> _pool = new Queue<T>();
    private GameObject _prefab;
    private Transform _parent;
    private int _expansionSize;

    public ObjectPool(GameObject prefab, int initialSize, int expansionSize, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;
        _expansionSize = expansionSize;

        Expand(initialSize);
    }

    public T Expand(int toAdd)
    {
        T tmp = null;  // Used if there is no objects left

        for (int i = 0; i < toAdd; i++)
        {
            GameObject obj = GameObject.Instantiate(_prefab, _parent);
            T comp = obj.GetComponent<T>();
            if (comp == null)
            {
                Debug.LogError($"ObjectPool: comp is NULL");
            }

            obj.SetActive(false);
            _pool.Enqueue(comp);

            if (i == toAdd - 1)
            {
                tmp = comp;
            }
        }

        return tmp;
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

        comp.gameObject.SetActive(true);
        return comp;
    }

    public void Return(T comp)
    {
        comp.gameObject.SetActive(false);
        _pool.Enqueue(comp);
    }
}
