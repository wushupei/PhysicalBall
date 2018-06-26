using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// //挂变大道具上
/// </summary>
public class BigBall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) //被碰撞是调用
    {
        if (collision.gameObject.tag != "BigBall") //当普通小球碰到时
        {
            collision.transform.localScale *= 1.2f;//小球变大20%成打球
            collision.gameObject.tag = "BigBall"; //大球的标签设为“BigBall”
        }
        Destroy(gameObject); //销毁变大道具
    }
}
