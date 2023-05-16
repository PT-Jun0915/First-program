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
            StopCoroutine("Swing"); //StopCoroutine() : �ڷ�ƾ ���� �Լ�
            StartCoroutine("Swing"); //StartCoroutine() : �ڷ�ƾ ���� �Լ�
        }
    }
    IEnumerator Swing()
    {
        //1������ ����
        yield return new WaitForSeconds(0.1f); // 0.1 ( default : 1������ ) ���
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        // ����� �����ϴ� Ű����, �ڷ�ƾ�� �̰� 1�� �̻� �����ؾ� ��!!
        //2������ ����
        yield return new WaitForSeconds(0.2f); // 0.2 ���
        meleeArea.enabled = false;
        //3������ ����
        yield return new WaitForSeconds(0.3f); // 0.3 ���
        trailEffect.enabled = false;
        
        yield break;
        //yield break�� �ڷ�ƾ Ż�� ����
    }

    //Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ ( ���� ���� ) : �Ϲ��Լ�
    //Use() ���η�ƾ + Swing() �ڷ�ƾ ( ���� ���� ) : �ڷ�ƾ �Լ�
}
