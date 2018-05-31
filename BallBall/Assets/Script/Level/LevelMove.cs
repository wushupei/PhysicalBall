using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LevelMove : MonoBehaviour //挂LevelPanel上
{
    public GameObject deathPanel;//把死亡菜单拖进去
    public LevelState levelState = LevelState.life; //游戏状态为存活
    List<Transform> lineList = new List<Transform>();//用集合分层管理每层关卡
    LevelCreate levelcreate; //声明创建关卡类
    private void Start()
    {
        levelcreate = GetComponent<LevelCreate>(); //获取创建关卡类
        lineList = GetAllChild(transform); //将所有子物体(不包括孙物体)添加进集合中   
        CreateLevel(); //游戏开始时创建一次关卡
    }
    private void Update()
    {
        if (levelState == LevelState.life && Input.GetKeyDown(KeyCode.Escape)) //在游戏运行时按Esc
        {
            levelState = LevelState.pause; //状态变为暂停 
            deathPanel.SetActive(true); //启用死亡菜单
            Time.timeScale = 0; //游戏暂停
        }
        else if(levelState == LevelState.pause&&Input.GetKeyDown(KeyCode.Escape)) //暂停状态时按Esc
        {
            levelState = LevelState.life; //状态变为运行
            deathPanel.SetActive(false); //禁用死亡菜单
            Time.timeScale = 1; //游戏暂停
        }
    }
    List<Transform> GetAllChild(Transform fatherObj) //获取所有子物体(不包括孙物体)
    {
        List<Transform> sonList = new List<Transform>();
        int number = fatherObj.childCount; //获取子物体数量
        for (int i = 0; i < number; i++)
        {
            sonList.Add(fatherObj.GetChild(i)); //将所有第一层子物体添加进集合中
        }
        return sonList;
    }
    void CreateLevel() //创建底层关卡
    {
        Transform last = lineList[lineList.Count - 1]; //获取最下层关卡,敌人将从该层产生
        List<Transform> sonList = GetAllChild(last); //获取最下层所有小方格

        Transform enemy = levelcreate.CreateEnemy(); //生成敌人
        int index = Random.Range(0, last.childCount); //随机定位一个格子
        enemy.position = last.GetChild(index).position; //将敌人创建在该格子
        enemy.parent = last.GetChild(index); //该敌人作为该格子的子物体可随关卡层移动

        Transform obj; //声明用于接收格子工厂里生产的东西
        for (int i = 0; i < sonList.Count; i++)
        {
            if (i != index) //除了刚才已经有敌人的格子外
            {
                obj = levelcreate.PaneFactory(); //生产东西
                if (obj != null) //如果成功生产出东西
                {
                    obj.position = sonList[i].position; //将该东西生产在此方格
                    obj.parent = sonList[i]; //作为该方格的子物体随关卡移动
                }
            }
        }
    }
    public void LevelGetUp() //关卡往上走一层(第一层跳到最后)
    {
        Vector3 tempPos = lineList[lineList.Count - 1].position; //获取最后层的坐标
        for (int i = lineList.Count - 1; i >= 0; i--)
        {
            if (i != 0) //除第一层以外
                lineList[i].position = lineList[i - 1].position; //都往自己前一层升一格
            else
                lineList[i].position = tempPos; //第一层直接跳到最后层
        }
        DestroyStunt(lineList[0]); //销毁顶层特效
        lineList.Add(lineList[0]); //将第一层添加到集合最后
        lineList.RemoveAt(0); //再移除第一层

        CreateLevel(); //创建关卡
        if (Death()) //判断是否死亡
        {
            levelState = LevelState.die; //状态变为死亡
            deathPanel.SetActive(true); //调用死亡菜单
        }
    }
    void DestroyStunt(Transform topLine) //销毁顶层特技
    {
        //获取该层所有子物体
        Transform[] lineSon = topLine.GetComponentsInChildren<Transform>(); 
        for (int i = 0; i < lineSon.Length; i++)
        {
            if (lineSon[i].tag == "Stunt") //如果是特技,则销毁
            {
                Destroy(lineSon[i].gameObject); //销毁
            }
        }
    }
    bool Death() //死亡判断
    {
        //获取该层所有子物体
        Transform[] lineSon = lineList[0].GetComponentsInChildren<Transform>(); 
        for (int i = 0; i < lineSon.Length; i++)
        {
            if (lineSon[i].tag == "Enemy") //如果子物体里有敌人标签的东西
                return true; //直接去世
        }
        return false; //如果一个都没有,游戏继续
    }   
}
