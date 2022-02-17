using SailwindConsole.Commands;
using SailwindConsole.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Logger = UnityModManagerNet.UnityModManager.Logger;

namespace SailwindConsole
{
    public static class ModConsole
    {
        private static bool initialised;
        private static CanvasGroup canvasGroup;
        internal static InputField consoleInput;
        private static Text logText;
        private static Canvas modCanvas;
        private static GameObject modConsoleObject;
        private static List<Command> commands = new List<Command>();

        private const string RE_ARG_MATCHER_PATTERN = @"""[^""\\]*(?:\\.[^""\\]*)*""|'[^'\\]*(?:\\.[^'\\]*)*'|\S+";
        private const string RE_QUOTE_STRIP_PATTERN = @"^""+|""+$|^'+|'+$";

        private static string[] logColors = new string[] { "fuchsia", "cyan", "orange", "red" };

        internal static List<string> previousCommands = new List<string>();
        internal static int previousCommandIndex = -1;

        #region Initialisation
        private static void InitCanvasScaler()
        {
            modConsoleObject = new GameObject();
            modConsoleObject.name = "Console";
            modCanvas = modConsoleObject.AddComponent<Canvas>();
            modCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            modConsoleObject.AddComponent<GraphicRaycaster>();

            CanvasScaler canvasScaler = modConsoleObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.scaleFactor = 1;
        }

        private static void InitLog()
        {
            GameObject logBackgroundObject = new GameObject();
            logBackgroundObject.name = "ConsoleLogBackground";
            logBackgroundObject.transform.parent = modConsoleObject.transform;
            logBackgroundObject.transform.localScale = Vector3.one;
            logBackgroundObject.AddComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            logBackgroundObject.GetComponent<Image>().raycastTarget = true;

            RectTransform logBackgroundRectTransform = logBackgroundObject.GetComponent<RectTransform>();
            logBackgroundRectTransform.anchorMin = new Vector2(0f, 0f);
            logBackgroundRectTransform.anchorMax = new Vector2(1f, 1f);
            logBackgroundRectTransform.pivot = new Vector2(0.5f, 0.5f);
            logBackgroundRectTransform.localPosition = Vector3.zero;
            logBackgroundRectTransform.sizeDelta = Vector2.zero;

            GameObject logTextObject = new GameObject();
            logTextObject.name = "ConsoleLogText";
            logTextObject.transform.parent = logBackgroundObject.transform;
            logTextObject.transform.localScale = Vector3.one;

            logText = logTextObject.AddComponent<Text>();
            logText.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
            logText.text = "";
            logText.color = Color.white;
            logText.supportRichText = true;
            logText.fontSize = 20;
            logText.alignment = TextAnchor.LowerLeft;
            logText.verticalOverflow = VerticalWrapMode.Overflow;

            float size = (1 / 18f) * Screen.height;

            RectTransform logTextRectTransform = logTextObject.GetComponent<RectTransform>();
            logTextRectTransform.anchorMin = new Vector2(0f, 0f);
            logTextRectTransform.anchorMax = new Vector2(1f, 1f);
            logTextRectTransform.pivot = new Vector2(0.5f, 0.5f);
            logTextRectTransform.localPosition = new Vector3(0, size*1.25f, 0);
            logTextRectTransform.sizeDelta = Vector2.zero;
        }

        private static void InitInputField()
        {
            float size = (1 / 18f) * Screen.height;

            GameObject consoleInputObject = new GameObject();
            consoleInputObject.name = "ConsoleInputField";
            consoleInputObject.transform.parent = modConsoleObject.transform;
            consoleInputObject.transform.localScale = Vector3.one;
            consoleInputObject.AddComponent<Image>().color = Color.gray;
            consoleInput = consoleInputObject.AddComponent<InputField>();

            RectTransform rectTransform = consoleInputObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 0f);
            rectTransform.anchorMax = new Vector2(1f, 0f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.localPosition = new Vector3(0, -(Screen.height-size)/2f, 0);
            rectTransform.sizeDelta = new Vector2(0, size);

            GameObject consoleTextObject = new GameObject();
            consoleTextObject.name = "ConsoleText";
            consoleTextObject.transform.parent = consoleInputObject.transform;
            consoleTextObject.transform.localScale = Vector3.one;

            RectTransform component3 = consoleTextObject.AddComponent<RectTransform>();
            component3.anchorMin = new Vector2(0f, 0f);
            component3.anchorMax = new Vector2(1f, 0f);
            component3.pivot = new Vector2(0.5f, 0.5f);
            component3.localPosition = Vector3.zero;
            component3.sizeDelta = new Vector2(0, size);

            Text component4 = consoleTextObject.AddComponent<Text>();
            component4.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
            component4.text = "";
            component4.color = Color.white;
            component4.supportRichText = false;
            component4.fontSize = (int)(size/2f);
            component4.alignment = TextAnchor.MiddleLeft;
            component4.verticalOverflow = VerticalWrapMode.Overflow;

            consoleInput.textComponent = component4;

            consoleInput.Select();
            consoleInput.ActivateInputField();
            consoleInput.targetGraphic = consoleInput.transform.GetComponent<Image>();

            consoleInput.onEndEdit.AddListener((_) => OnEndEdit());

            consoleInputObject.AddComponent<CommandHistoryChanger>();
        }

        internal static void InitialiseConsole()
        {
            if (initialised)
                return;
            Logger.Log("Setting up console");

            InitCanvasScaler();
            InitLog();
            InitInputField();

            modConsoleObject.AddComponent<EventSystem>();
            modConsoleObject.AddComponent<StandaloneInputModule>();
            canvasGroup = modConsoleObject.AddComponent<CanvasGroup>();

            initialised = true;
            Logger.Log("Console is created");

            HideConsole();

            AddDefaultCommands();
        }
        #endregion

        private static void AddDefaultCommands()
        {
            AddCommand(new SetGoldCommand());
            AddCommand(new AddGoldCommand());
            AddCommand(new SetThirstCommand());
            AddCommand(new SetHungerCommand());
            AddCommand(new SetSleepCommand());
            AddCommand(new SetAlcoholCommand());
            AddCommand(new SetTimeCommand());
            AddCommand(new GetWeightCommand());
            AddCommand(new CurrentTimeCommand());
            AddCommand(new WorldCoordsCommand());
            AddCommand(new LatLongCommand());
            AddCommand(new AddReputation());
            AddCommand(new HelpCommand(commands));
        }

        internal static void OnEndEdit()
        {
            Regex reg = new Regex(RE_ARG_MATCHER_PATTERN);
            string text = consoleInput.text;
            if (string.IsNullOrEmpty(text)) return;
            MatchCollection matches = reg.Matches(text);
            List<string> commandData = new List<string>();
            foreach (Match match in matches)
            {
                commandData.Add(Regex.Replace(match.Value, RE_QUOTE_STRIP_PATTERN, ""));
            }
            if (commandData.Count <= 0) return;

            string commandName = commandData[0].ToLower();
            List<string> args = commandData.GetRange(1, commandData.Count - 1);

            Command command = commands.Find(c => c.Name.ToLower().Equals(commandName) || c.Aliases.Map(x => x.ToLower()).Contains(commandName));

            if (command != null)
            {
                if (args.Count < command.MinArgs)
                {
                    Error($"\"{command.Name}\" requires {command.MinArgs} arguments!");
                }
                else
                {
                    command.OnRun(args);
                }
            }
            else
            {
                Error($"\"{commandName}\" is not a valid command!");
            }
            previousCommands.Add(text);
            previousCommandIndex = previousCommands.Count;
            consoleInput.text = "";
            consoleInput.Select();
            consoleInput.ActivateInputField();
        }

        public static void AddCommand(Command command)
        {
            commands.Add(command);
        }

        internal static void HideConsole()
        {
            canvasGroup.alpha = 0;
        }

        internal static void ShowConsole()
        {
            canvasGroup.alpha = 1;
        }

        internal static void ToggleConsole()
        {
            if(canvasGroup.alpha <= 0)
            {
                ShowConsole();
            }
            else
            {
                HideConsole();
            }
        }

        private static void Log(LogLevel logLevel, params object[] message)
        {
            string str = "";
            foreach (object item in message)
            {
                str += item.ToString();
            }
            logText.text += $"\n[<color={logColors[(int)logLevel]}>{logLevel}</color>] {str}";
        }

        public static void Log(params object[] message)
        {
            Log(LogLevel.Info, message);
        }

        public static void Error(params object[] message)
        {
            Log(LogLevel.Error, message);
        }

        public static void Warn(params object[] message)
        {
            Log(LogLevel.Warn, message);
        }

        public static void Debug(params object[] message)
        {
            Log(LogLevel.Debug, message);
        }

        private enum LogLevel { Debug, Info, Warn, Error }
    }

    internal class CommandHistoryChanger : MonoBehaviour
    {
        private InputField inputField;

        private void Start()
        {
            inputField = GetComponent<InputField>();
        }

        private void Update()
        {
            if (!inputField.isFocused) return;
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ModConsole.previousCommandIndex++;
                if (ModConsole.previousCommandIndex >= ModConsole.previousCommands.Count)
                {
                    inputField.text = "";
                    ModConsole.previousCommandIndex = ModConsole.previousCommands.Count;
                }
                else
                {
                    inputField.text = ModConsole.previousCommands[ModConsole.previousCommandIndex];
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ModConsole.previousCommandIndex--;
                if (ModConsole.previousCommandIndex < 0)
                {
                    ModConsole.previousCommandIndex = 0;
                }
                inputField.text = ModConsole.previousCommands[ModConsole.previousCommandIndex];
            }
        }
    }
}
