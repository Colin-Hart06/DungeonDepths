using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LevelManager instance;

    void Start()
    {
        updateLevel();
    }

    // Update is called once per fram
    public void updateLevel()
    {
        string a = SceneManager.GetActiveScene().name;
        if (a != "Back To Menu" && a != "Main Menu" && a != "Try Again Speedrun" && a != null)
            SetString("LevelName", a);
    }
    public void SetString(string LevelName, string level)
    {
            PlayerPrefs.SetString(LevelName, level);
            PlayerPrefs.Save();
    }
}
