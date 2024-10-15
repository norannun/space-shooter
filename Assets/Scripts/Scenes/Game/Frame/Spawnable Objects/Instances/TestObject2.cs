using UnityEngine;

public class TestObject2 : SpawnableObject
{
    public override void Initialize(PoolableObjectConfig config)
    {
        TestObject2Config testObject2Config = config as TestObject2Config;

        base.Initialize(config);
    }
}

[CreateAssetMenu(fileName = "TestObject2 Config", menuName = "Configs/Spawnable Objects/TestObject2 Config")]
public class TestObject2Config : SpawnableObjectConfig
{

}
