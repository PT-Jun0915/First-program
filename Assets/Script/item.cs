using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public enum Type { Ammo, Coin, Grenage, Heart, Weapon }; // 열거형 타입 ( 타입 이름 지정 필요 )
    public Type type;
    public int value; // 아이템 종류와 값을 저장할 변수 선언

    void Update()
    {
        transform.Rotate(Vector3.up * 25 * Time.deltaTime);    
    }

}
