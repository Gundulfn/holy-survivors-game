using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageMaker
{
    public static string makeMessage(object[] sections){    
        return string.Join(";", sections);;
    }
}
/*
public class Message
{
    const string SEPERATOR = ";";
    object[] sections;

    public Message() {}

    public Message(object[] sections) {
        this.sections = sections;
    }

    public string toString() {
        return string.Join(SEPERATOR, this.sections);
    }
}
*/
