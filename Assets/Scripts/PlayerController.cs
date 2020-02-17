using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
   public AudioClip deathClip; // 사망시 재생할 오디오 클립
   public float jumpForce = 700f; // 점프 힘

   private int jumpCount = 0; // 누적 점프 횟수
   private bool isGrounded = false; // 바닥에 닿았는지 나타냄
   private bool isDead = false; // 사망 상태

   private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
   private Animator animator; // 사용할 애니메이터 컴포넌트
   private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트

   private void Start() {
        // 초기화
        // 이 스크립트는 player에 붙는다는 걸 생각하자.
        // private부분 일괄캐싱
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
   }

   private void Update() {
        // 사용자 입력을 감지하고 점프하는 처리
        if (isDead)
        {
            return;
        }

        // 점프요청이 들어왔고 아직 점프 2번이상 안함 >> 점프가능!
        // 점프도중에 길게 누르고 있는만큼 더 멀리 점프하나보다..
        if(Input.GetMouseButtonDown(0) && jumpCount < 2)
        {
            jumpCount++;
            // 점프직전 속도를 0으로 변경
            // 이단점프를 하게되면 아래로 떨어질때 이처리를 안할시 위로 안올라갈 수도 있기 때문
            playerRigidbody.velocity = Vector2.zero;
            // y축 윗방향으로 힘을 준다. 포물선 이동
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            // 뛰는 소리 재생
            // jumpclip은 player에 붙여진 상태이므로 사용가능
            playerAudio.Play();
        }
        // 버튼이 unpressed && 캐릭터 y축 윗방향으로 이동중
        else if(Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0)
        {
            // 낙하처리
            // 속도가 계속 깎여서 아래로 내려가게 됨
            playerRigidbody.velocity *= 0.5f;
        }

        // bool 상태변경 >> fsm으로 플레이어 애니메이션 조정
        animator.SetBool("Grounded", isGrounded);
   }

   private void Die() {
        // 사망 처리
        // 애니메이션 fsm 트리거 
        animator.SetTrigger("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play();

        // 속도를 0으로
        playerRigidbody.velocity = Vector2.zero;
        
        isDead = true;
   }

   private void OnTriggerEnter2D(Collider2D other) {
        // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
        if (other.tag == "Dead" && !isDead)
            Die();
   }

   private void OnCollisionEnter2D(Collision2D collision) {
        // 바닥에 닿았음을 감지하는 처리
        // normal.y가 0.7f보다 크면 충돌면의 방향이 위쪽이라는 것
        // >> 충돌물체가 자기보다 아래에 있을 확률이 높음.
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }
   }

   private void OnCollisionExit2D(Collision2D collision) {
        // 바닥에서 벗어났음을 감지하는 처리
        isGrounded = false;
   }
}