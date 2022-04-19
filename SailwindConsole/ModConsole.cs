using SailwindConsole.Commands;
using SailwindConsole.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityModManagerNet;
using Logger = UnityModManagerNet.UnityModManager.Logger;

namespace SailwindConsole
{
    public static class ModConsole
    {
        private static bool initialised;
        private static CanvasGroup canvasGroup;
        internal static InputField consoleInput;
        internal static Text logText;
        private static ScrollRect scrollRect;
        private static Canvas modCanvas;
        private static GameObject modConsoleObject;
        private static List<Command> commands = new List<Command>();

        private const string RE_ARG_MATCHER_PATTERN = @"""[^""\\]*(?:\\.[^""\\]*)*""|'[^'\\]*(?:\\.[^'\\]*)*'|\S+";
        private const string RE_QUOTE_STRIP_PATTERN = @"^""+|""+$|^'+|'+$";

        internal static List<string> previousCommands = new List<string>();
        internal static int previousCommandIndex = -1;

        #region Initialisation
        /*private static void InitCanvasScaler()
        {
            modConsoleObject = new GameObject();
            modConsoleObject.name = "ConsoleCanvas";
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

            float size = (1 / 18f) * Screen.height;

            RectTransform logBackgroundRectTransform = logBackgroundObject.GetComponent<RectTransform>();
            logBackgroundRectTransform.anchorMin = new Vector2(0f, 0f);
            logBackgroundRectTransform.anchorMax = new Vector2(1f, 1f);
            logBackgroundRectTransform.pivot = new Vector2(0.5f, 0.5f);
            logBackgroundRectTransform.localPosition = new Vector3(0, 0, 0);
            logBackgroundRectTransform.sizeDelta = new Vector2(0, size * 1.25f);

            ScrollRect scrollRect = logBackgroundObject.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Elastic;
            scrollRect.elasticity = 0.1f;
            scrollRect.inertia = true;
            scrollRect.decelerationRate = 0.135f;
            scrollRect.scrollSensitivity = 20;

            #region Viewport
            GameObject viewportGameObject = new GameObject();
            viewportGameObject.name = "Viewport";
            viewportGameObject.transform.parent = logBackgroundObject.transform;
            viewportGameObject.transform.localScale = Vector3.one;

            viewportGameObject.AddComponent<Mask>().showMaskGraphic = false;
            viewportGameObject.AddComponent<Image>();

            RectTransform viewportRectTransform = viewportGameObject.GetComponent<RectTransform>();
            viewportRectTransform.anchorMin = new Vector2(0f, 0f);
            viewportRectTransform.anchorMax = new Vector2(1f, 1f);
            viewportRectTransform.pivot = new Vector2(0, 1);
            viewportRectTransform.localPosition = new Vector3(-Screen.width/2f, Screen.height/2f, 0);
            viewportRectTransform.sizeDelta = new Vector2(-20, -size * 1.25f - 20);
            scrollRect.viewport = viewportRectTransform;
            #endregion

            #region Content
            GameObject contentGameObject = new GameObject();
            contentGameObject.name = "Content";
            contentGameObject.transform.parent = viewportGameObject.transform;
            contentGameObject.transform.localScale = Vector3.one;

            VerticalLayoutGroup verticalLayoutGroup = contentGameObject.AddComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.padding = new RectOffset(0, 0, 0, 0);
            verticalLayoutGroup.spacing = 0;
            verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
            verticalLayoutGroup.childControlWidth = true;
            verticalLayoutGroup.childControlHeight = true;
            verticalLayoutGroup.childScaleWidth = false;
            verticalLayoutGroup.childScaleHeight = false;
            verticalLayoutGroup.childForceExpandWidth = true;
            verticalLayoutGroup.childForceExpandHeight = true;

            ContentSizeFitter contentSizeFitter = contentGameObject.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            RectTransform contentRectTransform = contentGameObject.GetComponent<RectTransform>();

            contentRectTransform.anchorMin = new Vector2(0f, 0f);
            contentRectTransform.anchorMax = new Vector2(1f, 1f);
            contentRectTransform.pivot = new Vector2(0, 1);
            contentRectTransform.localPosition = Vector3.zero;
            contentRectTransform.sizeDelta = Vector2.zero;

            scrollRect.content = contentRectTransform;

            #endregion

            #region Log Text
            GameObject logTextObject = new GameObject();
            logTextObject.name = "ConsoleLogText";
            logTextObject.transform.parent = contentGameObject.transform;
            logTextObject.transform.localScale = Vector3.one;

            logText = logTextObject.AddComponent<Text>();
            logText.font = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
            logText.text = "";
            logText.color = Color.white;
            logText.supportRichText = true;
            logText.fontSize = 20;
            logText.alignment = TextAnchor.LowerLeft;
            logText.verticalOverflow = VerticalWrapMode.Overflow;

            RectTransform logTextRectTransform = logTextObject.GetComponent<RectTransform>();
            logTextRectTransform.anchorMin = new Vector2(0f, 0f);
            logTextRectTransform.anchorMax = new Vector2(1f, 1f);
            logTextRectTransform.pivot = new Vector2(0.5f, 0.5f);
            logTextRectTransform.localPosition = Vector3.zero;
            logTextRectTransform.sizeDelta = Vector2.zero;
            #endregion

            #region Scroll Bar

            #region ScrollBar
            GameObject scrollBarGameObject = new GameObject();
            scrollBarGameObject.name = "Scrollbar Vertical";
            scrollBarGameObject.transform.parent = logBackgroundObject.transform;
            scrollBarGameObject.transform.localScale = Vector3.one;

            scrollBarGameObject.AddComponent<Image>().color = new Color(0, 0, 0, 0.2666667f);
            scrollBarGameObject.GetComponent<Image>().raycastTarget = true;

            Scrollbar scrollBar = scrollBarGameObject.AddComponent<Scrollbar>();

            scrollBar.interactable = true;
            scrollBar.transition = Selectable.Transition.ColorTint;
            ColorBlock colors = new ColorBlock();
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f);
            colors.pressedColor = new Color(0.7843137f, 0.7843137f, 0.7843137f);
            colors.selectedColor = new Color(0.9607843f, 0.9607843f, 0.9607843f);
            colors.disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
            colors.colorMultiplier = 1;
            colors.fadeDuration = 0.1f;
            scrollBar.colors = colors;

            scrollBar.direction = Scrollbar.Direction.BottomToTop;
            scrollBar.value = 0;
            scrollBar.size = 1;
            scrollBar.numberOfSteps = 0;

            scrollRect.verticalScrollbar = scrollBar;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;

            RectTransform scrollBarRectTransform = scrollBarGameObject.GetComponent<RectTransform>();
            scrollBarRectTransform.anchorMin = new Vector2(1f, 0f);
            scrollBarRectTransform.anchorMax = new Vector2(1f, 1f);
            scrollBarRectTransform.pivot = new Vector2(1f, 1f);
            scrollBarRectTransform.localPosition = new Vector3((Screen.width - 20)/2f, size, 0);
            scrollBarRectTransform.sizeDelta = new Vector2(20, -((size*2) - 20) * 2f);
            #endregion

            #region Sliding Area
            GameObject slidingAreaGameObject = new GameObject();
            slidingAreaGameObject.name = "Sliding Area";
            slidingAreaGameObject.transform.parent = scrollBarGameObject.transform;
            slidingAreaGameObject.transform.localScale = Vector3.one;

            RectTransform slidingAreaRectTransform = slidingAreaGameObject.AddComponent<RectTransform>();
            slidingAreaRectTransform.anchorMin = new Vector2(0f, 0f);
            slidingAreaRectTransform.anchorMax = new Vector2(1f, 1f);
            slidingAreaRectTransform.pivot = new Vector2(0.5f, 0.5f);
            slidingAreaRectTransform.localPosition = new Vector3(10, 10, 0);
            slidingAreaRectTransform.sizeDelta = new Vector2(10, 10);
            #endregion

            #region Handle
            GameObject handleGameObject = new GameObject();
            handleGameObject.name = "Handle";
            handleGameObject.transform.parent = slidingAreaGameObject.transform;
            handleGameObject.transform.localScale = Vector3.one;

            handleGameObject.AddComponent<Image>().color = new Color(1, 1, 1, 1);
            handleGameObject.GetComponent<Image>().raycastTarget = true;

            RectTransform handleRectTransform = handleGameObject.GetComponent<RectTransform>();

            handleRectTransform.anchorMin = new Vector2(0f, 0f);
            handleRectTransform.anchorMax = new Vector2(1f, 1f);
            handleRectTransform.pivot = new Vector2(0.5f, 0.5f);
            handleRectTransform.localPosition = new Vector3(-10, -10, 0);
            handleRectTransform.sizeDelta = new Vector2(-10, -10);

            scrollBar.handleRect = handleRectTransform;
            scrollBar.targetGraphic = handleGameObject.GetComponent<Image>();
            #endregion

            #endregion
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
        }*/

        internal static void InitialiseConsole()
        {
            if (initialised)
                return;
            Logger.Log("Setting up console");

            var asset = AssetBundle.LoadFromFile(Path.Combine(Main.mod.Path, "assets", "console"));
            if (asset == null)
            {
                UnityModManager.Logger.Error($"Failed to load asset bundle");
                return;
            }

            var prefab = asset.LoadAsset<GameObject>("Console");
            modConsoleObject = GameObject.Instantiate(prefab);
            modCanvas = modConsoleObject.GetComponent<Canvas>();

            logText = modConsoleObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();

            logText.text = "";

            consoleInput = modConsoleObject.transform.GetChild(1).GetComponent<InputField>();

            consoleInput.Select();
            consoleInput.ActivateInputField();

            consoleInput.onEndEdit.AddListener((_) => OnEndEdit());

            modConsoleObject.transform.GetChild(1).gameObject.AddComponent<CommandHistoryChanger>();

            scrollRect = modConsoleObject.transform.GetChild(0).GetComponent<ScrollRect>();

            scrollRect.scrollSensitivity = 10;

            //InitCanvasScaler();
            //InitLog();
            //InitInputField();

            canvasGroup = modConsoleObject.GetComponent<CanvasGroup>();

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
            AddCommand(new ShowPortsCommand());
            AddCommand(new ShowRegionsCommand());
            AddCommand(new TeleportCommand());
            AddCommand(new SeaLevelCommand());
            AddCommand(new RespawnShopsCommand());
            AddCommand(new GodModeCommand());
            AddCommand(new GameSpeedCommand());
            AddCommand(new SetStormCommand());
            AddCommand(new SetWindSpeedCommand());
            AddCommand(new CookFoodCommand());
            AddCommand(new SetWaveHeightCommand());
            //AddCommand(new ListSpawnableObjectsCommand());
            AddCommand(new SpawnObjectCommand());
            AddCommand(new HelpCommand(commands));
        }

        public static void MoveScrollToEnd()
        {
            scrollRect.verticalNormalizedPosition = 0;
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
                    ModConsoleLog.Error($"\"{command.Name}\" requires {command.MinArgs} arguments!");
                    if (!string.IsNullOrEmpty(command.Usage))
                    {
                        ModConsoleLog.Error($"Usage: {command.Usage}");
                    }
                }
                else
                {
                    command.OnRun(args);
                }
            }
            else
            {
                ModConsoleLog.Error($"\"{commandName}\" is not a valid command!");
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
            consoleInput.DeactivateInputField();
            canvasGroup.alpha = 0;
        }

        internal static void ShowConsole()
        {
            consoleInput.DeactivateInputField();
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
                    inputField.caretPosition = inputField.text.Length;
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
                inputField.caretPosition = inputField.text.Length;
            }
        }
    }
}
