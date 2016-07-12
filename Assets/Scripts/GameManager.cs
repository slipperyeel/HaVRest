using UnityEngine;
using System.Collections;

/// <summary>
/// Eventually this will manage the game.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    private FarmingGame game;
    public FarmingGame Game { get { return game; } }

	void Awake ()
    {
        game = new FarmingGame();
        game.Initialize();
        game.Serialize();
        game.Deserialize();
	}
}
