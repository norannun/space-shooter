using UnityEngine;

public class Player : MonoBehaviour
{
    // Configuration
    [SerializeField] private PlayerConfig config;
    [SerializeField] private NovaShotConfig _novaShotConfig;
    private int _health;
    public int CollisionDamage { get; private set; }
    private int _scoreLow;
    private int _scoreHigh;
    private float _speed;
    private float _fireRate;
    private Vector2 _startingPos;
    private float _projectileOffsetY;
    private float _projectileOffsetX;

    private int _score;
    private float _fireLimit;
    private float _boundXLeft;
    private float _boundXRight;
    private float _boundYUp;
    private float _boundYDown;

    // Components
    private BoxCollider2D _boxCollider2D;

    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        Unclassified.NullCheckComponent(_boxCollider2D);

        _health = config.health;
        CollisionDamage = config.collisionDamage;
        _scoreLow = config.scoreLow;
        _scoreHigh = config.scoreHigh;
        _speed = config.speed;
        _fireRate = config.fireRate;
        _projectileOffsetY = config.projectileOffsetY;
        _projectileOffsetX = config.projectileOffsetX;
        transform.position = config.startingPosition;

        _score = 0;
        _fireLimit = 0.0f;

        _boundXLeft = GlobalValuesManager.Instance.LeftBoundary - _boxCollider2D.bounds.size.x / 2;
        _boundXRight = GlobalValuesManager.Instance.RightBoundary + _boxCollider2D.bounds.size.x / 2;
        _boundYUp = 0;
        _boundYDown = GlobalValuesManager.Instance.BottomBoundary + _boxCollider2D.bounds.size.y / 2;
    }

    void Update()
    {
        CalculateMovement();
        LaunchProjectile();
    }

    void CalculateMovement()
    {
        // Translation
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * _speed);

        // Bounds
        if (transform.position.x > _boundXRight)
        {
            transform.position = new Vector2(_boundXLeft, transform.position.y);
        }
        else if (transform.position.x < _boundXLeft)
        {
            transform.position = new Vector2(_boundXRight, transform.position.y);
        }

        transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, _boundYDown, _boundYUp));
    }

    void LaunchProjectile()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _fireLimit)
        {
            _fireLimit = Time.time + _fireRate;

            Vector2 projectilePos = new Vector2(transform.position.x + _projectileOffsetX, transform.position.y + _projectileOffsetY);
            ProjectileManager.Instance.SpawnProjectile(_novaShotConfig.projectileName, projectilePos);

            AudioSourcePool.Instance.PlayAudio(GlobalValuesManager.Instance.projectileAudioName, transform.position);
        }
    }

    public void ReceiveDamage(int damage)
    {
        _health -= damage;

        if (_health < 1)
        {
            GlobalValuesManager.Instance.UpdateHighestScore(_score);
            GlobalValuesManager.Instance.state = GlobalValuesManager.GameState.Over;
            HUDController.Instance.GameOverSequence();
            AudioSourcePool.Instance.PlayAudio("Explosion", transform.position);
            gameObject.SetActive(false);
        }
    }

    public void AddScore()
    {
        _score += Random.Range(_scoreLow, _scoreHigh);
        HUDController.Instance.UpdateScore(_score);
    }
}

[CreateAssetMenu(fileName = "Player Config", menuName = "Configs/Player")]
public class PlayerConfig : ScriptableObject
{
    public int health;
    public float speed;
    public int collisionDamage;
    public Vector2 startingPosition;
    public float fireRate;
    public float projectileOffsetY;
    public int projectileOffsetX;
    public int scoreLow;
    public int scoreHigh;
    public GameObject novaShot;
}
