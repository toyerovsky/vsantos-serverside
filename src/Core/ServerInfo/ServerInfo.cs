/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.IO;
using System.Linq;
using Serverside.Core.Serialization.Xml;
using Serverside.Core.ServerInfo.Models;

namespace Serverside.Core.ServerInfo
{
    [Serializable]
    public class ServerInfo
    {
        public ServerInfoModel Model { get; set; }

        private static ServerInfo _instance;
        public static ServerInfo Instance => _instance = _instance ?? new ServerInfo();

        public ServerInfo()
        {
            if (!File.Exists($"{Constant.ServerInfo.XmlDirectory}ServerInfo\\ServerInfo.xml"))
                XmlHelper.AddXmlObject(new ServerInfoModel(), $"{Constant.ServerInfo.XmlDirectory}ServerInfo\\", "ServerInfo");

            Model = XmlHelper.GetXmlObjects<ServerInfoModel>(
                $"{Constant.ServerInfo.XmlDirectory}ServerInfo\\").First();
        }

        public void Save()
        {
            
        }
    }
}