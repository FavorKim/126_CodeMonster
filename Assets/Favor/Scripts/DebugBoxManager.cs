using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugBoxManager : Singleton<DebugBoxManager>
{
    public TMP_Text Txt_DebugMsg;
    

    public void Log(string msg)
    {
        if(Txt_DebugMsg != null)
        Txt_DebugMsg.text += msg + '\n';
            
    }

    public void Log()
    {
        if(Txt_DebugMsg != null)
        Txt_DebugMsg.text += "triggered";
    }

    public void ClearText()
    {
        if(Txt_DebugMsg != null)
        Txt_DebugMsg.text = string.Empty;
    }
    
}
