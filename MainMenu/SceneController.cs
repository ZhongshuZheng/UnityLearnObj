using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public IEnumerator LoadScene(string name, string name_ori = null) {

        yield return null;

        AsyncOperationHandle<SceneInstance> operation = Addressables.LoadSceneAsync(name, LoadSceneMode.Additive);

        yield return null;

        while(!operation.IsDone) {
            // print(operation.GetDownloadStatus().Percent);   // if download
            print(operation.PercentComplete);
            yield return null;
        }

        if (name == "SampleScene") {
            TimerController.Instance.do_after_seconds(1f, GameController.Instance.GameLoad);
        }

        if (name_ori != null) {
            SceneManager.UnloadSceneAsync(name_ori);    // If the scene is loaded traditionally
            // Addressables.UnloadSceneAsync(scene_ori); // if the scene is loaded by addressable, name_ori should be a addressable handler
        }
    }

}
