using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour //挂每个敌人预制体上
{
    Text son;
    private void Start()
    {
        son = GetComponentInChildren<Text>(); //找到儿子(数字)
    }
    private void Update()
    {
        if (Convert.ToInt32(son.text) < 1) //如果数字小于1时
            Destroy(gameObject); //销毁自身
    }
}
