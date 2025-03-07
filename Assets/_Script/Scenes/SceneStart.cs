using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStart : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmVersion;

    void OnValidate()
    {
        if (tmVersion != null)
            tmVersion.text = $"v{Application.version}";
    }

    public void TapToStart()
    {
        //Debug.Log("탭 투 스타트");
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
