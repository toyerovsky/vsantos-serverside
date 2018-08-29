/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VRP.BLL.Serialization
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

        public static async Task AddJsonObject<T>(T value, string path, string fileName = "")
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
            byte[] data = Encoding.UTF8.GetBytes(json);

            using (FileStream file = File.Open($"{path}{fileName}.json", FileMode.OpenOrCreate))
                await file.WriteAsync(data, 0, data.Length);
        }
    }
}