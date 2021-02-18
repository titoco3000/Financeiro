using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class LogFile 
{

    public static void Log( object origin, string txt)
    {
        string doc = "";

        if(File.Exists(StorageManager.Path + "Log.txt"))
        {
            doc = File.ReadAllText(StorageManager.Path + "Log.txt");
        }
        else
        {
            File.Create(StorageManager.Path + "Log.txt");
        }
        if(origin == null)
            txt = txt + "\n [??]\n";
        else
            txt = txt + "\n [" + origin.ToString() + "]\n";

        Debug.Log(txt);
        doc +=  txt;
        File.WriteAllText(StorageManager.Path + "Log.txt", doc);

    }

}
