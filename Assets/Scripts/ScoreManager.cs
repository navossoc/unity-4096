using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    /*
     * Fields
     */

    private const string HighScoreKey = "HighScore";

    private Text scoreText;
    private Text highScoreText;

    /*
     * Properties
     */

    public int Score { get; private set; }

    public int HighScore { get; private set; }

    /*
     * Methods
     */

    public void AddPoints(int points)
    {
        Score += points;
        scoreText.text = Score.ToString();
        ////Debug.Log("Score: " + Score);

        // If the current score is higher than the high score
        if (Score > HighScore)
        {
            highScoreText.text = scoreText.text;
            WriteHighScore(Score);
        }
    }

    // Use this for initialization
    private void Start()
    {
        scoreText = GameObject.Find("Score/Value").GetComponent<Text>();
        highScoreText = GameObject.Find("High Score/Value").GetComponent<Text>();

        // Read high score from player prefs
        highScoreText.text = ReadHighScore();
        scoreText.text = "0";
    }

    private string ReadHighScore()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        return HighScore.ToString();
    }

    private void WriteHighScore(int score)
    {
        HighScore = score;
        PlayerPrefs.SetInt(HighScoreKey, score);
    }
}
