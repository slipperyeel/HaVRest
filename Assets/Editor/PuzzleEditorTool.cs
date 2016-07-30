using UnityEngine;
using System.Collections;
using UnityEditor;
using HVRPuzzle;

public class PuzzleEditorTool : EditorWindow
{
    private string kDirectoryPath = "Assets/Editor/Data/";
    private string kAssetName = "PuzzleCollection";

    private PuzzleCollectionData mPuzzleData = null;
    private Puzzle mCurrentPuzzle = null;
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

            // clear button
            if (GUILayout.Button("Clear All"))
            {
                mPuzzleData.PuzzleCollection.Clear();
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
            mCurrentPuzzle = new Puzzle();
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
    private void ModifyFields(Puzzle p)
    {
        p.Name = EditorGUILayout.TextField("Name: ", p.Name);
        p.Description = EditorGUILayout.TextArea(p.Description, GUILayout.Height(60));

        // Set puzzle data
        p.Data.Type = (eType)EditorGUILayout.EnumPopup("Puzzle Type: ", p.Data.Type);
        ModifyTypeData(p.Data);

        DrawLine();

        // Set trigger data
        p.Trigger.Type = (eType)EditorGUILayout.EnumPopup("Trigger Type: ", p.Trigger.Type);
        ModifyTypeData(p.Trigger);

        DrawLine();

        // pick reward sub-type
        ModifyRewardData(p.Reward);
    }

    private void ModifyTypeData(Puzzle.TypeData data)
    {
        switch (data.Type)
        {
            case eType.Physical:
                data.PhysicalSubType = (ePhysicalType)EditorGUILayout.EnumPopup("  Physical: ", data.PhysicalSubType);

                // break physical type down into specifics
                switch (data.PhysicalSubType)
                {
                    case ePhysicalType.PlayerLocation:
                        data.PlayerCollider = (Collider)EditorGUILayout.ObjectField("  Collider: ", data.PlayerCollider, typeof(Collider), false);
                        data.PlayerPosition = EditorGUILayout.Vector3Field("  Position:", data.PlayerPosition);
                        break;

                    case ePhysicalType.ItemLocation:
                        data.ItemCollider = (Collider)EditorGUILayout.ObjectField("  Collider: ", data.ItemCollider, typeof(Collider), false);
                        data.ItemPosition = EditorGUILayout.Vector3Field("  Position:", data.ItemPosition);
                        break;

                    case ePhysicalType.PlayerStateChange:
                        // show enum field for data.PlayerState
                        break;

                    case ePhysicalType.ItemStateChange:
                        // show enum field for data.ItemState
                        break;
                }

                break;

            case eType.Temporal:
                data.TemporalSubType = (eTemporalType)EditorGUILayout.EnumPopup("  Temporal: ", data.TemporalSubType);

                // break temporal type down into specifics
                switch (data.TemporalSubType)
                {
                    case eTemporalType.TimeOfDay:
                        data.TemporalTimeOfDay = (TimeOfDay)EditorGUILayout.EnumPopup("  Time of Day: ", data.TemporalTimeOfDay);
                        break;

                    case eTemporalType.Duration:
                        data.TemporalDuration = EditorGUILayout.FloatField("  Duration: ", data.TemporalDuration);
                        break;
                }
                break;
        }
    }

    private void ModifyRewardData(Puzzle.RewardData data)
    {
        data.RewardType = (eReward)EditorGUILayout.EnumPopup("Reward Type: ", data.RewardType);

        switch (data.RewardType)
        {
            case eReward.Currency:
                data.RewardAmount = EditorGUILayout.IntField("  Amount: ", data.RewardAmount);
                break;

            case eReward.Resource:
                data.RewardObject = (GameObject)EditorGUILayout.ObjectField("  Prefab: ", data.RewardObject, typeof(GameObject), false);
                data.RewardAmount = EditorGUILayout.IntField("  Amount: ", data.RewardAmount);
                break;

            case eReward.Seed:
                data.RewardObject = (GameObject)EditorGUILayout.ObjectField("  Prefab: ", data.RewardObject, typeof(GameObject), false);
                data.RewardAmount = EditorGUILayout.IntField("  Amount: ", data.RewardAmount);
                break;

            case eReward.Tool:
                data.RewardObject = (GameObject)EditorGUILayout.ObjectField("  Prefab: ", data.RewardObject, typeof(GameObject), false);
                break;
        }
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
