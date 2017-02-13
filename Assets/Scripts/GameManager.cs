using UnityEngine;
using System.Collections;
using HVRTime;

/// <summary>
/// Eventually this will manage the game.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    private FarmingGame mGame;
    public FarmingGame Game { get { return mGame; } }

    [SerializeField]
    private Player mPlayer;
    public Player Player
    {
        get { return mPlayer; }
        set { mPlayer = value; }
    }

    [SerializeField]
    private GameObject mPlayerPrefab;

    [SerializeField]
    private Transform mSpawnPoint;

    // World Chunks!
    [SerializeField]
    private WorldChunkCollection mChunkCollection;
    public WorldChunkCollection ChunkCollection { get { return mChunkCollection; } }

    public float WorldChunkMinViewDistance = 100.0f;

    public bool TestSaveTrigger = false;

	public void Awake ()
    {
        mGame = new FarmingGame();
        mGame.Initialize();
        mGame.Serialize();
        mGame.Deserialize();

        // Initialize Managers
        DataManager.Instance.Init();
        TimeManager.Instance.Init();

        DataManager.Instance.LoadGameData(() =>
        {
            if (DataManager.Instance.IsFirstBoot)
            {
                Debug.Log("Is First Boot.");
                mPlayer = DataManager.Instance.SpawnObject<Player, PlayerMomento>(mPlayerPrefab, mSpawnPoint.position, mSpawnPoint.rotation, Vector3.one);
                mPlayer.InitPlayer();
            }
        });
    }

    void Update()
    {
        //if(TestSaveTrigger)
        if (Input.GetKeyUp("space"))
        {
            DataManager.Instance.SaveGameData();
        }
    }
}
