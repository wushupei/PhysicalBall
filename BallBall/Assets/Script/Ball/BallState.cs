using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 小球状态机,不用挂在任何物体上
/// </summary>
public enum BallState 
{   
    Ready, //准备阶段
    Battle, //战斗阶段
    Bore, //上膛阶段
}
