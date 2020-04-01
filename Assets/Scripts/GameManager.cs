using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : MonoBehaviour {
    public static GameManager instance; // 싱글톤을 할당할 전역 변수

    public int level = 1; // 현재 레벨(difficulty 정의)
    public int Leveltime = 10; // 10초 후마다 레벨이 바뀜. 무조건 3초 이상으로 설정해 주세요!!
    // 20초마다 레벨이 올라가고, 밟을때 점수가 더 많이 올라가는 대신 더 빨라진다.
    public float AddspeedperLevel = 3;
    public int AddscoreperLevel = 3;


    public bool isGameover = false; // 게임 오버 상태
    public Text levelText; // level text
    public Text scoreText; // 점수를 출력할 UI 텍스트
    public GameObject gameoverUI; // 게임 오버시 활성화 할 UI 게임 오브젝트
    public Text HighscoreText;

    private int score = 0; // 게임 점수
    private int savedHighscore = 0; // 최고 점수
    private string prefsScoreKey = "Score";

    // 게임 시작과 동시에 싱글톤을 구성
    void Awake() {
        // 싱글톤 변수 instance가 비어있는가?
        if (instance == null)
        {
            // instance가 비어있다면(null) 그곳에 자기 자신을 할당
            instance = this;
        }
        else
        {
            // instance에 이미 다른 GameManager 오브젝트가 할당되어 있는 경우

            // 씬에 두개 이상의 GameManager 오브젝트가 존재한다는 의미.
            // 싱글톤 오브젝트는 하나만 존재해야 하므로 자신의 게임 오브젝트를 파괴
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }

        // 관련 값들 다 초기화
        // 스크롤 속도, 점수..
        ScrollingObject.ModifySpeed(ScrollingObject.initialspeed);
        Platform.ModifyScore(Platform.Initialstepscore);
        HighscoreText.color = Color.white;
    }

    private IEnumerator Start()
    {
        // IEnumerator start()면 미리 선언한 level=1이 안먹히네..
        level = 1;
        levelText.text = "Level : " + level;
        StartCoroutine(IncreaseLevel());

        yield return null;
    }

    void Update() {
        // 게임 오버 상태에서 게임을 재시작할 수 있게 하는 처리
        if (isGameover && Input.GetMouseButtonDown(0))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator IncreaseLevel()
    {
        if (!isGameover)
        {
            yield return new WaitForSeconds(Leveltime - 3);

            // 레벨 바뀌기 3초 전부터 공지할거임!

            yield return NotifyBeforeLevelChanged(3f);

            levelText.color = Color.red;
            level++;
            levelText.text = "Level : " + level + "\nincreases moving speed and score per landing!";

            // 스크롤 속도, 밟을 때 발판당 점수 증가
            ScrollingObject.ModifySpeed(ScrollingObject.speed + AddspeedperLevel);
            Platform.ModifyScore(Platform.stepscore + AddscoreperLevel); // 레벨당 ?점씩 증가

            StartCoroutine(MarkLevelSometime(3));

            StartCoroutine(IncreaseLevel());
           
        }
    }

    IEnumerator NotifyBeforeLevelChanged(float time)
    {
        Debug.Log("entered!");
        // time초 동안 색상 10번 바꾸기
        for(int i=0;!isGameover && i<10; i++)
        {
            levelText.text = "Level : " + level + "\nlevel will be changed in " + (int)(time-i*time/10) + " seconds!";

            if (levelText.color == Color.white)
                levelText.color = Color.yellow;
            else
                levelText.color = Color.white;
            yield return new WaitForSeconds(time / 10);
        }
        yield return null;
    }

    IEnumerator MarkLevelSometime(int time)
    {
        yield return new WaitForSeconds(time);
        levelText.color = Color.white;
        levelText.text = "Level : " + level;
    }


    // 점수를 증가시키는 메서드
    public void AddScore(int newScore) {
        if (!isGameover)
        {
            score += newScore;
            scoreText.text = "Score : " + score;
        }
    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    // 이벤트 함수라 on~
    public void OnPlayerDead() {
        isGameover = true;
        levelText.enabled = false;

        // 최고 기록 display
        savedHighscore = PlayerPrefs.GetInt(prefsScoreKey, 0);
        if (score > savedHighscore)
        {
            PlayerPrefs.SetInt(prefsScoreKey, score);
            HighscoreText.color = Color.green;
            HighscoreText.text = "New Highscore!\nHighscore : " + score;
        }
        else
            HighscoreText.text = "Highscore : " + savedHighscore;

        gameoverUI.SetActive(true);
    }

}