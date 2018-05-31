using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCreate : MonoBehaviour//挂LevelPanel上
{
    public Text scoreText; //把当前分数ScoreText拖进去
    public Transform[] Enemys; //所有种类敌人,把所有敌人预制件拖进去
    public Text numberText; //把NumberText预制件拖进去
    public Transform[] stunts; //所有种类特效,把左右特效预制件拖进去
    public Transform PaneFactory() //格子工厂,决定格子里是否生成东西
    {
        int chance = Random.Range(0, 4); //25%产生东西,75%不产生东西
        if (chance < 3)
            return null;
        else
            return PaneManage(); //生产东西
    }
    Transform PaneManage() //格子管理,决定该格子生产什么东西
    {
        int chance = Random.Range(0, 3); //33%产生特技,66%产生敌人
        if (chance < 2)
            return CreateEnemy(); //生产敌人
        else
            return CreateStunt(); //生产特技
    }  
    Transform CreateStunt() //随机生成特技
    {
        int index = Random.Range(0, stunts.Length);//随机产生一个索引,生成相应特技
        return Instantiate(stunts[index]);
    }
    Transform CreateNumber() //随机生成敌人数字(受当前分数scoreText的影响)
    {
        Text number = Instantiate(numberText);       
        int score = System.Convert.ToInt32(scoreText.text); //获取当前分数
        if (score < 100)
            number.text = Random.Range(1, 10).ToString(); //百分以内
        else
            number.text = Random.Range(1, score / 10).ToString(); //百分之后数字随当前分数逐渐变大     
        return number.transform;
    }
    public Transform CreateEnemy() //随机生成敌人
    {
        int index = Random.Range(0, Enemys.Length); //随机产生一个索引,在所有敌人中挑一个创建
        Transform enemy = Instantiate(Enemys[index]); 
        enemy.rotation = Quaternion.Euler(0, 0, Random.Range(0, 45)); //给一个随机旋转角度
        //随机初始化颜色
        enemy.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value); 
        Transform number = CreateNumber(); //生成数字
        number.position = enemy.position; //将当前敌人坐标给它
        number.parent = enemy; //认当前敌人做父物体,跟着它移动
        return enemy;
    }
}
