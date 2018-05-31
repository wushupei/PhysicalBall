using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFindWay : MonoBehaviour //FindTheWays下的所有子物体都挂一个
{
    public Transform muzzle; //把编辑器把枪口拖进去
    private void OnTriggerStay2D(Collider2D ball) //触发时持续调用
    {
        switch (name) //根据碰撞器的名字决定施加力的方向
        {
            case "LeftDown":
                ball.GetComponent<Rigidbody2D>().AddForce(-transform.right * 0.002f);
                break;
            case "RightDown":
                ball.GetComponent<Rigidbody2D>().AddForce(transform.right * 0.003f);
                break;
            case "Left":
            case "Right":
                ball.GetComponent<Rigidbody2D>().AddForce(transform.up * 0.002f);
                break;
            case "Up":               
                StartCoroutine(MoveToMuzzle(ball.transform, muzzle)); //启动协程寻路              
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D ball) //离开触发时调用
    {
        if(name=="Up") 
            ball.GetComponent<CircleCollider2D>().isTrigger = true;//打开触发器,使其能越过枪口阀门
    }
    public IEnumerator MoveToMuzzle(Transform ball, Transform muzzle) //小球朝枪口处移动
    {
        ball.GetComponent<BallMove>().state = BallState.Bore; //小球状态改为上膛状态
        while (ball.GetComponent<BallMove>().state == BallState.Bore) //如果是上膛状态
        {          
            //小球往枪口处寻路,完成上膛
            ball.position = Vector3.MoveTowards(ball.position, muzzle.position, 0.2f * Time.deltaTime);
            yield return new WaitForFixedUpdate(); //每帧执行一下
            if ((ball.position - muzzle.position).sqrMagnitude <= 0.001f) //如果接近枪口
            {
                ball.GetComponent<BallMove>().state = BallState.Ready; //状态变为准备阶段
                ball.position = muzzle.position; //将枪口位置为小球
            }
        }
    }
}
