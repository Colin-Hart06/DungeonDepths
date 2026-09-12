using UnityEngine;

public class triggerMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private new audioManager audio;
    public bool StartOverworldMusic;
    public bool StartCaveMusic;
    public bool StartDungeonMusic;
    void Start()
    {
        audio = audioManager.instance;
        if (audio.IsPlaying("Ground Theme"))
            audio.StopPlaying("Ground Theme");
        if(audio.IsPlaying("Cave Theme"))
            audio.StopPlaying("Cave Theme");
        if(audio.IsPlaying("Dungeon Theme"))
            audio.StopPlaying("Dungeon Theme");

        if (StartOverworldMusic)
            audio.Play("Ground Theme");
        else if (StartCaveMusic)
            audio.Play("Cave Theme");
        else if (StartDungeonMusic)
            audio.Play("Dungeon Theme");
    }

    // Update is called once per frame
    
}
