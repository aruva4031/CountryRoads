/**
 *  Title:       DifficultyChosen.cs
 *  Description: For pssing the difficulty level between scenes
 *  Outcome addressed: Model player and apply it to game difficulty management
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DifficultyChosen
{
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
