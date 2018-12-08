/**
 *  For pssing the difficulty level between scenes
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DifficultyChosen {
    private static int difficultyLevel;

    // return the difficult level passed
    public static int getDifficultyLevel()
    {
        return difficultyLevel;
    }

    // set the new difficulty level
    public static void setDifficultyLevel(int val)
    {
        difficultyLevel = val;
    }
}
