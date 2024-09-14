using System.Collections;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// Events
    public delegate void LevelEndHandle();
    public static event LevelEndHandle OnLevelEnd;

    // Numbers
    [SerializeField] private Vector2 _enemySpawnDelayRange = new Vector2(2.0f, 5.0f);
    [SerializeField] private Vector2 _powerupSpawnDelayRange = new  Vector2(6.0f, 15.5f);
    [SerializeField] private int _enemiesOnScreenMax = 20;
    private int _currentLevel = 1;
    private int _enemiesMultiplier = 5;
    private int _enemiesSpawned = 0;
    private float _levelStartDelay = 3.0f;
    public int _enemiesDestroyed = 0;

    private void OnEnable()
    {
        Asteroid.OnAsteroidDeath += StartLevel;
    }

    private void OnDisable()
    {
        Asteroid.OnAsteroidDeath -= StartLevel;
    }

    private void StartLevel()
    {
        HUDController.Instance.UpdateLevel(_currentLevel);

        StartCoroutine(EnemySpawnRoutine(_currentLevel * _enemiesMultiplier));
        StartCoroutine(PowerupSpawnRoutine());
    }

    IEnumerator EnemySpawnRoutine(int quantity)
    {
        yield return new WaitForSeconds(_levelStartDelay);

        // Spawn enemies until player destroy all the enemies
        while (_enemiesDestroyed < _currentLevel * _enemiesMultiplier)
        {
            if (Unclassified.GetActiveChildrenCount(EnemyPool.Instance.gameObject) < _enemiesOnScreenMax && _enemiesSpawned < _currentLevel * _enemiesMultiplier)
            {
                StartCoroutine(SpawnPositionManager.Instance.GetPosition(SpawnEnemy));
                _enemiesSpawned++;

                yield return new WaitForSeconds(Random.Range(_enemySpawnDelayRange.x, _enemySpawnDelayRange.y));
            }
            else
            {
                yield return null;
            }
        }

        _enemiesSpawned = 0;
        _enemiesDestroyed = 0;
        _currentLevel++;
        StopAllCoroutines();
        OnLevelEnd?.Invoke();
    }

    IEnumerator PowerupSpawnRoutine()
    {
        while (_enemiesDestroyed < _currentLevel * _enemiesMultiplier)
        {

            yield return new WaitForSeconds(Random.Range(_powerupSpawnDelayRange.x, _powerupSpawnDelayRange.y));

            StartCoroutine(SpawnPositionManager.Instance.GetPosition(SpawnPowerup));            
        }
    }

    private void SpawnEnemy(Vector2 position)
    {
        GameObject enemy = EnemyPool.Instance.GetEnemy().gameObject;
        enemy.transform.position = position;
    }

    private void SpawnPowerup(Vector2 position)
    {
        GameObject powerup = PowerupPool.Instance.GetRandomPowerup();
        powerup.transform.position = position;
    }

    public void IncreaseEnemiesDestroyed()
    {
        _enemiesDestroyed++;
    }
}
