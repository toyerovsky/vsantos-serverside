/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using GTANetworkInternals;

namespace Serverside.Jobs.Base
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