using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public SpawnManager Instance { get; private set; }

    [SerializeField] TestObjectConfig testObjectConfig;
    [SerializeField] TestObject2Config testObject2Confg;

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
            if (SpawnableObjectManager.Instance == null)
            {
                Debug.Log("SpawnableObjectManager is NULL");
            }
            SpawnableObject obj = SpawnableObjectManager.Instance.Get(testObjectConfig.spawnableObjectName);
            Vector2 position = SpawnPositionManager.Instance.GetRandomPosition(obj.Size);
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            SpawnableObject obj = SpawnableObjectManager.Instance.Get(testObjectConfig.spawnableObjectName);
            Vector2 position = SpawnPositionManager.Instance.GetRandomPosition(obj.Size);
        }
    }
}

[CreateAssetMenu(fileName = "SpawnManager Config", menuName = "Configs/Spawn System/SpawnManager")]
public class SpawnManagerConfig : ScriptableObject
{
    public int spawnAreaHeight;
}