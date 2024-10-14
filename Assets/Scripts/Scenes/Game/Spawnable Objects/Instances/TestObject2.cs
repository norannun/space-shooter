using UnityEngine;

public class TestObject2 : SpawnableObject
{
    [SerializeField] private TestObject2Config config;

    protected void Awake()
    {
        Initialize(config);
    }

    protected override void Initialize<TestObject2Config>(TestObject2Config config)
    {
        base.Initialize(config);
    }

    protected override void ReceiveDamage(int damage)
    {
        Health -= damage;

        if (Health < 1)
        {
            TestPool2.Instance.ReturnObject(this);
        }
    }
}

[CreateAssetMenu(fileName = "TestObject2 Config", menuName = "Configs/Spawnable Objects/TestObject2 Config")]
public class TestObject2Config : SpawnableObjectConfig
{

}
