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
                DataManager.Instance.SpawnObject<Player, PlayerMomento>(playerPrefab, this.transform.position, this.transform.rotation, Vector3.one);
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
