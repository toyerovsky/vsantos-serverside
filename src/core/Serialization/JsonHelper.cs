﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace VRP.Core.Serialization
{
    public static class JsonHelper
    {
        public static List<T> GetJsonObjects<T>(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Colorful.Console.WriteLine($"[INFO][{nameof(JsonHelper)}] Created path: {path}", Color.CornflowerBlue);
            }

            return Directory.GetFiles(path).Select(JsonConvert.DeserializeObject<T>).ToList();
        }

        public static void AddJsonObject<T>(T value, string path, string fileName = "")
        {
            if (path.Last() != '\\')
            {
                Colorful.Console.WriteLine($"[ERROR][{nameof(JsonHelper)}] Specified path is inappropriate: {path}", Color.DarkRed);
                return;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Colorful.Console.WriteLine($"[INFO][{nameof(JsonHelper)}] Created path: {path}", Color.CornflowerBlue);
            }

            if (fileName == string.Empty)
            {
                List<T> collection = GetJsonObjects<T>(path);
                int index = collection.Count;

                do ++index;
                while (File.Exists($"{path}{index}.json"));

                fileName = index.ToString();
            }

            string json = JsonConvert.SerializeObject(value);

            if (!File.Exists($"{path}{fileName}.json"))
                File.Create($"{path}{fileName}.json");

            File.AppendAllText($"{path}{fileName}.json", json);
            
        }
    }
}