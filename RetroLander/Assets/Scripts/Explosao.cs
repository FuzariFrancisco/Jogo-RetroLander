using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosao : MonoBehaviour
{
    public void TerminouAnimacao()
    {
        GameController gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        gc.OnGameOver();
    }
}
