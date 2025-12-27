using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lifeCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public static int lifeCount = 3;
    public static bool playLifeLossAnimation;
    public GameObject lifePlatform1;
    public GameObject lifePlatform2;
    public GameObject lifePlatform3;

    public GameObject life1;
    public GameObject life2;
    //public loadLevel ll;
    void Start()
    {
        if (availableLevels.Count == 0)
    {
        RefillLevels();
    }

        if(playLifeLossAnimation)
        {
        Invoke("updateLifeUI",1);
        }
        else
        updateLifeUI();
        
        Invoke("randomLevel",3);
        
    }

    // Update is called once per frame
    void Update()
    {
       if(playLifeLossAnimation)
       updateLifeUI();
       if(playLifeLossAnimation)
       playLifeLossAnimation = false;
    }
    public void updateLifeUI()
    {
         if(lifeCount == 2 && playLifeLossAnimation)
        {
            lifePlatform1.SetActive(false);
        }
        if (lifeCount==2&&playLifeLossAnimation==false)
        {
            life1.SetActive(false);
        }

        if(lifeCount==1&&playLifeLossAnimation)
        {
            life1.SetActive(false);
            lifePlatform2.SetActive(false);
        }
        if(lifeCount==1&&playLifeLossAnimation==false)
        {
            life1.SetActive(false);
            life2.SetActive(false);
        }
        if(lifeCount==0)
        {
            life1.SetActive(false);
            life2.SetActive(false);
            lifePlatform3.SetActive(false);

        }
    }
    static int min = 23;
    static int max = 38;
    static List<int> availableLevels = new List<int>();
    public void randomLevel()
{
    // If we've used all levels, refill the list
    if (availableLevels.Count == 0)
    {
        RefillLevels();
    }
        int randomIndex = Random.Range(0, availableLevels.Count);
        int selectedLevel = availableLevels[randomIndex];
        availableLevels.RemoveAt(randomIndex);
        SceneManager.LoadScene(selectedLevel);
    }
    void RefillLevels()
{
    availableLevels.Clear();
    
    // Add all levels from min to max (excluding max)
    for (int i = min; i < max; i++)
    {
        availableLevels.Add(i);
    }
    
    //Debug.Log("Refilled levels pool - all levels available again!");
}
    public void loseLife()
    {
        lifeCount--;
    }
}
