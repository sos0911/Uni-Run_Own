using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPause : MonoBehaviour
{
    // Start is called before the first frame update
   
    public void Resume()
    {
        // 다시 시간을 재생시킴
        // paused됬을때 resume 누를때 적용
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        // 시간을 정지시킴.
        // pausemenu를 누르면 적용
        Time.timeScale = 0f;
    }

    public void GotoMenu()
    {
        // 시간 다시 해제
        Resume();
        SceneManager.LoadScene("StartMenu");
    }
}
