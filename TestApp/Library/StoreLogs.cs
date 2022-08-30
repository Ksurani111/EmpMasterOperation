namespace TestApp.Library
{
    public class StoreLogs
    {
        public void SaveLogs(string error)
        {
            /// <summary>
            ///  Will store the logs
            /// </summary>
            System.IO.File.WriteAllText(@"c:\Error.txt", error);
        }
    }
}
