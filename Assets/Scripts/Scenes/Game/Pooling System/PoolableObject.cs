using UnityEngine;

public abstract class PoolableObject : MonoBehaviour
{
    public string Name { get; protected set; }

    public virtual void Initialize(PoolableObjectConfig config)
    {
        Name = config.objectName;

        Deactivate();
    }

    // Activate & Deactivate define how will object behave on its appearance and disappearance
    public abstract void Activate();

    public abstract void Deactivate();
}

public abstract class PoolableObjectConfig : ScriptableObject
{
    public GameObject prefab;
    public string objectName;
    public int poolInitSize;
    public int poolExpSize;
}
