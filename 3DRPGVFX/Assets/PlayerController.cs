using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 5f;
    public float dashPower = 10f;
    public float gravityValue;

    public Transform realCamera;
    public float maxDistance;
    public float minDistance;
    Vector3 dirNomalized;
    Vector3 finalDir;
    float FinalDistance;

    bool isDash;

    [SerializeField]
    Transform characterBody;
    [SerializeField]
    Transform cameraArm;
    [SerializeField]
    Transform objFollow;
    Vector3 moveDir;

    CharacterController characterCon;
    Animator anim;
    Rigidbody rigid;


    // Start is called before the first frame update
    void Start()
    {
        anim = characterBody.GetComponent<Animator>();
        characterCon = GetComponent<CharacterController>();
        rigid = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        moveDir = Vector3.zero;

        dirNomalized = realCamera.localPosition.normalized;
        FinalDistance = realCamera.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
        CharacterMove();
    }

    //ī�޶� ����
    void LookAround()
    {
        //���� ���콺�� X,Y ��ġ ���� ����
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        float angleX = camAngle.x - mouseDelta.y;
        float angleY = camAngle.y + mouseDelta.x;
        if (angleX < 180f)
        {
            //X���� 180�� �̸��� �� -1 ~ 61�� ���� ���� �ʵ��� ��
            //����� ���� ����ȸ�� ����
            angleX = Mathf.Clamp(angleX, -1f, 61f);
        }
        else
        {
            //X���� 180�� ���� Ŭ �� 315 ~ 361�� ���� ���� �ʵ��� ��
            //����� ���� �Ʒ��� ȸ�� ���� 
            angleX = Mathf.Clamp(angleX, 315f, 361f);
        }
        cameraArm.rotation = Quaternion.Euler(angleX, angleY, camAngle.z);

        //ī�޶� ��ġ ���� ����
        cameraArm.position = Vector3.MoveTowards(cameraArm.position, objFollow.position, Time.deltaTime * 7f);

        finalDir = cameraArm.TransformPoint(dirNomalized * maxDistance);
        
        RaycastHit hit;
        Debug.DrawLine(cameraArm.position, finalDir, Color.blue);
        if(Physics.Linecast(cameraArm.position, finalDir, out hit))
        {
            if(hit.transform.gameObject != this.gameObject)
            {
                FinalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            }
        }
        else
        {
            FinalDistance = maxDistance;
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNomalized * FinalDistance, Time.deltaTime * 10f);
    }

    //�÷��̾� �̵� ����
    void CharacterMove()
    {
        //�÷��̾� �ٶ󺸴� ����
        Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0, cameraArm.forward.z).normalized, Color.red);

        //wasd �̵�
        Vector2 nextMove = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        bool isMove = Vector3.Magnitude(nextMove) != 0;
        anim.SetBool("isRun", isMove);

        Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), Vector3.down);
        RaycastHit hit;
        
        bool isGround = Physics.Raycast(ray, out hit, 0.1f);
        anim.SetBool("isJump", !isGround);
        anim.SetBool("isDash", moveSpeed == dashPower);

        if (characterCon.isGrounded || isGround)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0, cameraArm.right.z).normalized;
            moveDir = (lookForward * nextMove.y + lookRight * nextMove.x);

            if (Input.GetButtonDown("Jump") && moveSpeed != dashPower)
            {
                moveDir.y = jumpPower;
            }
            if (Input.GetButtonDown("Dash") && !isDash)
            {
                Debug.Log("Dash");
                StartCoroutine(DashCorutine());
            }
        }
        else
        {
            moveDir.y -= gravityValue * Time.deltaTime;
        }

        if (isMove && (characterCon.isGrounded || isGround))
        {
            //characterBody.forward = lookForward;
            //characterBody.rotation = Quaternion.Slerp(characterBody.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * 10f);
            characterBody.rotation = Quaternion.Slerp(characterBody.rotation, 
                Quaternion.LookRotation(new Vector3(moveDir.x, moveDir.y, moveDir.z)), Time.deltaTime * 10f);
        }

        characterCon.Move(moveDir * moveSpeed * Time.deltaTime);

    }


    IEnumerator DashCorutine()
    {
        float reset = moveSpeed;
        isDash = true;
        moveSpeed = dashPower;
        yield return new WaitForSeconds(0.2f);
        moveSpeed = reset;
        yield return new WaitForSeconds(1f);
        isDash = false;

    }


}
