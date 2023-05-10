using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    bool wDown;

    bool jDown, isJump, isDodge;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput(); // Input받고 실행되기 때문에 제일 위에...
        Move();
        Turn();
        Jump();
        Dodge();
    }
    void GetInput() // 입력 ( 키보드 ) 관련 함수
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump"); /* 왜 GetButton으로 안 받는거지?*/
    }
    void Move() // 움직이는 방법 관련 함수
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge) moveVec = dodgeVec;

        if (wDown) transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        else transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("Is_run", moveVec != Vector3.zero);
        anim.SetBool("Is_walk", wDown);
    }
    void Turn() // 회전하는 방법 관련 함수
    {
        transform.LookAt(transform.position + moveVec);
    }
    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge) // ! = not
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("Is_jump", true);
            anim.SetTrigger("Do_jump");
            isJump = true;
        }
    }
    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge) // ! = not
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("Do_dodge");
            isDodge = true;

            Invoke("DodgeOut", 0.4f);
        }
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }
    private void OnCollisionEnter(Collision collision) // 이 함수를 통해 바닥에 닿는 걸 감지 + isJump 변수 false
    {
        if (collision.gameObject.tag == "Floor") /*!*/
        {
            anim.SetBool("Is_jump", false);
            isJump = false;
        }
    }
}
