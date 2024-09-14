using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance { get; private set; }

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

    [SerializeField] private TextMeshProUGUI _highestScoreText;

    private void Start()
    {
        RenderHighestScore();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void RenderHighestScore()
    {
        _highestScoreText.text = $"Highest Score\n<size=26><color=#DC4A35>{PlayerPrefs.GetInt(GlobalValuesManager.Instance.highestScoreKey)}</color></size>";
    }
}
