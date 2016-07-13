using UnityEngine;
using System.Collections;
using UnityEditor;

public class PuzzleEditorTool : EditorWindow
{
    private string kDirectoryPath = "Assets/Editor/Data/";
    private string kAssetName = "PuzzleCollection";

    private PuzzleCollectionData mPuzzleData = null;
    private HVRPuzzle.Puzzle mCurrentPuzzle = null;
    private bool mInProgress = false;
    private int mExpandedIndex = -1;

    [MenuItem("Tools/Puzzle Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PuzzleEditorTool));
    }

    void Awake()
    {
        Load();
    }

    void OnGUI()
    {
        GUILayout.Label("- Puzzle Collection -");

        DrawLine();

        if (mPuzzleData != null && mPuzzleData.PuzzleCollection.Count > 0)
        {
            for (int i = 0; i < mPuzzleData.PuzzleCollection.Count; ++i)
            {
                ShowSavedPuzzle(i);
            }
        }
        else
        {
            GUILayout.Label("There are currently no puzzles.");
        }

        GUILayout.Space(20);

        // create new puzzle
        if (!mInProgress && GUILayout.Button("Add New Puzzle"))
        {
            mInProgress = true;
        }

        // Show the puzzle creation portion
        if (mInProgress)
        {
            ShowCreatePuzzle();
        }
    }

    // Show the options for creating a new puzzle
    private void ShowCreatePuzzle()
    {
        GUILayout.Label("> New Puzzle");
        if (mCurrentPuzzle == null)
        {
            mCurrentPuzzle = new HVRPuzzle.Puzzle();
        }

        // set the fields
        ModifyFields(mCurrentPuzzle);

        if (GUILayout.Button("Discard Puzzle"))
        {
            mInProgress = false;
            mCurrentPuzzle = null;
        }

        if (GUILayout.Button("Save Puzzle"))
        {
            mPuzzleData.Add(mCurrentPuzzle);
            Save();
            mCurrentPuzzle = null;
            mInProgress = false;
        }
    }

    // show the fields for a saved puzzle at a given index
    private void ShowSavedPuzzle(int puzzleIndex)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(mPuzzleData.PuzzleCollection[puzzleIndex].Name);
        if (mExpandedIndex == -1 && GUILayout.Button("Modify"))
        {
            mExpandedIndex = puzzleIndex;
        }

        // delete a puzzle
        if (GUILayout.Button("Delete"))
        {
            mPuzzleData.RemoveAt(puzzleIndex);
            mExpandedIndex = -1;
        }
        
        GUILayout.EndHorizontal();

        // show fields for modifying
        if (puzzleIndex == mExpandedIndex)
        {
            GUILayout.Label("> Modify Puzzle");

            // set fields
            ModifyFields(mPuzzleData.PuzzleCollection[puzzleIndex]);

            if (GUILayout.Button("Done"))
            {
                Save();
                mExpandedIndex = -1;
            }
        }

        DrawLine();
    }

    // modify the fields for a given puzzle
    private void ModifyFields(HVRPuzzle.Puzzle p)
    {
        p.Name = EditorGUILayout.TextField("Name: ", p.Name);
        p.Description = EditorGUILayout.TextArea(p.Description, GUILayout.Height(60));
        p.PuzzleType = (HVRPuzzle.Type)EditorGUILayout.EnumPopup("Puzzle Type: ", p.PuzzleType);
        p.TriggerType = (HVRPuzzle.Type)EditorGUILayout.EnumPopup("Trigger Type: ", p.TriggerType);
        // TODO objects (maybe be able to drag them into the list? or drag them into a field and click an add button)s
        p.RewardType = (HVRPuzzle.Reward)EditorGUILayout.EnumPopup("Reward Type: ", p.RewardType);
    }

    // load the saved puzzle collection asset
    private void Load()
    {
        string totalPath = string.Format("{0}{1}.asset", kDirectoryPath, kAssetName);
        mPuzzleData = (PuzzleCollectionData)AssetDatabase.LoadAssetAtPath(totalPath, typeof(PuzzleCollectionData));

        if (mPuzzleData == null)
        {
            Debug.LogError("Failed to load puzzle data");
            Save();
        }
    }

    // save the saved puzzle collection asset
    private void Save()
    {
        if (mPuzzleData != null)
        {
            AssetDatabase.SaveAssets();
            Debug.Log("Puzzle data saved.");
        }
        else
        {
            string totalPath = string.Format("{0}{1}.asset", kDirectoryPath, kAssetName);
            mPuzzleData = (PuzzleCollectionData)ScriptableObject.CreateInstance("PuzzleCollectionData");
            AssetDatabase.CreateAsset(mPuzzleData, totalPath);
        }
    }

    private void DrawLine()
    {
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
    }
}
