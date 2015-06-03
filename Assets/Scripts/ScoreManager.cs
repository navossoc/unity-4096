using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    private int score = 0;
    private int highScore = 0;

    private Text scoreText;
    private Text highScoreText;

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
        Debug.Log("Score: " + score);

        // If the current score is higher than the high score
        if (score > highScore)
        {
            highScoreText.text = scoreText.text;
            WriteHighScore(score);
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
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        return highScore.ToString();
    }

    private void WriteHighScore(int score)
    {
        highScore = score;
        PlayerPrefs.SetInt(HighScoreKey, score);
    }
}
