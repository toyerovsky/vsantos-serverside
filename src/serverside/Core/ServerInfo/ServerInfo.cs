﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.IO;
using System.Linq;
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.Serverside.Core.ServerInfo.Models;

namespace VRP.Serverside.Core.ServerInfo
{
    [Serializable]
    public class ServerInfo
    {
        public ServerInfoModel Model { get; set; }

        private static ServerInfo _instance;
        public static ServerInfo Instance => _instance = _instance ?? new ServerInfo();

        public ServerInfo()
        {
            string directory = Path.Combine(Utils.XmlDirectory, "ServerInfo\\");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(Path.Combine(directory, "ServerInfo.xml")))
                XmlHelper.AddXmlObject(new ServerInfoModel(), directory, "ServerInfo");

            Model = XmlHelper.GetXmlObjects<ServerInfoModel>(directory).First();
        }

        public void Save()
        {

        }
    }
}