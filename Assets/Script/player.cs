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
        GetInput(); // Input�ް� ����Ǳ� ������ ���� ����...
        Move();
        Turn();
        Jump();
        Dodge();
    }
    void GetInput() // �Է� ( Ű���� ) ���� �Լ�
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump"); /* �� GetButton���� �� �޴°���?*/
    }
    void Move() // �����̴� ��� ���� �Լ�
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge) moveVec = dodgeVec;

        if (wDown) transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        else transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("Is_run", moveVec != Vector3.zero);
        anim.SetBool("Is_walk", wDown);
    }
    void Turn() // ȸ���ϴ� ��� ���� �Լ�
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
    private void OnCollisionEnter(Collision collision) // �� �Լ��� ���� �ٴڿ� ��� �� ���� + isJump ���� false
    {
        if (collision.gameObject.tag == "Floor") /*!*/
        {
            anim.SetBool("Is_jump", false);
            isJump = false;
        }
    }
}
