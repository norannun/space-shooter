using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public SpawnManager Instance { get; private set; }

    [SerializeField] TestObjectConfig testObjectConfig;
    [SerializeField] TestObject2Config testObject2Config;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        TestSpawn();
    }

    private void TestSpawn()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            PoolableObject obj = SpawnableObjectManager.Instance.GetObject(testObjectConfig.objectName);
            SpawnableObject newObj = obj as SpawnableObject;
            newObj.transform.position = SpawnPositionManager.Instance.GetRandomPosition(newObj.Size);
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            PoolableObject obj = SpawnableObjectManager.Instance.GetObject(testObject2Config.objectName);
            SpawnableObject newObj = obj as SpawnableObject;
            Vector2 position = SpawnPositionManager.Instance.GetRandomPosition(newObj.Size);
        }
    }
}

[CreateAssetMenu(fileName = "SpawnManager Config", menuName = "Configs/Spawn System/SpawnManager")]
public class SpawnManagerConfig : ScriptableObject
{
    public int spawnAreaHeight;
}