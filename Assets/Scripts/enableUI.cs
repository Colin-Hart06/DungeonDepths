using UnityEngine;

public class enableUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new private audioManager audio;
    void Start()
    {
        audio = audioManager.instance;
    }

    // Update is called once per frame
    public GameObject UI;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        UI.SetActive(true);
        audio.Play("Victory");
    }
}
