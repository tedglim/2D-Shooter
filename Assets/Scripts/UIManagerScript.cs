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
    private Text _waveStatusText;

    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private GameObject[] _shieldStrength;

    [SerializeField]
    private Color _shieldColorOff;

    [SerializeField]
    private Color _shieldColorOn;
    [SerializeField]
    private Image _thrusterBar;
    [SerializeField]
    private Text _ammoCount;


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
        if(!_gameManager.isGameOver)
        {
            _scoreText.text = "Score: " + score.ToString();
        }
    }

    public void UpdateLivesImg(int lives)
    {
        _livesImg.sprite = _livesSprites[lives];
        _lives = lives;
    }

    public void UpdateShield(int shieldLives)
    {
        if (shieldLives == 3)
        {
            for (int i = 0; i < _shieldStrength.Length; i++)
            {
                _shieldStrength[i].GetComponent<Image>().color = _shieldColorOn;
            }
        }
        else
        {
            _shieldStrength[shieldLives].GetComponent<Image>().color =
                _shieldColorOff;
        }
    }

    public void UpdateThrusters(float currThrusters, float totalThrusters)
    {
        _thrusterBar.fillAmount = currThrusters / totalThrusters;
    }

    public void UpdateAmmoCount(int ammo, int total)
    {
        _ammoCount.text = ammo.ToString() + "/" + total.ToString();
    }

    public void UpdateWaveStatus(bool empty, bool start, int num)
    {
        if(empty)
        {
            _waveStatusText.text = "";
            return;
        }
        if(start)
        {
            _waveStatusText.text = "Wave " + num.ToString();
        } else 
        {
            _waveStatusText.text = "Wave " + num.ToString() + " Complete";
        }
    }

    public void UpdateWaveEndStatus()
    {
        _waveStatusText.text = "All Waves Completed";
    }

    public void DisplayGameOver()
    {
        _gameManager.GameOver();
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
