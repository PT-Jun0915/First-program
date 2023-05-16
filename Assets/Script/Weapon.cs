using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.LegacyInputHelpers;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing"); //StopCoroutine() : 코루틴 정지 함수
            StartCoroutine("Swing"); //StartCoroutine() : 코루틴 실행 함수
        }
    }
    IEnumerator Swing()
    {
        //1번구역 실행
        yield return new WaitForSeconds(0.1f); // 0.1 ( default : 1프레임 ) 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        // 결과를 전달하는 키워드, 코루틴은 이게 1개 이상 존재해야 함!!
        //2번구역 실행
        yield return new WaitForSeconds(0.2f); // 0.2 대기
        meleeArea.enabled = false;
        //3번구역 실행
        yield return new WaitForSeconds(0.3f); // 0.3 대기
        trailEffect.enabled = false;
        
        yield break;
        //yield break로 코루틴 탈출 가능
    }

    //Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인루틴 ( 교차 실행 ) : 일반함수
    //Use() 메인루틴 + Swing() 코루틴 ( 같이 실행 ) : 코루틴 함수
}
