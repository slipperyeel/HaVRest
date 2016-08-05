using UnityEngine;
using System.Collections;

public class IslandTileInputTest : MonoBehaviour
{
    private GameObject mDebugCube;
    private IslandData mCurrentIsland;
    private Vector2 mCubeLocation;
    private Vector3 mCenterOffset = new Vector3(0.5f, 0.5f, 0.5f);
    private Tile mPreviousTile;

    void Start()
    {
        WorldGenerator.Instance.OnIslandReady += OnIslandReady;
    }

    void OnDestroy()
    {
        WorldGenerator.Instance.OnIslandReady -= OnIslandReady;
    }

    private void OnIslandReady(object sender, System.EventArgs e)
    {
        // get island
        mCurrentIsland = WorldGenerator.Instance.CurrentIsland;
        mDebugCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // set up cube
        mCubeLocation = new Vector2(25f, 25f);
        MoveCube(0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCube(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCube(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveCube(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCube(0, -1);
        }
    }

    private void MoveCube(int xOffset, int zOffset)
    {
        int nextX = (int)mCubeLocation.x + xOffset;
        int nextY = (int)mCubeLocation.y + zOffset;

        Tile nextTile = mCurrentIsland.GetTile(nextX, nextY);
        if (nextTile.OccupyingObject == null)
        {
            // empty the previous tile
            if (mPreviousTile != null)
            {
                mPreviousTile.SetOccupyingObject(null);
            }

            // move to the next
            mCubeLocation.x = nextX;
            mCubeLocation.y = nextY;

            // occupy new one
            mDebugCube.transform.position = nextTile.TerrainPosition + mCenterOffset;
            nextTile.SetOccupyingObject(mDebugCube);

            Debug.LogFormat("You now occupy {0}.", mCubeLocation);

            mPreviousTile = nextTile;
        }
        else
        {
            Debug.LogWarningFormat("Next tile is occupied by {0}", nextTile.OccupyingObject.name);
        }
    }
}