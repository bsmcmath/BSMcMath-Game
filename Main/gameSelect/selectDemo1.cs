using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectDemo1 : UIoption
{
    public override void MakeSelection()
    {
        Main.main.streaming.loadScene(Main.main.streaming.demo1Index);
        Main.main.streaming.unloadScene(Main.main.streaming.gameSelectIndex);
    }
}
