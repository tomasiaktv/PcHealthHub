
namespace PcHealthHub.Utility
{
    public static class FriendlyFileSize
    {
        private static readonly string[] Units =
        {
            "B",
            "KB",
            "MB",
            "GB",
            "TB",
            "PB"
        };

        public static string FormatBytes(long bytes)
        {
            double size = bytes;
            int unitIndex = 0;

            while (size >= 1024 && unitIndex < Units.Length - 1)
            {
                size /= 1024;
                unitIndex++;
            }

            long roundedSize = (long)Math.Ceiling(size);

            return $"{roundedSize} {Units[unitIndex]}";
        }
    }
}
