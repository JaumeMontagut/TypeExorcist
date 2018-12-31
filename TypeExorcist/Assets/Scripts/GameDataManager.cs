using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using UnityEngine;


public class GameDataManager : MonoBehaviour
{
    private PlayerController player = null;
    private float totalPlayTime;
    private Timer sessionTimer = new Timer();
    private string filePath;
    private XmlDocument xmlDoc = new XmlDocument();
    private StreamWriter txtDoc;
    private int sessionCount = 0;

    public void OnDestroy()
    {
        SaveData();
    }

    void Start()
    {
        sessionTimer.StarTimer();
        player = FindObjectOfType<PlayerController>();
        txtDoc = new StreamWriter(Application.dataPath + "/Data/DataTxt.txt", true);
        filePath = Application.dataPath + "/Data/DataXml.xml";
        xmlDoc.Load(filePath);
        LoadData();
    }

    public void SaveData()
    {
        // Save XML =============================================
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
        xmlDoc.Save(filePath);

        // Save Txt =============================================

        XmlNodeList nodeList = root.ChildNodes;

        foreach (XmlNode node in nodeList)
        {
            ++sessionCount;
        }

        string line_1;
        line_1 = "---- " + "Session: " + sessionCount.ToString() + "  "+ "Date: " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " -----------------------";
        txtDoc.WriteLine(line_1);
        string line_2;
        line_2 = "Time:        " + sessionTime.ToString();
        txtDoc.WriteLine(line_2);
        string line_3;
        line_3 = "Total Time:  " + totalPlayTime.ToString();
        txtDoc.WriteLine(line_3);
        string line_4;
        line_4 = "Mistakes:    " + player.mistakes;
        txtDoc.WriteLine(line_4);
        string line_5;
        line_5 = "Correct:     " + player.correct;
        txtDoc.WriteLine(line_5);
        string line_6;
        line_6 = "Accuracy:    " + player.GetAccuracy() + "%";
        txtDoc.WriteLine(line_6);

        txtDoc.Close();
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
