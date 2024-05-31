using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 360f; // 좌우 회전 속도
    private Transform mapTransform;
    private Camera mainCamera;
    public Gun gun;

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    private void Start()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        mainCamera = Camera.main;

        GameObject leverArt = GameObject.Find("Level Art");
        if (leverArt != null)
        {
            mapTransform = leverArt.transform;
        }
        else
        {
            Debug.LogWarning("Level Art 오브젝트를 찾을 수 없습니다.");
        }
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate()
    {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        Rotate();
        Move();

        float move = playerInput.moveX == 0 ? playerInput.moveY : playerInput.moveX;
        playerAnimator.SetFloat("Move", move);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move()
    {
        //Vector3 moveDistance = playerInput.moveX * transform.forward * moveSpeed * Time.deltaTime;

        //Vector3 moveDistance = (playerInput.moveX * transform.forward + playerInput.moveY * transform.right).normalized * moveSpeed * Time.deltaTime; 

        //Vector3 moveDistance = new Vector3(playerInput.moveY, 0f, playerInput.moveX).normalized * moveSpeed * Time.deltaTime;

        //Vector3 moveX = playerInput.moveX * mapTransform.transform.forward * moveSpeed * Time.deltaTime;
        //Vector3 moveY = playerInput.moveY * mapTransform.transform.right * moveSpeed * Time.deltaTime;
        //Vector3 moveDistance = moveX + moveY;

        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0f;
        Vector3 cameraRight = mainCamera.transform.right;
        cameraRight.y = 0f;
        Vector3 moveX = playerInput.moveX * cameraForward.normalized;
        Vector3 moveY = playerInput.moveY * cameraRight.normalized;
        Vector3 moveDistance = (moveX + moveY).normalized * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate()
    {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0);

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, gun.transform.position.y, 0));
//plane.Translate(0, gun.transform.position.y, 0);
        if(plane.Raycast(ray, out float distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = (target - transform.position).normalized;
            direction.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(direction);

            playerRigidbody.MoveRotation(rotation);
        }


        ////마우스 움직임 따라 회전
        //// 마우스의 현재 위치를 가져옵니다.
        //Vector3 mousePosition = Input.mousePosition;

        //// 마우스의 화면 상 위치를 월드 좌표로 변환합니다.
        //Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));

        //// 캐릭터가 바라보는 방향을 계산합니다.
        //Vector3 lookDirection = worldMousePosition - transform.position;
        //lookDirection.y = 0f; // Y 축 방향은 회전하지 않도록 합니다.

        //// 캐릭터를 회전합니다.
        //Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        //playerRigidbody.rotation = (Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime));
    }
}