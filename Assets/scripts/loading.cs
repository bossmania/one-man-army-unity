using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loading : MonoBehaviour
{
    public int dest;
    public player player_scr;
    void Start()
    {
        player player_scr = GameObject.FindWithTag("Player").GetComponent<player>();
        player_scr.saveSystemController.savePlayer(player_scr);
        // GetComponent<Transform>().GetChild(0).GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(0.1f);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadSceneAsync(dest);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Destroy(gameObject);
    }
}
