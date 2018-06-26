using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 挂每个几何体上
/// </summary>
public class Enemy : MonoBehaviour 
{
    Text number; //声明数字
    private void Start()
    {
        number = GetComponentInChildren<Text>(); //找到子物体(数字)
    }
    private void Update()
    {
        if (Convert.ToInt32(number.text) < 1) //如果数字小于1时
            Destroy(gameObject); //销毁几何体自身
    }
}
