using UnityEngine;

public class GlobalValuesManager : MonoBehaviour
{
    public static GlobalValuesManager Instance { get; private set; }
    public float TopBoundary { get; private set; }
    public float BottomBoundary { get; private set; }
    public float LeftBoundary { get; private set; }
    public float RightBoundary { get; private set; }
    public float ScreenHeight { get; private set; }
    public float ScreenWidth { get; private set; }
    public Vector2 BadPosiiton { get; private set; } = new Vector2(54, 67);
    public string highestScoreKey = "highest score";


    public enum GameState { On, Over };
    public GameState state = GameState.On;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CalculateBounds();

            if (!PlayerPrefs.HasKey(highestScoreKey))
            {
                PlayerPrefs.SetInt(highestScoreKey, 0);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CalculateBounds()
    {
        TopBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        BottomBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        LeftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        RightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        ScreenHeight = TopBoundary - BottomBoundary;
        ScreenWidth = RightBoundary - LeftBoundary;
    }

    public void UpdateHighestScore(int score)
    {
        if (score > PlayerPrefs.GetInt(highestScoreKey))
        {
            PlayerPrefs.SetInt(highestScoreKey, score);
            PlayerPrefs.Save();
        }
    }
}
