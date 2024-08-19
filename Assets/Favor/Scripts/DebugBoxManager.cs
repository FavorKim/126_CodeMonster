using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugBoxManager : Singleton<DebugBoxManager>
{
    public TMP_Text Txt_DebugMsg;
    

    public void Log(string msg)
    {
        Txt_DebugMsg.text = msg;
    }

    
}
