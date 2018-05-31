using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBall : MonoBehaviour //挂变大特效上
{
    private void OnCollisionEnter2D(Collision2D collision) //小球碰到它后
    {
        if (collision.gameObject.tag != "BigBall") //当普通小球碰到时
        {
            collision.transform.localScale = collision.transform.localScale * 1.2f;//小球变大
            collision.gameObject.tag = "BigBall"; //改变大球标签
        }
        Destroy(gameObject); //销毁脚本物体
    }
}
