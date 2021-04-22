using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
  public void start()
    {
        SceneManager.LoadScene(1);
    }
    public void end()
    {
        SceneManager.LoadScene(2);
    }
}