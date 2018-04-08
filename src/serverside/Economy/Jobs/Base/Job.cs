/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;

namespace VRP.Serverside.Economy.Jobs.Base
{
    public abstract class Job
    {
        public List<JobWorkerController> Workers { get; set; } = new List<JobWorkerController>();

        public string JobName { get; set; }
        public decimal MoneyLimit { get; set; }
        public string JsonDirectory { get; set; }

        protected Job(string jobName, decimal moneyLimit, string xmlDirectory)
        {
            JobName = jobName;
            MoneyLimit = moneyLimit;
            JsonDirectory = xmlDirectory;
        }
    }
}