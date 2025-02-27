using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStart : MonoBehaviour
{
    public string sceneIngame;
    public void TapToStart()
    {
        //Debug.Log("탭 투 스타트");
        SceneManager.LoadScene(sceneIngame,LoadSceneMode.Single);
    }
}
