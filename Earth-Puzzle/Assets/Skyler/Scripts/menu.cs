using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public void Load()
    {
        SceneManager.LoadScene("Bunker");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
