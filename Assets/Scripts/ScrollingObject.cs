using UnityEngine;

// 게임 오브젝트를 계속 왼쪽으로 움직이는 스크립트
public class ScrollingObject : MonoBehaviour {
    public const float initialspeed = 10f; // 처음 속도
    public static float speed = 10f; // 현재 속도

    private void Update() {
        // 게임오버가 아닐 시 게임 오브젝트를 왼쪽으로 일정 속도로 평행 이동하는 처리
        // 1초에 10만큼 이동
        if(!GameManager.instance.isGameover)
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        // start platform 정리
        // destroy vs setactive(false)..
        if (gameObject.tag == "StartPlatform" && transform.position.x < -20.0f)
            gameObject.SetActive(false);
    }

    public static void ModifySpeed(float newspeed)
    {
        speed = newspeed;
    }
}