/**
 *  Title:       SanitySelector.cs
 *  Description: Used to help select Scenes through the menus and to quit the game.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    // given the int of a scene, load that scene
    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    // quit the game
    public void QuitGame()
    {
        Debug.Log("Entered QuitGame");
        Application.Quit();
    }
}
