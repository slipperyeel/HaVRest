using UnityEngine;
using System.Collections;

namespace HVRPuzzle
{
    [System.Serializable]
    public class Puzzle
    {
        // TODO mikes
        // puzzles need:
        // a trigger to make them availabe to the player (a class type for subclassing different trigger types [temporal, physical, etc])
        // a set of objects to instantiate related the the puzzle (with puzzle-specific components)
        // a reward for completing the puzzle (reward class type for subclassing different rewards?)

        // additionally:
        private bool mIsActive;
    }
}
