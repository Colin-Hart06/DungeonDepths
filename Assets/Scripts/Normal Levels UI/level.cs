using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class level : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI levelText;
    private Scene scene;
    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        if ((levelText!=null))
        {
            levelText.text = scene.name;
        }
        
    }
}
