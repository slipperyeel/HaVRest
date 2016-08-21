using UnityEngine;
using System.Collections;

/// <summary>
/// Eventually this will manage the game.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    private FarmingGame mGame;
    public FarmingGame Game { get { return mGame; } }

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

	void Start ()
    {
        mGame = new FarmingGame();
        mGame.Initialize();
        mGame.Serialize();
        mGame.Deserialize();

        DataManager.Instance.LoadGameData(() =>
        {
            if (DataManager.Instance.IsFirstBoot)
            {
                Debug.Log("Is First Boot.");
                DataManager.Instance.SpawnObject<Player, PlayerMomento>(mPlayerPrefab, mSpawnPoint.position, mSpawnPoint.rotation, Vector3.one);
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
