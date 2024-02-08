using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{

    SaveManager saveManager;

    void Start() {
        StartCoroutine(SceneController.Instance.LoadScene("MainMenu"));
    }

    void Update()
    {
        CheckESC();
    }

    public void GameLoad() {
        saveManager = SaveManager.Instance;
        ResetAndLoadData();
        saveManager.LoadAll();
    }

    public void ResetAndLoadData() {
        saveManager.register(HPController.Instance);
        saveManager.register(InventoryControllor.Instance);
    }

    void CheckESC() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            print("esc");
            saveManager.SaveAll();
        }
    }
}
