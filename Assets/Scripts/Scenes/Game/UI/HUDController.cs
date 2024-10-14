using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance { get; private set; }

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

    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _levelText;
    [SerializeField]
    private TextMeshProUGUI _gameOverText;
    [SerializeField]
    private float _flickeringRate = 0.5f;

    void Start()
    {
        _scoreText.text = $"Score: 0";
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = $"Score: {playerScore}";
    }

    public void UpdateLevel(int level)
    {
        _levelText.text = level.ToString();
    }

    public void GameOverSequence()
    {
        StartCoroutine(GOFlickerRoutine());
        StartCoroutine(WaitForRestartRoutine());
        // "Press 'R' key to restart the level"
        _gameOverText.gameObject.SetActive(true);
        _gameOverText.transform.GetChild(0).gameObject.SetActive(true);
    }

    IEnumerator GOFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(_flickeringRate);
            _gameOverText.text = "";
            yield return new WaitForSeconds(_flickeringRate);
        }
    }

    // Wait for 'R' input and restart the scene
    IEnumerator WaitForRestartRoutine()
    {
        while (GlobalValuesManager.Instance.state == GlobalValuesManager.GameState.Over)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                GlobalValuesManager.Instance.state = GlobalValuesManager.GameState.On;
            }

            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
