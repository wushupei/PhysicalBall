using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 挂LevelPanel上
/// </summary>
public class LevelCreate : MonoBehaviour
{
    //在编辑器中将当前分数ScoreText拖进去
    public Text scoreText;
    //几何体数组
    Transform[] enemys;
    //道具数组
    Transform[] stunts;
    void Awake()
    {
        //将所有几何体和道具加载到数组中
        enemys = LoadPrefab("Prefab/EnemyPrefab"); 
        stunts= LoadPrefab("Prefab/PropPrefab");
    }
    //加载预制件体返回数组
    public Transform[] LoadPrefab(string path)
    {
        Object[] obj = Resources.LoadAll(path);
        Transform[] th = new Transform[obj.Length];
        for (int i = 0; i < th.Length; i++)
        {
            th[i] = (obj[i] as GameObject).transform;
        }
        return th;
    }
    //物体生成器,决定格子里是否生成东西（几率可自行设定）
    public Transform PaneFactory()
    {
        int chance = Random.Range(0, 4);
        if (chance < 3)          //75%不产生东西,
            return null;
        else                     //25%产生东西
            return PaneManage();
    }
    //决定格子里该生产什么东西
    Transform PaneManage()
    {
        int chance = Random.Range(0, 3);
        if (chance < 2)           //66%产生几何体
            return CreateEnemy();
        else                      //33%产生道具
            return CreateStunt();
    }
    //随机生成道具
    Transform CreateStunt()
    {
        //随机产生一个道具数组索引
        int index = Random.Range(0, stunts.Length);
        //生成该索引处道具
        return Instantiate(stunts[index]);
    }
    //随机生成敌人
    public Transform CreateEnemy()
    {
        //随机产生一个几何体数组索引
        int index = Random.Range(0, enemys.Length);
        //生成该索引处几何体
        Transform enemy = Instantiate(enemys[index]);
        //给几何体赋一个随机颜色
        enemy.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
        //给几何体一个随机旋转角度
        enemy.rotation = Quaternion.Euler(0, 0, Random.Range(0, 90));
        //获取几何体子物体数字的Transform组件
        Transform tf = enemy.GetComponentInChildren<Text>().transform;
        //子物体不旋转
        tf.rotation = Quaternion.Euler(0, 0, 0);
        //获取当前分数
        int score = System.Convert.ToInt32(scoreText.text);
        if (score < 100) //如果当前分数不超过100分
            //几何体数字在 1~9 之间随机生成
            enemy.GetComponentInChildren<Text>().text = Random.Range(1, 10).ToString();
        else //当前分数超过100分
            //几何体数字在 1~当前分数/10 之间随机生成
            enemy.GetComponentInChildren<Text>().text = Random.Range(1, score / 10).ToString();
        return enemy;
    }
}
