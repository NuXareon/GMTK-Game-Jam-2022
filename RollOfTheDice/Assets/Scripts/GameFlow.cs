using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerDeath()
    {
        //gameState = GameState.PlayerDead;
        //Time.timeScale = 0;
        StartCoroutine(KillPlayer());
    }

    public void OnLevelCompleted()
    {
        Time.timeScale = 0;
        StartCoroutine(FinishLevel());
    }

    IEnumerator KillPlayer()
    {
        // TODO play death audio

        yield return new WaitForSecondsRealtime(0.5f);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    IEnumerator FinishLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        int nextSceneIndex = scene.buildIndex + 1;

        yield return new WaitForSecondsRealtime(0.5f);

        Time.timeScale = 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
