/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Net.Sockets;
using System.Text;

namespace VRP.Core.Services.Model
{
    public class State
    {
        public Socket WorkSocket = null;
        public byte[] Buffer = new byte[1024];
        public StringBuilder RecievedData = new StringBuilder();
    }
}