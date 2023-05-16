using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class player : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    float hAxis;
    float vAxis;
    bool jDown, isJump, isDodge, wDown, iDown, isSwap, fDown;
    bool isFireReady = true;
    bool sDown1, sDown2, sDown3;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;

    GameObject nearObject;
    Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;

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
        Attack();
        Dodge();
        Interation();
        Swap();
    }
    void GetInput() // 입력 ( 키보드 ) 관련 함수
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");/* 왜 GetButton으로 안 받는거지?*/
        fDown = Input.GetButtonDown("Fire1");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }
    void Move() // 움직이는 방법 관련 함수
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge) moveVec = dodgeVec;
        if (isSwap || !isFireReady) moveVec = Vector3.zero;

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
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge && !isSwap) // ! = not
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("Is_jump", true);
            anim.SetTrigger("Do_jump");
            isJump = true;
        }
    }
    void Attack()
    {
        if (equipWeapon == null) return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isDodge && !isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger("Do_swing");
            fireDelay = 0;
        }
    }
    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap) // ! = not
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
    void Swap()
    {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0)) return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1)) return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2)) return;

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
        {
            if (equipWeapon == null) gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("Do_swap");

            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }
    void SwapOut()
    {
        isSwap = false;
    }
    void Interation()
    {
        if (iDown && nearObject != null && !isJump && isDodge)
        {
            if (nearObject.tag == "Weapon")
            {
                item item = nearObject.GetComponent<item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }
    private void OnCollisionEnter(Collision collision) // 이 함수를 통해 바닥에 닿는 걸 감지 + isJump 변수 false
    {
        if (collision.gameObject.tag == "Floor") /*!*/
        {
            anim.SetBool("Is_jump", false);
            isJump = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
    }
}