using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnPlay : MonoBehaviour
{
    public void Win()
    {
        SceneManager.LoadScene("Score");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Game Over");
    }
}
