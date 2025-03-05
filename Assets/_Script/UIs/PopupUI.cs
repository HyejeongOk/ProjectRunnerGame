using UnityEngine;
using MoreMountains.Feedbacks;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PopupUI : MonoBehaviour
{
    //[SerializeField] GameObject dimmerUi;
    [SerializeField] GameObject quit;
    [SerializeField] MMF_Player quitOpen;
    [SerializeField] MMF_Player quitClose;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        quit.SetActive(false);
    }

    void Update()
    {
         if(Input.GetKeyDown(KeyCode.Escape))
         {
             if(quit.activeSelf)
            {
                QuitClose();
            }

            else
            {
                QuitOpen();
            }
         }
    }

    public void QuitOk()
    {
        // 에디터 모드 Quit 작동
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();

        // 빌드후런타임 에서 작동 ( PC, Mobile, Console, Web ...)
        #else
            Application.Quit();
        #endif
    }

    public void QuitOpen()
    {
        quitOpen?.PlayFeedbacks();
        GameManager.IsPlaying = false;
        GameManager.IsUiOpened = true;
    }

    public void QuitClose()
    {
        quitClose?.PlayFeedbacks();
        GameManager.IsPlaying = true;
        GameManager.IsUiOpened = false;
    }
}
