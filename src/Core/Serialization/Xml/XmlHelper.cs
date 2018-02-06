/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using GTANetworkAPI;

namespace Serverside.Core.Serialization.Xml
{
    public static class XmlHelper
    {
        /// <summary>
        /// Ścieżkę należy dostarczyć z \ na końcu
        /// </summary>
        /// <typeparam Name="T"></typeparam>
        /// <param Name="xmlObject"></param>
        /// <param Name="path"></param>
        /// <param Name="fileName"></param>
        public static void AddXmlObject<T>(T xmlObject, string path, string fileName = "")
        {
            if (path.Last() != '\\')
            {
                Tools.ConsoleOutput($"[Warning][{nameof(XmlHelper)}] Podano nieprawidłową ścieżkę: {path} " +
                                         $"\nDodanie obiektu anulowane.", ConsoleColor.Yellow);
                return;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Tools.ConsoleOutput($"[Info][{nameof(XmlHelper)}] Utworzono ścieżkę: {path}", ConsoleColor.Blue);
            }

            if (fileName == string.Empty)
            {
                var collection = GetXmlObjects<T>(path);
                int index = collection.Count;

                do ++index;
                while (File.Exists($"{path}{index}.xml"));

                fileName = index.ToString();
            }


            using (FileStream xmlStream = File.Create($"{path}{fileName}.xml"))
            {
                try
                {
                    var writerSerializer = new XmlSerializer(typeof(T));
                    writerSerializer.Serialize(xmlStream, xmlObject);
                }
                catch (InvalidOperationException ex)
                {
                    Tools.ConsoleOutput($"[Error][{nameof(XmlHelper)}] Serializacja nieudana. " +
                                             $"\n Wyjątek: {ex.Message}", ConsoleColor.Red);
                }
            }
        }

        /// <summary>
        /// Ścieżkę należy dostarczyć z \ na końcu
        /// </summary>
        /// <typeparam Name="T"></typeparam>
        /// <param Name="path"></param>
        /// <returns></returns>
        public static List<T> GetXmlObjects<T>(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Tools.ConsoleOutput($"[Info][{nameof(XmlHelper)}] Utworzono ścieżkę: {path}", ConsoleColor.Blue);
            }

            var readerSerializer = new XmlSerializer(typeof(T));
            var posDirs = Directory.GetFiles(path);

            var xmlPositions = new List<T>();
            //Dla kazdej sciezki dodaj odpowiadajacy jej .xml do listy
            foreach (var item in posDirs)
            {
                var stream = new FileStream(item, FileMode.Open);
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
                Tools.ConsoleOutput($"[Warning][{nameof(XmlHelper)}] Próbowano usunąć plik który nie istnieje.\n " +
                                         $"Ścieżka: {filePath}", ConsoleColor.Yellow);
                return false;
            }

            File.Delete(filePath);
            return true;
        }
    }
}