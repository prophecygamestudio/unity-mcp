using MCPForUnity.Editor.Constants;
using UnityEditor;
using UnityEngine;

namespace MCPForUnity.Editor.Helpers
{
    internal static class McpLog
    {
        private const string InfoPrefix = "<b><color=#2EA3FF>MCP-FOR-UNITY</color></b>:";
        private const string DebugPrefix = "<b><color=#6AA84F>MCP-FOR-UNITY</color></b>:";
        private const string WarnPrefix = "<b><color=#cc7a00>MCP-FOR-UNITY</color></b>:";
        private const string ErrorPrefix = "<b><color=#cc3333>MCP-FOR-UNITY</color></b>:";

        private static volatile bool _debugEnabled = ReadDebugPreference();
        private static volatile bool _stackTraceEnabled = ReadStackTracePreference();

        private static bool IsDebugEnabled() => _debugEnabled;
        private static bool IsStackTraceEnabled() => _stackTraceEnabled;

        private static bool ReadDebugPreference()
        {
            try { return EditorPrefs.GetBool(EditorPrefKeys.DebugLogs, false); }
            catch { return false; }
        }

        private static bool ReadStackTracePreference()
        {
            try { return EditorPrefs.GetBool(EditorPrefKeys.LogStackTrace, false); }
            catch { return false; }
        }

        public static void SetDebugLoggingEnabled(bool enabled)
        {
            _debugEnabled = enabled;
            try { EditorPrefs.SetBool(EditorPrefKeys.DebugLogs, enabled); }
            catch { }
        }

        public static void SetStackTraceEnabled(bool enabled)
        {
            _stackTraceEnabled = enabled;
            try { EditorPrefs.SetBool(EditorPrefKeys.LogStackTrace, enabled); }
            catch { }
        }

        public static void Debug(string message)
        {
            if (!IsDebugEnabled()) return;
            LogOption logOption = IsStackTraceEnabled() ? LogOption.None : LogOption.NoStacktrace;
            UnityEngine.Debug.LogFormat(LogType.Log, logOption, null, "{0} {1}", DebugPrefix, message);
        }

        public static void Info(string message, bool always = true)
        {
            if (!always && !IsDebugEnabled()) return;
            LogOption logOption = IsStackTraceEnabled() ? LogOption.None : LogOption.NoStacktrace;
            UnityEngine.Debug.LogFormat(LogType.Log, logOption, null, "{0} {1}", InfoPrefix, message);
        }

        public static void Warn(string message)
        {
            LogOption logOption = IsStackTraceEnabled() ? LogOption.None : LogOption.NoStacktrace;
            UnityEngine.Debug.LogFormat(LogType.Warning, logOption, null, "{0} {1}", WarnPrefix, message);
        }

        public static void Error(string message)
        {
            LogOption logOption = IsStackTraceEnabled() ? LogOption.None : LogOption.NoStacktrace;
            UnityEngine.Debug.LogFormat(LogType.Error, logOption, null, "{0} {1}", ErrorPrefix, message);
        }
    }
}
