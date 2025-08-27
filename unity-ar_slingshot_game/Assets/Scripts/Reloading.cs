using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reloading : MonoBehaviour
{
    public static string SceneToLoad = "ARSlingshotGame";

    void Start()
    {
        StartCoroutine(DelayedLoad());
    }

    private IEnumerator DelayedLoad()
    {
        yield return new WaitForSeconds(1f);

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Single);

        while (!loadOp.isDone)
            yield return null;
    }

}
