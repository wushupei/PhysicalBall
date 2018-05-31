using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aim : MonoBehaviour //挂枪口Muzzle上
{
    public GameObject balls; //把Balls拖进去   
    Rigidbody2D[] allBall; //用来管理所有小球
    LineRenderer aimLine; //声明瞄准线
    public GameObject levelPanel; //把LevelPanel拖进去
    bool levelStop; //关卡是否已上升
    void Start()
    {        
        Time.timeScale = 1; //游戏时间正常      
        GetAllBall();//初始化(获取当前所有小球)
        aimLine = GetComponent<LineRenderer>(); //获取画线组件
    }
    void Update()
    {
        //当游戏状态为活着时
        if (levelPanel.GetComponent<LevelMove>().levelState == LevelState.life) 
        {
            if (Homing()) //如果所有小球都进入准备状态了
            {
                GetAllBall(); //再次初始化
                if (levelStop) //如果关卡未上升
                {       
                    levelPanel.GetComponent<LevelMove>().LevelGetUp(); //调用关卡上升方法
                    levelStop = !levelStop; //关卡标记为已上升
                }
                AimLaunch(); //可进行瞄准发射
            }
        }
    }
    void AimLaunch() //瞄准发射
    {
        if (Input.GetMouseButtonDown(0)) //点击鼠标左键
        {
            aimLine.SetPosition(0, transform.position); //在枪口处生成瞄准线起点
        }
        if (Input.GetMouseButton(0)) //按住鼠标左键不放
        {
            Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            //限制瞄准范围(选用)
            if (v.x < 538)
                v.x = 538;
            if (v.x > 542)
                v.x = 542;
            if (v.y > 1143.5f)
                v.y = 1143.5f;
            if (v.y < 1136.5f)
                v.y = 1136.5f;
            aimLine.SetPosition(1, new Vector2(v.x, v.y)); //将鼠标位置实时给瞄准点
        }
        if (Input.GetMouseButtonUp(0)) //抬起鼠标左键
        {
            StartCoroutine(LineLaunch(transform.position)); //发射
            aimLine.SetPosition(1, transform.position); //让结束点和起点重合(撤销瞄准线) 
            levelStop = !levelStop; //关卡标记为可上升状态
        }
    }
    void GetAllBall() //获取所有小球
    {
        allBall = balls.GetComponentsInChildren<Rigidbody2D>(); //获取当前所有小球
    }  
    public bool Homing() //判断所有小球是否都进入准备状态
    {
        for (int i = 0; i < allBall.Length; i++)
        {
            if (allBall[i].GetComponent<BallMove>().state != BallState.Ready) 
                return false; //发现任何小球不在准备状态都返回False
        }
        return true;
    }
    IEnumerator LineLaunch(Vector3 muzzlePos) //用协程排队发射小球
    {
        Vector3 pos1 = aimLine.GetPosition(1);//获取瞄准结束点坐标
        Vector3 force1 = (pos1 - muzzlePos).normalized; //获取瞄准结束点与枪口的方向向量
        for (int i = 0; i < allBall.Length; i++) //挨个发射小球
        {
            allBall[i].GetComponent<BallMove>().state = BallState.Battle;//小球变为战斗状态
            allBall[i].AddForce(force1 * 0.05f); //球往瞄准结束点方向移动        
            yield return new WaitForSeconds(0.1f); //每隔0.1秒发射一个
        }
    }
}
