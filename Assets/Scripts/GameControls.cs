using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControls : MonoBehaviour {

    public List<GameObject> spawnPoints = new List<GameObject>();
    public float spawnActivationTime;
    public GameObject[] buttons;
    int counter;
    bool isRdyToAdd;
    bool gameStarted;
    public GameObject gameOverPanel;
    public Text euroAmount;
    public static int euros;
    public long timeSurvived;
    Color[] gunTurretColor = new Color[3];
 
    //Color buttonBaseColor;
    public BuildSpot build;
    public MeshRenderer [] gunTurretUI;
    public MeshRenderer [] missileTurretUI;
    public MeshRenderer [] slowTurretUI;
    public MeshRenderer [] nukeTurretUI;
    public static int zombieCount;

    private void Start()
    {
        zombieCount = 0;  
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();   
        gameOverPanel.SetActive(false); 
        //buttonBaseColor = buttons[0].GetComponent<Image>().color;
        euros = 100;
        counter = 0;
        gameStarted = false;
        isRdyToAdd = true;
        
        SignIn();

        for (int i = 0; i < 3; i++)
        {
            gunTurretColor[i] = gunTurretUI[i].material.color;
        }
    }

    void SignIn()
    {
        Social.localUser.Authenticate(success => { });
    }

    #region Achievements
    public static void UnlockAchievement(string id)
    {
        Social.ReportProgress(id, 100, success => { });
    }

    public static void IncrementAchievement(string id, int stepsToIncrement)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
    }
    
    public void ShowAchievementsUI()
    {
        
        Social.ShowAchievementsUI();
      
    }
    #endregion /Achievements

    #region Leaderboards

    public static void AddScoreToLeaderboard(string leaderboardId, long score)
    {
        Social.ReportScore(score, leaderboardId, success => { });
    }

    public void ShowLeaderboardsUI()
    {
        Social.ShowLeaderboardUI();
      
    }

    #endregion /Leaderboards

    public void GameIsOver()
    {
        gameOverPanel.SetActive(true);
        UnlockAchievement(GPGSIds.achievement_zombie_epidemic_always_wins);
        timeSurvived = (long)(Time.timeSinceLevelLoad * 1000);
        gameOverPanel.transform.GetChild(2).GetComponent<Text>().text = "GAME OVER\n You Survived : " + timeSurvived + "seconds!";
        AddScoreToLeaderboard(GPGSIds.leaderboard_artdleaderboard, timeSurvived);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        timeSurvived = 0;
        zombieCount = 0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        
        if(build.selectedBuilding == null)
        {
            for (int i = 0; i < 3; i++)
            {
                if (gunTurretUI[i].material.color == Color.green)
                {
                    gunTurretUI[i].material.color = gunTurretColor[i];
                }
                if (missileTurretUI[i].material.color == Color.green)
                {
                    missileTurretUI[i].material.color = gunTurretColor[i];
                }
                if (slowTurretUI[i].material.color == Color.green)
                {
                    slowTurretUI[i].material.color = gunTurretColor[i];
                }
                if (nukeTurretUI[i].material.color == Color.green)
                {
                    nukeTurretUI[i].material.color = gunTurretColor[i];
                }
            }     
        }
        euroAmount.text = "€ : " + euros;
        if (gameStarted)
        {
            
            if (isRdyToAdd && counter <= spawnPoints.Count)
            {

                isRdyToAdd = false;    
                spawnPoints[counter].SetActive(true);
                counter++;
                StartCoroutine(Increment());

            }
        }
    }

    public void BuildingSelection1()
    { 

        build.SetBuilding(0);

        for (int i = 0; i < 3; i++)
        {
            if (gunTurretUI[i].material.color != Color.green)
            {
                gunTurretUI[i].material.color = Color.green;
            }
            if (missileTurretUI[i].material.color == Color.green)
            {
                missileTurretUI[i].material.color = gunTurretColor[i];
            }
            if (slowTurretUI[i].material.color == Color.green)
            {
                slowTurretUI[i].material.color = gunTurretColor[i];
            }
            if (nukeTurretUI[i].material.color == Color.green)
            {
                nukeTurretUI[i].material.color = gunTurretColor[i];
            }
        }     
    }
    public void BuildingSelection2()
    {

        build.SetBuilding(1);

        for (int i = 0; i < 3; i++)
        {
            if (missileTurretUI[i].material.color != Color.green)
            {
                missileTurretUI[i].material.color = Color.green;
            }
            if (gunTurretUI[i].material.color == Color.green)
            {
                gunTurretUI[i].material.color = gunTurretColor[i];
            }
            if (slowTurretUI[i].material.color == Color.green)
            {
                slowTurretUI[i].material.color = gunTurretColor[i];
            }
            if (nukeTurretUI[i].material.color == Color.green)
            {
                nukeTurretUI[i].material.color = gunTurretColor[i];
            }
        }
        
    }
    public void BuildingSelection3()
    {

        build.SetBuilding(2);

        for (int i = 0; i < 3; i++)
        {
            if (slowTurretUI[i].material.color != Color.green)
            {
                slowTurretUI[i].material.color = Color.green;
            }
            if (gunTurretUI[i].material.color == Color.green)
            {
                gunTurretUI[i].material.color = gunTurretColor[i];
            }
            if (missileTurretUI[i].material.color == Color.green)
            {
                missileTurretUI[i].material.color = gunTurretColor[i];
            }
            if (nukeTurretUI[i].material.color == Color.green)
            {
                nukeTurretUI[i].material.color = gunTurretColor[i];
            }
        }
       
    }
    public void BuildingSelection4()
    {

        build.SetBuilding(3);

        for (int i = 0; i < 3; i++)
        {
            if (nukeTurretUI[i].material.color != Color.green)
            {
                nukeTurretUI[i].material.color = Color.green;
            }
            if (gunTurretUI[i].material.color == Color.green)
            {
                gunTurretUI[i].material.color = gunTurretColor[i];
            }
            if (missileTurretUI[i].material.color == Color.green)
            {
                missileTurretUI[i].material.color = gunTurretColor[i];
            }
            if (slowTurretUI[i].material.color == Color.green)
            {
                slowTurretUI[i].material.color = gunTurretColor[i];
            }

        }
       
    }

    public void StartGame()
    {
       
        Time.timeScale = 1;
        EnemyZombie[] enemys = FindObjectsOfType<EnemyZombie>();
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].GameContinue();
        }

        if (!gameStarted)
            UnlockAchievement(GPGSIds.achievement_new_game);

        gameStarted = true;
       
    }

    IEnumerator Increment()
    {
        
        yield return new WaitForSeconds(spawnActivationTime);

        isRdyToAdd = true;
        
    }

    public void PauseGame()
    {
          
        Time.timeScale = 0;

        EnemyZombie[] enemys = FindObjectsOfType<EnemyZombie>();
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].GamePaused();
        }

    }

}
