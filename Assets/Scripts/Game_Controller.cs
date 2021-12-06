using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Game_Controller : MonoBehaviour
{
    public void LoadSettings()
    {
        SceneManager.LoadScene("Menu");
    }
}
