using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;

public class DataSavingManager : MonoBehaviour
{
    public static DataSavingManager Instance { get ; private set; }
    [SerializeField] private String SavePath;
    [SerializeField] private bool UseDateDirectoryExtension = true;
    private string DateDirectoryExtension;

    private string _participantId;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }


        DateDirectoryExtension = GetDirectoryExtension();
        
        if (SavePath == "")
        {
            SavePath = Application.persistentDataPath;
        }

        if (Directory.Exists(SavePath))
        {
            SavePath = Path.Combine(SavePath, DateDirectoryExtension);
        }
        else
        {
            Debug.LogError("Path do not exist. Crash");
        }
    }
    
    private List <string> ConvertToJson<T>(List<T> genericList)
    {
        List<string> list = new List<string>();
        foreach (var g in genericList)
        {
           // Debug.Log(g.ToString());
            string jsonString = JsonUtility.ToJson(g);
            list.Add(jsonString);
        }
        
        return list;
    }
    
    private string ConvertToJson<T>(T generic)
    {
        string json= JsonUtility.ToJson(generic);
    
        return json;
    }

    

    public List<T> LoadFileList<T>(string FileName)
    {
        string path = GetPathForSaveFile(FileName);
        List<T> genericList=new List<T>();

        if (File.Exists(path))
        {
            string[] data = File.ReadAllLines(path);
            foreach (var line in data)
            {
                T tmp= JsonUtility.FromJson<T>(line);
                genericList.Add(tmp);
            }
            return genericList;
        }
        else
        {
            throw new Exception("file not found " + path);
        }
    }
    
    public T LoadFile<T>(string DataName)
    {
        string path = GetPathForSaveFile(DataName);
        if (File.Exists(path))
        {
            string[] data = File.ReadAllLines(path);
            T tmp= JsonUtility.FromJson<T>(data[0]);
            return tmp;
        }
        else
        {
            throw new Exception("file not found");
        }
    }
    
    public void Save<T>(T file, string  fileName)
    {
        var data = ConvertToJson(file);

        string path = GetPathForSaveFile(fileName);
        
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        
        FileStream fileStream= new FileStream(path, FileMode.Create);
        using (var fileWriter= new StreamWriter(fileStream))
        {
            fileWriter.WriteLine(data);
        }
        
        
        Debug.Log("saved  " +fileName + " to : " + SavePath );
    }
    
    
    public void SaveList<T>(List<T> file, string  fileName, bool pythonConform=false)         
    {
        var stringList = ConvertToJson(file);

        var path = GetPathForSaveFile(fileName);

        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        FileStream fileStream= new FileStream(path, FileMode.Create);
        using (var fileWriter= new StreamWriter(fileStream))
        {
            if(pythonConform)
                fileWriter.Write("[");
            
            fileWriter.WriteLine();
            for (int i = 0; i < stringList.Count; i++)
            {
               
                fileWriter.Write(stringList[i]);
                if(i<stringList.Count-1)
                    fileWriter.Write(",");
                fileWriter.WriteLine();
            }
            
            if(pythonConform)
                fileWriter.Write("]");
        }
        
        
        Debug.Log("saved  " +fileName + " to : " + SavePath );
    }


    private string GetDirectoryExtension()
    {
        var stringDate = String.Format("{0}-{1}-{2}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        return stringDate;
    }
    
    private string GetPathForSaveFile(string fileName, string format=".json")
    {
        string name = fileName + format;
        return Path.Combine(SavePath, name);
    }

    public string GetSavePath()
    {
        return SavePath;
    }
    
}
