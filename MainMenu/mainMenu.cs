using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void exit_game() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif

    }

    public void start_game() {
        StartCoroutine(SceneController.Instance.LoadScene("SampleScene", "MainMenu"));
    }
}
