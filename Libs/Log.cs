using System.Diagnostics;

namespace LifeInDiary.Libs
{
    class Log
    {
        public static void Info(object o)
        {
            Debug.WriteLine($"[INFO] {o}");
        }
        public static void Success(object o)
        {
            Debug.WriteLine($"[SUCCESS] {o}");
        }
        public static void Error(object o)
        {
            Debug.WriteLine($"[ERROR] {o}");
        }
        public static void Warning(object o)
        {
            Debug.WriteLine($"[WARNING] {o}");
        }
    }
}
