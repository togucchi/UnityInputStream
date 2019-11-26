using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Toguchi
{
    public class Utility
    {
        ///<summary>
        ///dataからjsonを作成してpathに書き出す
        ///</summary>
        public static void WriteJson<T>(string path, T data)
        {
            if (Directory.Exists(Path.GetDirectoryName(path)))
            {
                string json = JsonUtility.ToJson(data);

                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(path);
                bf.Serialize(file, json);
                file.Close();
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                WriteJson(path, data);
            }

        }

        ///<summary>
        ///pathに保存されているjsonを読み込みdataに格納
        ///</summary>
        public static void ReadJson<T>(string path, out T data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);

            string json = (string)bf.Deserialize(file);
            file.Close();
            data = JsonUtility.FromJson<T>(json);
        }
    }
}