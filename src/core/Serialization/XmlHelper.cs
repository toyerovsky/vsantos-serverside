/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Color = System.Drawing.Color;

namespace VRP.Core.Serialization
{
    public static class XmlHelper
    {
        public static void AddXmlObject<T>(T xmlObject, string path, string fileName = "")
        {
            if (path.Last() != '\\')
            {
                Colorful.Console.WriteLine(($"[Warning][{nameof(XmlHelper)}] Specified path is inappropriate: {path} " +
                                         $"\nAdding object canceled.", Color.Yellow));
                return;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Colorful.Console.WriteLine(($"[Info][{nameof(XmlHelper)}] Created path: {path}", Color.CornflowerBlue));
            }

            if (fileName == string.Empty)
            {
                List<T> collection = GetXmlObjects<T>(path);
                int index = collection.Count;

                do ++index;
                while (File.Exists($"{path}{index}.xml"));

                fileName = index.ToString();
            }


            using (FileStream xmlStream = File.Create($"{path}{fileName}.xml"))
            {
                try
                {
                    XmlSerializer writerSerializer = new XmlSerializer(typeof(T));
                    writerSerializer.Serialize(xmlStream, xmlObject);
                }
                catch (InvalidOperationException ex)
                {
                    Colorful.Console.WriteLine(($"[Error][{nameof(XmlHelper)}] Serialization unsuccessful. " +
                                             $"\n Exception: {ex.Message}", Color.DarkRed));
                }
            }
        }

        public static List<T> GetXmlObjects<T>(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Colorful.Console.WriteLine($"[Info][{nameof(XmlHelper)}] Created path: {path}", Color.CornflowerBlue);
            }

            XmlSerializer readerSerializer = new XmlSerializer(typeof(T));
            string[] posDirs = Directory.GetFiles(path);

            List<T> xmlPositions = new List<T>();
            //Dla kazdej sciezki dodaj odpowiadajacy jej .xml do listy
            foreach (string item in posDirs)
            {
                FileStream stream = new FileStream(item, FileMode.Open);
                if (Path.GetExtension(item) == ".xml")
                {
                    xmlPositions.Add((T)readerSerializer.Deserialize(stream));
                }
                stream.Close();
                stream.Dispose();
            }
            return xmlPositions;
        }

        public static bool TryDeleteXmlObject(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Colorful.Console.WriteLine($"[Warning][{nameof(XmlHelper)}] Tried to delete file which not exists.\n " +
                                         $"Path: {filePath}", Color.Yellow);
                return false;
            }

            File.Delete(filePath);
            return true;
        }
    }
}