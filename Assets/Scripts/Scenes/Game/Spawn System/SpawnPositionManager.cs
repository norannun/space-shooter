using UnityEngine;


public class SpawnPositionManager : MonoBehaviour
{
    public static SpawnPositionManager Instance { get; private set; }
    [SerializeField] private SpawnManagerConfig _config;

    private Vector2 _leftBottomPoint;
    private Vector2 _rightTopPoint;

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

    private void Start()
    {
        _leftBottomPoint = new Vector2(GlobalValuesManager.Instance.LeftBoundary, GlobalValuesManager.Instance.BottomBoundary);
        _rightTopPoint = new Vector2(GlobalValuesManager.Instance.RightBoundary, GlobalValuesManager.Instance.TopBoundary + _config.spawnAreaHeight);
    }

    public Vector2 GetRandomPosition(Vector2 objSize)
    {
        float posX = Random.Range(_leftBottomPoint.x + objSize.x, _rightTopPoint.x - objSize.x);
        float posY = Random.Range(_leftBottomPoint.y + objSize.y, _rightTopPoint.y - objSize.y);

        return new Vector2(posX, posY);
    }
}
