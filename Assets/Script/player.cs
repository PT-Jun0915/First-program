using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    bool wDown;

    Vector3 moveVec;
    Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if(wDown) transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        else transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("Is_run", moveVec !=  Vector3.zero);
        anim.SetBool("Is_walk", wDown);

        //회전하는 방법
        transform.LookAt(transform.position + moveVec);

    }

}
