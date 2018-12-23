using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using UnityEngine;


public class GameDataManager : MonoBehaviour
{
    private float totalPlayTime;
    private Timer sessionTimer = new Timer();
    private string filepath;
    private XmlDocument xmlDoc = new XmlDocument();

    void OnApplicationQuit()
    {
        SaveData();
    }

    void Start()
    {
        sessionTimer.StarTimer();
        filepath = Application.dataPath + @"/Data/gamedata.xml";
        xmlDoc.Load(filepath);
        LoadData();
    }

    public void SaveData()
    {
        float sessionTime = sessionTimer.GetCurrentTime();
        totalPlayTime += sessionTime;

        XmlElement root = xmlDoc.DocumentElement;

        XmlElement session_element = xmlDoc.CreateElement("session"); 
        session_element.SetAttribute("date", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        XmlElement time_element = xmlDoc.CreateElement("time"); 
        time_element.InnerText = sessionTime.ToString();
        XmlElement total_time = xmlDoc.CreateElement("total_time");
        total_time.InnerText = totalPlayTime.ToString();

        
        session_element.AppendChild(time_element);
        session_element.AppendChild(total_time);

        root.AppendChild(session_element); 
        xmlDoc.Save(filepath); // save file.
    }

    public void LoadData()
    {
        XmlElement root = xmlDoc.DocumentElement;
        XmlNode last_session_node = root.LastChild;
        XmlNodeList nodeList = last_session_node.ChildNodes;

        foreach (XmlNode node in nodeList)
        {
            if (node.Name == "total_time")
            {
                totalPlayTime = float.Parse(node.InnerText); 
            }
        }
    }
}
