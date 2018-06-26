using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aim : MonoBehaviour //挂枪口Muzzle上
{
    public GameObject balls; //把Balls拖进去   
    Rigidbody2D[] allBall; //声明一个数组用来管理所有小球
    LineRenderer aimLine; //声明瞄准线
    public Transform CriticalPointLeft; //把左边界拖进去
    public Transform CriticalPointRight; //把右边界拖进去
    public float shootingSpeed = 3.5f; //小球发射速度
    public GameObject levelPanel; //把LevelPanel拖进去
    bool levelStop; //判断关卡是否已上升
    void Start()
    {
        Time.timeScale = 1; //游戏时间正常      
        allBall = balls.GetComponentsInChildren<Rigidbody2D>();//初始化(获取当前所有小球)
        aimLine = GetComponent<LineRenderer>(); //获取枪口上的LineRenderer组件
    }
    void Update()
    {
        //当游戏状态为活着时
        if (levelPanel.GetComponent<LevelMove>().levelState == LevelState.life)
        {
            if (Homing()) //所有小球都进入准备状态了
            {
                allBall = balls.GetComponentsInChildren<Rigidbody2D>(); //再次获取所有小球
                if (levelStop) //关卡处于未上升状态
                {
                    levelPanel.GetComponent<LevelMove>().LevelGetUp(); //调用关卡上升方法
                    levelStop = !levelStop; //关卡处于已上升状态
                }
                else
                    AimLaunch(); //关卡上升完成后可进行瞄准发射
            }
        }
    }
    //判断所有小球是否都进入准备状态
    public bool Homing() 
    {
        //发现任何小球不在准备状态都返回False
        for (int i = 0; i < allBall.Length; i++)
        {
            if (allBall[i].GetComponent<BallMove>().state != BallState.Ready)
                return false; 
        }
        return true; //未发现不在准备状态的小球，返回True
    }
    void AimLaunch() //瞄准发射
    {
        if (Input.GetMouseButtonDown(0)) //点击鼠标左键
        {
            aimLine.SetPosition(0, transform.position); //在枪口处生成瞄准线起点
        }
        if (Input.GetMouseButton(0)) //按住鼠标左键不放
        {
            //获取鼠标坐标
            Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //限制瞄准范围
            v = DirectionRestriction(v, CriticalPointLeft, CriticalPointRight);
            //将被限制过的鼠标坐标实时给瞄准线结束点
            aimLine.SetPosition(1, new Vector2(v.x, v.y)); 
        }
        if (Input.GetMouseButtonUp(0)) //抬起鼠标左键
        {
            StartCoroutine(LineLaunch(transform.position)); //启动协程发射小球
            aimLine.SetPosition(1, transform.position); //让结束点和起点重合(撤销瞄准线) 
            levelStop = !levelStop; //关卡标记为可上升状态
        }
    }
   
    IEnumerator LineLaunch(Vector3 muzzlePos) //用协程排队发射小球
    {
        Vector3 pos1 = aimLine.GetPosition(1);//获取瞄准线结束点坐标
        Vector3 directionAttack = (pos1 - muzzlePos).normalized;//获取瞄准结束点与枪口的方向向量
        for (int i = 0; i < allBall.Length; i++) //挨个发射小球
        {
            //被发射的小球变为战斗状态
            allBall[i].GetComponent<BallMove>().state = BallState.Battle;
            //球往瞄准结束点方向寻路移动  
            allBall[i].AddForce(directionAttack * shootingSpeed * Time.deltaTime);       
            yield return new WaitForSeconds(0.1f); //每隔0.1秒发射一个
        }
    }
    //限定枪口瞄准方向
    Vector3 DirectionRestriction(Vector3 v, Transform left, Transform right)
    {
        //最左不能左过左边界
        if (v.x < left.position.x) 
            v.x = left.position.x;
        //最右不能右过右边界
        if (v.x > right.position.x)
            v.x = right.position.x;
        //高度不能超过边界
        if (v.y > left.position.y)
            v.y = left.position.y;
        return v; //返回被限制后的坐标
    }
}
