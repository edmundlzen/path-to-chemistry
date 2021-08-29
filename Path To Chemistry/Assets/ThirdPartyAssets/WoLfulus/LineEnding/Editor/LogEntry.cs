using System;
using System.Reflection;
using UnityEditor;

namespace WoLfulus.LineEnding
{
    public static class LogEntry
    {
        public static object instance;

        private static Type _type;

        private static FieldInfo _condition;
        private static FieldInfo _errorNum;
        private static FieldInfo _file;
        private static FieldInfo _line;
        private static FieldInfo _mode;
        private static FieldInfo _instanceID;
        private static FieldInfo _identifier;
        private static FieldInfo _isWorldPlaying;

        static LogEntry()
        {
            Initialize();
        }

        public static string condition => (string) _condition.GetValue(instance);

        public static int errorNum => (int) _errorNum.GetValue(instance);

        public static string file => (string) _file.GetValue(instance);

        public static int line => (int) _line.GetValue(instance);

        public static Mode mode => (Mode) (int) _mode.GetValue(instance);

        public static int instanceID => (int) _instanceID.GetValue(instance);

        public static int identifier => (int) _identifier.GetValue(instance);

        public static int isWorldPlaying => (int) _isWorldPlaying.GetValue(instance);

        private static void Initialize()
        {
            if (_type == null)
            {
                var flags = BindingFlags.Instance | BindingFlags.Public;

                var assembly = Assembly.GetAssembly(typeof(Editor));
                _type = assembly.GetType("UnityEditorInternal.LogEntry");
                if (_type == null) // 2017 Fix
                    _type = assembly.GetType("UnityEditor.LogEntry");

                _condition = _type.GetField("condition", flags);
                _errorNum = _type.GetField("errorNum", flags);
                _file = _type.GetField("file", flags);
                _line = _type.GetField("line", flags);
                _mode = _type.GetField("mode", flags);
                _instanceID = _type.GetField("instanceID", flags);
                _identifier = _type.GetField("identifier", flags);
                _isWorldPlaying = _type.GetField("isWorldPlaying", flags);

                instance = Activator.CreateInstance(_type);
            }
        }
    }
}