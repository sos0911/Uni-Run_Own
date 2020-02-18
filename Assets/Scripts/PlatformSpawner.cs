using UnityEngine;

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour {
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    public int count = 3; // 생성할 발판의 개수

    public float timeBetSpawnMin = 1.25f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 2.25f; // 다음 배치까지의 시간 간격 최댓값
    private float timeBetSpawn; // 다음 배치까지의 시간 간격

    public float yMin = -3.5f; // 배치할 위치의 최소 y값
    public float yMax = 1.5f; // 배치할 위치의 최대 y값
    private float xPos = 20f; // 배치할 위치의 x 값

    private GameObject[] platforms; // 미리 생성한 발판들
    private int currentIndex = 0; // 사용할 현재 순번의 발판

    private Vector2 poolPosition = new Vector2(0, -25); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치
    private float lastSpawnTime; // 마지막 배치 시점


    void Start() {
        // 변수들을 초기화하고 사용할 발판들을 미리 생성
        platforms = new GameObject[count];

        // 배열을 프리팹을 구현하여 채운다. 위치는 poolposition
        // instantiate의 세번째 인자는 회전에 관한 인자이다. 회전시키지 않을 것이므로 identity
        // 맵에는 보이지 않지만 구현은 됬으므로 화면아래에서 움직이고있을듯 ㅋㅋ
        for (int i = 0; i < count; i++)
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);

        lastSpawnTime = 0f;
        // 다음번까지의 배치간격을 0으로 초기화
        // 바로 첫번째 발판이 준비됨.
        timeBetSpawn = 0f;
    }

    void Update() {
        // 순서를 돌아가며 주기적으로 발판을 배치
        if (GameManager.instance.isGameover)
            return;

        if(Time.time >= lastSpawnTime + timeBetSpawn)
        {
            // 발판을 준비하기로 결정함
            // 스폰시간 기록하고 다음 배치간격시간 랜덤화
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            float yPos = Random.Range(yMin, yMax);

            // setactive >> 가시상태 재배열
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);

            // 위치이동
            platforms[currentIndex].transform.position = new Vector2(xPos, yPos);
            currentIndex++;

            // 원형 큐(?)
            if (currentIndex >= count)
                currentIndex = 0;
        }
    }
}