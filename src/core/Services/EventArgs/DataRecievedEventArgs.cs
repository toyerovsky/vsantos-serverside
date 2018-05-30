/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

namespace VRP.Core.Services.EventArgs
{
    public class DataRecievedEventArgs : System.EventArgs
    {
        /// <summary>
        /// Action Type
        /// </summary>
        public string Header { get; set; }
        public string Json { get; set; }
    }
}