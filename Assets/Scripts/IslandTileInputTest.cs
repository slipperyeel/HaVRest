using UnityEngine;
using System.Collections;

public class IslandTileInputTest : MonoBehaviour
{
    private Ray r;
    private RaycastHit hitInfo;
    private GameObject hoverCube;
    private Tile tileHovered;
    private Vector3 centerOffset = new Vector3(0.5f, 0.5f, 0.5f);
    private GameObject currentOccupyingObject = null;
    private string currentFormattedString;

    void Start()
    {
        hoverCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    void Update()
    {
        r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out hitInfo))
        {
            tileHovered = hitInfo.collider.gameObject.GetComponent<Tile>();
            if (tileHovered != null)
            {
                // hovering over a tile
                currentOccupyingObject = tileHovered.OccupyingObject;
                hoverCube.transform.position = tileHovered.WorldPosition + centerOffset;

                string objName = "Unoccupied";
                if (currentOccupyingObject != null)
                {
                    objName = currentOccupyingObject.name;
                }

                currentFormattedString = string.Format("Tile[{0},{1}] -> {2}", tileHovered.IslandPosition.x, tileHovered.IslandPosition.y, objName);
            }
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 30), currentFormattedString);
    }
}