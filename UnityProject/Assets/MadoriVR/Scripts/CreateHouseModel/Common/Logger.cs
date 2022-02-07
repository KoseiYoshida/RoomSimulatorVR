namespace MadoriVR.Scripts.CreateHouseModel.Common
{
    // TODO: ロギングの仕組みを整える。今はとりあえず呼び出すメソッドの一本化だけしておく。
    public static class Log
    {
        private const string Prefix = "[RoomCreation] ";

        public static void Debug(string msg)
        {
            // FIX: Editor/DevelopmentBuild時のみの出力などにしたい。
            UnityEngine.Debug.Log($"{Prefix} + msg");
        }
        
        public static void Info(string msg)
        {
            UnityEngine.Debug.Log($"{Prefix} + msg");
        }
        
        public static void Warning(string msg)
        {
            UnityEngine.Debug.LogWarning($"{Prefix} + msg");
        }
        
        public static void Error(string msg)
        {
            UnityEngine.Debug.LogError($"{Prefix} + msg");
        }
    }
}