using UnityEngine;
using System.Collections;

/// <summary>
/// Eventually this will manage the game.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    private FarmingGame game;
    public FarmingGame Game { get { return game; } }

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private Transform spawnPoint;

    public bool TestSaveTrigger = false;

	void Start ()
    {
        game = new FarmingGame();
        game.Initialize();
        game.Serialize();
        game.Deserialize();

        DataManager.Instance.LoadGameData(() =>
        {
            if (DataManager.Instance.IsFirstBoot)
            {
                Debug.Log("Is First Boot.");
                DataManager.Instance.SpawnObject<Player, PlayerMomento>(playerPrefab, spawnPoint.position, spawnPoint.rotation, Vector3.one);
            }
        });
    }

    void Update()
    {
        if(TestSaveTrigger)
        {
            DataManager.Instance.SaveGameData();
        }
    }
}
