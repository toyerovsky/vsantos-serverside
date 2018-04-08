/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

namespace VRP.Serverside.Entities.EventArgs
{
    public class ServerIdChangeEventArgs : System.EventArgs
    {
        public int LastId { get; set; }
        public int NewId { get; set; }

        public bool Cancel { get; set; }

        public ServerIdChangeEventArgs(int lastId, int newId)
        {
            LastId = lastId;
            NewId = newId;
        }
    }
}