
using UnityEngine;

public class TestObject : SpawnableObject
{
    [SerializeField] private TestObjectConfig config;

    protected void Awake()
    {
        Initialize(config);
    }

    protected override void Initialize<TestObjectConfig>(TestObjectConfig config)
    {
        base.Initialize(config);
    }

    protected override void ReceiveDamage(int damage)
    {
        Health -= damage;

        if (Health < 1)
        {
            TestPool.Instance.ReturnObject(this);
        }
    }
}


[CreateAssetMenu(fileName="TestObject Config", menuName= "Configs/Spawnable Objects/TestObject Config")]
public class TestObjectConfig : SpawnableObjectConfig
{

}
