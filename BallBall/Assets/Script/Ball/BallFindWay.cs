using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// FindTheWays下的所有寻路碰撞器都挂一个
/// </summary>
public class BallFindWay : MonoBehaviour
{
    public Transform muzzle; //在编辑器把枪口拖进去，我们需要枪口的坐标
    public float boreSpeed=0.2f; //上膛速度
    private void OnTriggerStay2D(Collider2D ball) //触发时持续调用
    {
        //获取小球的刚体
        Rigidbody2D r2d = ball.GetComponent<Rigidbody2D>(); 
        switch (name) //根据寻路碰撞器的名字决定施加力的方向
        {
            case "LeftDown":
                r2d.AddForce(-transform.right * 0.002f);
                break;
            case "RightDown":
                r2d.AddForce(transform.right * 0.003f);
                break;
            case "Left":
            case "Right":
                r2d.AddForce(transform.up * 0.002f);
                break;
            case "Up":
                //启动协程寻路（上膛）   
                StartCoroutine(MoveToMuzzle(ball.transform, muzzle)); 
                //打开小球触发器,使小球能越过枪口阀门
                ball.GetComponent<CircleCollider2D>().isTrigger = true;
                break;
        }
    }
    //使用协程寻路让小球朝枪口处移动
    public IEnumerator MoveToMuzzle(Transform ball, Transform muzzle) 
    {
        ball.GetComponent<BallMove>().state = BallState.Bore; //小球状态改为上膛状态
        while (ball.GetComponent<BallMove>().state == BallState.Bore) //如果是上膛状态
        {          
            //小球往枪口处寻路,完成上膛           
            ball.position = Vector3.MoveTowards(ball.position, muzzle.position, boreSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate(); //每次循环间隔1帧
            //如果小球位置和枪口位置接近
            if ((ball.position - muzzle.position).sqrMagnitude <= 0.001f) 
            {
                ball.GetComponent<BallMove>().state = BallState.Ready; //小球进入准备阶段
                ball.position = muzzle.position; //将小球定在枪口位置
            }
        }
    }
}
