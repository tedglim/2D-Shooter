using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private Sprite[] _livesSprites;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartText;

    private int _lives;

    private GameManagerScript _gameManager;

    void Start()
    {
        _scoreText.text = "Score: 0";
        _livesImg.sprite = _livesSprites[_livesSprites.Length - 1];
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _lives = _livesSprites.Length - 1;
        _gameManager =
            GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateLivesImg(int lives)
    {
        _livesImg.sprite = _livesSprites[lives];
        _lives = lives;
    }

    public void DisplayGameOver()
    {
        _gameManager.GameOVer();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
