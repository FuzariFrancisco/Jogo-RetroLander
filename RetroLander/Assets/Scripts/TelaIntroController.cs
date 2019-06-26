using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaIntroController : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Sair()
    {
        Application.Quit();
    }
}
