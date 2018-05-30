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