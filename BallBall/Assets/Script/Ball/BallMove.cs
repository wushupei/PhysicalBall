
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMove : MonoBehaviour //挂小球Ball上
{
    float timer; //计时用
    public BallState state = BallState.Ready; //初始状态为准备状态
    private void OnCollisionEnter2D(Collision2D collision) //碰撞时调一次
    {
        if (state == BallState.Battle) //如果在战斗阶段
        {
            GetComponent<Rigidbody2D>().gravityScale = 1; //碰到东西后重力为1
            if (collision.gameObject.tag == "Enemy") //如果碰到敌人    
            {
                Text enemyNumber = collision.transform.GetChild(0).GetComponent<Text>();//获取敌人数字
                Text Score = GameObject.Find("ScoreText").GetComponent<Text>(); //获取分数
                if (tag == "BigBall") //如果是大球
                {
                   enemyNumber.text = ((System.Convert.ToInt32(enemyNumber.text)) - 2).ToString();//敌人数字-2
                   Score.text = ((System.Convert.ToInt32(Score.text)) + 2).ToString();//分数+2
                }
                else //如果是小球
                {
                    enemyNumber.text = ((System.Convert.ToInt32(enemyNumber.text)) - 1).ToString();//敌人数字-1
                    Score.text = ((System.Convert.ToInt32(Score.text)) + 1).ToString();//分数+1
                }
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision) //碰撞时持续调用,防止小球被卡住
    {
        if (collision.gameObject.tag == "Enemy") //如果碰撞的是敌人
        {
            timer += Time.deltaTime; //开始计时
            if (timer > 1) //一秒后还停留在那上面
            {
                switch (Random.Range(0, 4)) //随机方向弹开
                {
                    case 0:
                        GetComponent<Rigidbody2D>().AddForce(transform.up * 0.01f);
                        break;
                    case 1:
                        GetComponent<Rigidbody2D>().AddForce(-transform.up * 0.01f);
                        break;
                    case 2:
                        GetComponent<Rigidbody2D>().AddForce(transform.right * 0.01f);
                        break;
                    case 3:
                        GetComponent<Rigidbody2D>().AddForce(-transform.up * 0.01f);
                        break;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision) //离开碰撞时调一次
    {
        timer = 0; //计时归零
    }
    private void Update()
    {
        transform.Rotate(0, 0, 0.0001f); //物体处于运动状态,持续碰撞才会生效
        switch (state)
        {
            case BallState.Bore: //上膛阶段
                GetComponent<Rigidbody2D>().gravityScale = 0; //重力变为0
                break;
            case BallState.Ready: //准备阶段
                GetComponent<CircleCollider2D>().isTrigger = false; //关闭触发
                GetComponent<Rigidbody2D>().Sleep(); //小球停止不动
                break;
        }
    }
}