using UnityEngine;

// 발판으로서 필요한 동작을 담은 스크립트
public class Platform : MonoBehaviour {
    public GameObject[] obstacles; // 장애물 오브젝트들
    private bool stepped = false; // 플레이어 캐릭터가 밟았었는가
    private int stepscore = 1; // 밟을 때마다 오르는 점수

    // 컴포넌트가 활성화될때 마다 매번 실행되는 메서드
    // 발판 오브젝트를 1-2개 만들어놓고 계속 껐다켰다 하면서 재활용할 생각
    // 그때마다 활성화되는 가시수는 랜덤
    private void OnEnable()
    {
        // 발판을 리셋하는 처리
        // 밟았다는 사실 리셋
        stepped = false;

        for (int i = 0; i < obstacles.Length; i++)
        {
            // 각 가시 오브젝트마다 1/3 확률로 생성되게 함
            if (Random.Range(0, 3) == 0)
                obstacles[i].SetActive(true);
            else
                obstacles[i].SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // 플레이어 캐릭터가 자신을 밟았을때 점수를 추가하는 처리
        if(collision.collider.tag=="Player" && !stepped)
        {
            stepped = true;
            // 밟을때마다 1점
            GameManager.instance.AddScore(stepscore);
        }
    }
}