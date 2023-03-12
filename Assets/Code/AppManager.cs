using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    public void ReloadScene(int idx)
    {
        SceneManager.LoadScene(idx);
    }    
    public void Quit()
    {
        Application.Quit();
    }
}
