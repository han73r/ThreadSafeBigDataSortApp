namespace SorterApp.Utilities {
    internal class GlobalSettings {
        private static readonly Lazy<string> _resultsFolder = new Lazy<string>(() => {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "results");
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        });
        public static string RESULTS_FOLDER => _resultsFolder.Value;
        public const int MB_TO_BYTE = 1024 * 1024;
        public const int BYTE_TO_MB = 1024 / 1024;
        public const int MIN_CHUNK_SIZE_MB = 1;
        public const string TIMESTAMP_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public static string GetCurrentTimestamp() {
            return DateTime.Now.ToString(TIMESTAMP_FORMAT);
        }
    }
}