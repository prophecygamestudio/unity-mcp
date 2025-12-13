using MCPForUnity.Editor.Constants;
using MCPForUnity.Editor.Helpers;
using MCPForUnity.Editor.Services;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MCPForUnity.Editor.Windows.Components.Settings
{
    /// <summary>
    /// Controller for the Settings section of the MCP For Unity editor window.
    /// Handles version display and debug logs.
    /// </summary>
    public class McpSettingsSection
    {
        // UI Elements
        private Label versionLabel;
        private Toggle debugLogsToggle;
        private Toggle stackTraceToggle;
        private Toggle logMcpRequestsResponsesToggle;
        private Button resetSettingsButton;

        public VisualElement Root { get; private set; }

        public McpSettingsSection(VisualElement root)
        {
            Root = root;
            CacheUIElements();
            InitializeUI();
            RegisterCallbacks();
        }

        private void CacheUIElements()
        {
            versionLabel = Root.Q<Label>("version-label");
            debugLogsToggle = Root.Q<Toggle>("debug-logs-toggle");
            stackTraceToggle = Root.Q<Toggle>("stack-trace-toggle");
            logMcpRequestsResponsesToggle = Root.Q<Toggle>("log-mcp-requests-responses-toggle");
            resetSettingsButton = Root.Q<Button>("reset-settings-button");
        }

        private void InitializeUI()
        {
            if (versionLabel != null)
            {
                UpdateVersionLabel();
            }

            if (debugLogsToggle != null)
            {
                bool debugEnabled = EditorPrefs.GetBool(EditorPrefKeys.DebugLogs, false);
                debugLogsToggle.value = debugEnabled;
                McpLog.SetDebugLoggingEnabled(debugEnabled);
            }

            if (stackTraceToggle != null)
            {
                bool stackTraceEnabled = EditorPrefs.GetBool(EditorPrefKeys.LogStackTrace, true);
                stackTraceToggle.value = stackTraceEnabled;
                McpLog.SetStackTraceEnabled(stackTraceEnabled);
            }

            if (logMcpRequestsResponsesToggle != null)
            {
                bool logMcpRequestsResponsesEnabled = EditorPrefs.GetBool(EditorPrefKeys.LogMcpRequestsAndResponses, false);
                logMcpRequestsResponsesToggle.value = logMcpRequestsResponsesEnabled;
                McpLog.SetLogMcpRequestsAndResponsesEnabled(logMcpRequestsResponsesEnabled);
            }
        }

        private void RegisterCallbacks()
        {
            debugLogsToggle.RegisterValueChangedCallback(evt =>
            {
                McpLog.SetDebugLoggingEnabled(evt.newValue);
            });

            stackTraceToggle.RegisterValueChangedCallback(evt =>
            {
                McpLog.SetStackTraceEnabled(evt.newValue);
            });

            logMcpRequestsResponsesToggle.RegisterValueChangedCallback(evt =>
            {
                McpLog.SetLogMcpRequestsAndResponsesEnabled(evt.newValue);
            });

            if (resetSettingsButton != null)
            {
                resetSettingsButton.clicked += ResetSettings;
            }
        }

        private void ResetSettings()
        {
            // Default values as defined in InitializeUI
            const bool defaultDebugLogs = false;
            const bool defaultStackTrace = true;
            const bool defaultLogMcpRequestsResponses = false;

            // Update UI toggles to reflect the new values
            if (debugLogsToggle != null)
            {
                debugLogsToggle.value = defaultDebugLogs;
            }

            if (stackTraceToggle != null)
            {
                stackTraceToggle.value = defaultStackTrace;
            }

            if (logMcpRequestsResponsesToggle != null)
            {
                logMcpRequestsResponsesToggle.value = defaultLogMcpRequestsResponses;
            }

                        // Explicitly update McpLog state (this also updates EditorPrefs internally)
            McpLog.SetDebugLoggingEnabled(defaultDebugLogs);
            McpLog.SetStackTraceEnabled(defaultStackTrace);
            McpLog.SetLogMcpRequestsAndResponsesEnabled(defaultLogMcpRequestsResponses);
        }

        private void UpdateVersionLabel()
        {
            if (versionLabel == null)
                return;

            string currentVersion = AssetPathUtility.GetPackageVersion();
            versionLabel.text = $"v{currentVersion}";

            var updateCheck = MCPServiceLocator.Updates.CheckForUpdate(currentVersion);

            if (updateCheck.UpdateAvailable && !string.IsNullOrEmpty(updateCheck.LatestVersion))
            {
                versionLabel.text = $"\u2191 v{currentVersion} (Update available: v{updateCheck.LatestVersion})";
                versionLabel.style.color = new Color(1f, 0.7f, 0f);
                versionLabel.tooltip = $"Version {updateCheck.LatestVersion} is available. Update via Package Manager.\n\nGit URL: https://github.com/prophecygamestudio/unity-mcp.git?path=/MCPForUnity";
            }
            else
            {
                versionLabel.style.color = StyleKeyword.Null;
                versionLabel.tooltip = $"Current version: {currentVersion}";
            }
        }

    }
}
