using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyBall : MonoBehaviour //挂复制特效上
{
    public Transform ball; //把小球预制件拖进去
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform newBall = Instantiate(ball); //复制一个新小球
        newBall.parent = collision.transform.parent; //认旧小球的父亲做爹
        newBall.position = collision.transform.position; //把旧小球的位置给它
        newBall.GetComponent<Rigidbody2D>().gravityScale = 1; //新小球重力为1
        newBall.GetComponent<BallMove>().state = BallState.Battle; //新小球状态为战斗状态
        //新小球往右跳     
        newBall.GetComponent<Rigidbody2D>().AddForce(transform.right * 0.02f);
        //旧小球往左跳
        collision.transform.GetComponent<Rigidbody2D>().AddForce(-transform.right * 0.02f);
        Destroy(gameObject); //销毁脚本物体
    }
}
