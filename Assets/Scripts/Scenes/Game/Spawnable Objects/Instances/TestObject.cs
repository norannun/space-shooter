
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
}


[CreateAssetMenu(fileName="TestObject Config", menuName= "Configs/Spawnable Objects/TestObject Config")]
public class TestObjectConfig : SpawnableObjectConfig
{

}
