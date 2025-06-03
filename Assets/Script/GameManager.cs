using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("Player Info")]
    public float hp;
    public float maxHp;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = {};

    [Header("Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public GameObject Hud;
    public GameObject uiResult;
    public GameObject resume;
    public GameObject esc;

    void Awake()
    {
        AudioManager.instance.PlayBgm(true);
        instance = this;
        GameStart();
    }

    public void GameStart()
    {
        hp = maxHp;

        uiLevelUp.Select(0);

        isLive = true;
        Resume();

        AudioManager.instance.PlayBgm(true);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }
    public void GameOut()
    {
        Application.Quit();
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);

        uiResult.SetActive(true);
        //Hud.SetActive(false);

        Stop();

        AudioManager.instance.PlayBgm(false);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(1);
    }

    public void Title()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (!isLive) return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameOver();
        }

        else if (gameTime > 541f && gameTime < 600f)
        {
            if (!GameObject.FindWithTag("Boss")) 
            {
                GameOver();
            }
        }

        if (hp > maxHp)
        {
            hp = maxHp;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isLive)
            {
                resume.SetActive(true);
                esc.SetActive(true);
                Stop();
                Debug.Log("정지");
            }

            else
            {
                resume.SetActive(false);
                esc.SetActive(false);
                Resume();
                Debug.Log("시작");
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            uiLevelUp.Show();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            gameTime = 539f;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            GameOver();
        }
    }

    public void GetExp()
    {
        if (!isLive) return;

        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        resume.SetActive(false);
        esc.SetActive(false);
        isLive = true;
        Time.timeScale = 1;
    }
}
