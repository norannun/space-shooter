using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Inspector
    [SerializeField] private GameObject _shield;
    [SerializeField] private GameObject _rightDamage, _leftDamage;

    // Numbers
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score = 0;
    [SerializeField] private int _scoreLow = 5;
    [SerializeField] private int _scoreHigh = 21;
    [SerializeField] private float _speed = 5.13f;
    [SerializeField] private float _fireRate = 0.15f;
    [SerializeField] private float _speedModifier = 2.0f;
    [SerializeField] private Vector2 _startingPos = new Vector2(0, -3);
    [SerializeField] private float _singleShotYOffset = 1.044f;
    [SerializeField] private float _tripleShotXOffset = 0.78f;
    [SerializeField] private float _tripleShotYOffset = -0.33f;

    private float _fireLimit = 0.0f;
    private float _boundXLeft;
    private float _boundXRight;
    private float _boundYUp;
    private float _boundYDown;

    // Conditions
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private bool _isShieldActive = false;

    // Components
    private BoxCollider2D _boxCollider2D;

    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        if (_boxCollider2D == null)
        {
            Debug.LogError("Player: _boxCollider2D is NULL");
            enabled = false;
            return;
        }

        transform.position = _startingPos;

        _boundXLeft = GlobalValuesManager.Instance.LeftBoundary - _boxCollider2D.bounds.size.x / 2;
        _boundXRight = GlobalValuesManager.Instance.RightBoundary + _boxCollider2D.bounds.size.x / 2;
        _boundYUp = 0;
        _boundYDown = GlobalValuesManager.Instance.BottomBoundary + _boxCollider2D.bounds.size.y / 2;
    }

    void Update()
    {
        CalculateMovement();
        Fire();
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
            transform.position = new Vector3(_boundXLeft, transform.position.y, 0);
        }
        else if (transform.position.x < _boundXLeft)
        {
            transform.position = new Vector3(_boundXRight, transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _boundYDown, _boundYUp));
    }

    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _fireLimit)
        {
            _fireLimit = Time.time + _fireRate;

            // Shoot 1 or 3 lasers
            if (_isTripleShotActive)
            {
                ProjectilePool.Instance.SpawnProjectile(transform.position, xOffset: -_tripleShotXOffset,
                                                                            yOffset: _tripleShotYOffset);
                ProjectilePool.Instance.SpawnProjectile(transform.position, yOffset: _singleShotYOffset);
                ProjectilePool.Instance.SpawnProjectile(transform.position, xOffset: _tripleShotXOffset,
                                                                            yOffset: _tripleShotYOffset);
            }
            else
            {
                ProjectilePool.Instance.SpawnProjectile(transform.position, yOffset: _singleShotYOffset);
            }

            AudioSourcePool.Instance.PlayAudio("Laser Shot", transform.position);
        }
    }

    public void Damage()
    {
        // Ignore damage and deactivate shield
        if (_isShieldActive)
        {
            DeactivateShield();
            return;
        }

        _lives--;

        // Damage visulization
        if (_lives == 2)
        {
            _rightDamage.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftDamage.SetActive(true);
        }

        HUDController.Instance.UpdateLives(_lives);

        if (_lives < 1)
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

    // Poweup Work
    private void OnEnable()
    {
        Powerup.OnPowerupCollected += ActivatePowerup;
    }

    private void OnDisable()
    {
        Powerup.OnPowerupCollected -= ActivatePowerup;
    }

    private void ActivatePowerup(Powerup.Type type, float duration)
    {
        switch (type)
        {
            case Powerup.Type.TripleShot: ActivateTripleShot(duration); break;
            case Powerup.Type.Speed: ActivateSpeed(duration); break;
            case Powerup.Type.Shield: ActivateShield(); break;
        }
    }

    private void ActivateTripleShot(float duration)
    {
        _isTripleShotActive = true;
        StartCoroutine(DeactivateTripleShot(duration));

    }

    private void ActivateSpeed(float duration)
    {
        _speed *= _speedModifier;
        StartCoroutine(DeactivateSpeed(duration));
    }

    private void ActivateShield()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    IEnumerator DeactivateTripleShot(float duration)
    {
        yield return new WaitForSeconds(duration);
        _isTripleShotActive = false;
    }

    IEnumerator DeactivateSpeed(float duration)
    {
        yield return new WaitForSeconds(duration);
        _speed /= _speedModifier;
    }

    public void DeactivateShield()
    {
        _isShieldActive = false;
        _shield.SetActive(false);
        return;
    }
}
