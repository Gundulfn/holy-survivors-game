using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageMaker
{
    public static string makeMessage(object[] sections){    
        return string.Join(";", sections);;
    }
}