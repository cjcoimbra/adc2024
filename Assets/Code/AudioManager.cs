using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioSource audio;
    public AudioSource hit;
    public AudioSource notPassable;
    public AudioSource enemyDeath;
    public AudioSource door;
    public AudioSource footsteps;
    public AudioSource medkit;
    public AudioSource terminal;
    // Start is called before the first frame update
    void Start() {
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayNotPassable() {
        notPassable.Play();
    }

    public void PlayHit() {
        hit.Play();
    }

    public void PlayDoorOpen() {
        door.Play();
    }

    public void PlayTerminal()
    {
        terminal.Play();
    }

    public void PlayMedkit()
    {
        medkit.Play();
    }

    public void PlayFootsteps() {
        if (footsteps.isPlaying) {
            return;
        }
        
        footsteps.Play();
    }

    public void PlayEnemyDeath() {
        enemyDeath.Play();
    }
}
