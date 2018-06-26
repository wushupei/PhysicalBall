using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 挂LevelPanel上
/// </summary>
public class LevelMove : MonoBehaviour
{
    //需要做一个菜单，游戏死亡时弹出，在编辑器把死亡菜单拖进来
    public GameObject deathPanel;
    //游戏初始状态为存活
    public LevelState levelState = LevelState.life;
    //声明一个关卡集合用来管理每层关卡
    List<Transform> lineList = new List<Transform>();
    LevelCreate levelcreate; //声明创建物体的类
    private void Start()
    {
        levelcreate = GetComponent<LevelCreate>(); //获取创建关卡类
        lineList = GetAllChild(transform); //获取第一层子物体(关卡)添加进关卡集合中   
        CreateLevel(); //游戏开始时创建一次底层的物体
    }
    private void Update()
    {
        //在游戏运行时按Esc
        if (levelState == LevelState.life && Input.GetKeyDown(KeyCode.Escape))
        {
            levelState = LevelState.pause; //游戏状态变为暂停 
            deathPanel.SetActive(true); //启用菜单
            Time.timeScale = 0; //游戏暂停
        }
        //在暂停状态时按Esc
        else if (levelState == LevelState.pause && Input.GetKeyDown(KeyCode.Escape))
        {
            levelState = LevelState.life; //游戏状态变为运行
            deathPanel.SetActive(false); //禁用菜单
            Time.timeScale = 1; //游戏恢复
        }
    }
    List<Transform> GetAllChild(Transform fatherObj) //获取所有第一层子物体
    {
        //声明一个集合放第一层所有子物体
        List<Transform> sonList = new List<Transform>();
        int number = fatherObj.childCount; //获取第一层子物体数量
        for (int i = 0; i < number; i++)
        {
            //将所有第一层子物体添加进集合中
            sonList.Add(fatherObj.GetChild(i));
        }
        return sonList; //返回第一层子物体集合
    }
    void CreateLevel() //创建底层关卡
    {
        Transform last = lineList[lineList.Count - 1]; //获取底层关卡,物体将从该层产生
        List<Transform> sonList = GetAllChild(last); //获取底层所有小方格
        //生成一个几何体（每次创建关卡至少有一个几何体）
        Transform enemy = levelcreate.CreateEnemy();
        int index = Random.Range(0, last.childCount); //随机定位一个格子
        enemy.position = last.GetChild(index).position; //将几何体创建在该格子内
        enemy.parent = last.GetChild(index); //几何体作为该格子的子物体可随关卡层移动

        //然后在其它格子里随机生成物体
        for (int i = 0; i < sonList.Count; i++)
        {
            if (i != index) //除了刚才已经有敌人的格子外
            {
                //声明一个变量接受生成的物体
                Transform obj = levelcreate.PaneFactory();
                if (obj != null) //如果成功生产出东西
                {
                    obj.position = sonList[i].position; //将该东西生产在此方格
                    obj.parent = sonList[i]; //作为该方格的子物体随关卡层移动
                }
            }
        }
    }
    //关卡往上走一层(第一层跳到最后)
    public void LevelGetUp()
    {
        Vector3 tempPos = lineList[lineList.Count - 1].position; //获取最后层的坐标
        //遍历所有关卡层 
        for (int i = lineList.Count - 1; i >= 0; i--)
        {
            if (i == 0) //如果是顶层
                lineList[i].position = tempPos; //直接跳到底层
            else //如果是其它层
                lineList[i].position = lineList[i - 1].position; //移动到自己上一层
        }
        DestroyStunt(); //销毁顶层道具
        lineList.Add(lineList[0]); //将第一层添加到集合最后
        lineList.RemoveAt(0); //再移除第一层
        CreateLevel(); //创建一次底层关卡
        if (Death()) //判断是否死亡
        {
            levelState = LevelState.die; //状态变为死亡
            deathPanel.SetActive(true); //调用菜单
        }
    }
    void DestroyStunt() //销毁顶层特技
    {
        //获取该层所有子物体
        Transform[] lineSon = lineList[0].GetComponentsInChildren<Transform>();      
        for (int i = 0; i < lineSon.Length; i++) //遍历所有子物体的标签 
        {
            if (lineSon[i].tag == "Stunt") //如果是道具
            {
                Destroy(lineSon[i].gameObject); //销毁该子物体
            }
        }
    }
    bool Death() //死亡判断
    {
        //获取顶层所有子物体
        Transform[] lineSon = lineList[0].GetComponentsInChildren<Transform>();
        for (int i = 0; i < lineSon.Length; i++) //遍历所有子物体的标签 
        {
            if (lineSon[i].tag == "Enemy") //如果发现有几何体
                return true; //直接游戏结束
        }
        return false; //如果一个都没有,游戏继续
    }
}
