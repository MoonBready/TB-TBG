using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public Character[] enemyTeam;
    public List<Character> playerTeam = new List<Character>();

    private List<Character> allCharacters = new List<Character>();

    [Header("Components")]
    public Transform[] playerTeamSpawn;
    public Transform[] enemyTeamSpawn;

    [Header("Data")]
    public PlayerPersistentData playerPersistentData;
    public CharacterSet defaultEnemySet;

    public static GameManager instance;
    public static CharacterSet curEnemySet;
    private float totalGameTime;

    public int Scores { get; private set; }
    public event Action<int> OnScoreChanged;

    public float timer;
    public event System.Action<float> OnTimerChanged;
    private bool isTimerOn;
    private float currentTimer;


    public float Timer
    {
        get { return timer; }
    }

    public float TotalGameTime
    {
        get { return totalGameTime; }
    }


    private void Update()
    {
        //CountTime();
        timer += Time.deltaTime;

        totalGameTime += Time.deltaTime;

        OnTimerChanged?.Invoke(timer);
    }

    private void OnEnable()
    {
        Character.onCharacterDeath += OnCharacterKilled;
    }

    private void OnDisable()
    {
        Character.onCharacterDeath -= OnCharacterKilled;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Scores = 0;
            instance = this;
        }
    }

    private void Start()
    {
        if(curEnemySet == null)
        {
            CreateCharacters(playerPersistentData, defaultEnemySet);
        }
        else
        {
            CreateCharacters(playerPersistentData, curEnemySet);
        }

        TurnManager.instance.Begin();
    }

    void CreateCharacters (PlayerPersistentData playerData, CharacterSet enemyTeamSet)
    {
        playerTeam.Clear();
        enemyTeam = new Character[enemyTeamSet.characters.Length];

        int playerSpawnIndex = 0;

        for (int i = 0; i < playerData.characters.Length; i++)
        {
            if (!playerData.characters[i].isDead)
            {
                Character character = CreateCharacter(playerData.characters[i].characterPrefab, playerTeamSpawn[playerSpawnIndex]);
                character.curHp = playerData.characters[i].health;
                playerTeam.Add(character);
                playerSpawnIndex++;
            }
            
        }

        for (int i = 0;i < enemyTeamSet.characters.Length; i++)
        {
            Character character = CreateCharacter(enemyTeamSet.characters[i], enemyTeamSpawn[i]);
            enemyTeam[i] = character;
        }


        allCharacters.AddRange(playerTeam);
        allCharacters.AddRange(enemyTeam);
    }

    Character CreateCharacter (GameObject characterPrefab, Transform spawnPos)
    {
        GameObject obj = Instantiate(characterPrefab, spawnPos.position, spawnPos.rotation);
        return obj.GetComponent<Character>();
    }

     void OnCharacterKilled(Character character)
    {
        allCharacters.Remove(character);

        int playerRemaining = 0;
        int enemiesRemaining = 0;

        for (int i = 0; i < allCharacters.Count; i++)
        {
            if (allCharacters[i].team == Character.Team.Player)
            {
                playerRemaining++;
            }
            else
            {
                enemiesRemaining++;
            }
        }

        if(enemiesRemaining == 0)
        {
            PlayerTeamWins();
            GameManager.instance.AddScore(10);
        }
        else if (playerRemaining == 0)
        {
            EnemyTeamsWins();
        }
    }

    void PlayerTeamWins()
    {
            UpdatePlayerPersistentData();
            Invoke(nameof(LoadMapScene), 1f);
    }

    void EnemyTeamsWins()
    {
        //Game Over
        playerPersistentData.ResetCharacter();
        Invoke(nameof(LoadMapScene), 1f);
        GameManager.instance.Scores.ToString();
        SceneManager.LoadScene("End");
    }

    void UpdatePlayerPersistentData()
    {
        for(int i = 0; i < playerTeam.Count; i++)
        {
            if (playerTeam[i] != null)
            {
                playerPersistentData.characters[i].health = playerTeam[i].curHp;
            }
            else
            {
                playerPersistentData.characters[i].isDead = true;
            }
        }
    }

    internal void AddScore(int points)
    {
        Scores += points;

        OnScoreChanged?.Invoke(Scores);
    }

    void LoadMapScene()
    {
        SceneManager.LoadScene("Map");
    }

}
