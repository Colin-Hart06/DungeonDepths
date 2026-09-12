using UnityEngine;
using UnityEngine.SceneManagement;

public class scenetransition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string fadeOutSong;
    private void Start()
    {
        audioManager.instance.FadeOut(fadeOutSong, 2f); // fades out over 2 seconds
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(col.gameObject);
        Invoke("endGame", 2);

    }
    public string level;
    private void endGame()
    {
            SceneManager.LoadScene(sceneName: level);
    }
}
