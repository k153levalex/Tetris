using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject panelGameOver;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textRecord;

    void Start()
    {
        panelGameOver.SetActive(false);
    }

    public void ShowPanel(int score, int record)
    {
        Time.timeScale = 0f;
        panelGameOver.SetActive(true);
        textScore.text = $"SCORE: {score}";
        textRecord.text = $"RECORD: {record}";
    }

    public void OnClickButtonRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void OnClickButtonMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
