using FastConsoleUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using TextAdventurer.Data;

/// <summary>
/// Text Adventurer namespace
/// </summary>
namespace TextAdventurer
{
    /// <summary>
    /// Story class
    /// </summary>
    internal class Story
    {
        /// <summary>
        /// Configuration path
        /// </summary>
        private static readonly string configPath = "./config.json";

        /// <summary>
        /// Configuration data serializer
        /// </summary>
        private static readonly DataContractJsonSerializer configDataSerializer = new DataContractJsonSerializer(typeof(ConfigDataContract));

        /// <summary>
        /// Story data serializer
        /// </summary>
        private static readonly DataContractJsonSerializer storyDataSerializer = new DataContractJsonSerializer(typeof(StoryDataContract), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });

        /// <summary>
        /// Configuration data serializer
        /// </summary>
        private static readonly DataContractJsonSerializer saveStatesSerializer = new DataContractJsonSerializer(typeof(List<SaveStateDataContract>));

        /// <summary>
        /// Yes and no dictionary
        /// </summary>
        private static readonly IReadOnlyDictionary<string, bool> yesNoDictionary = new Dictionary<string, bool>()
        {
            { "y", true },
            { "yes", true },
            { "ye", true },
            { "sure", true },
            { "always", true },
            { "n", false },
            { "no", false },
            { "nah", false },
            { "never", false }
        };

        /// <summary>
        /// Configuration data
        /// </summary>
        private ConfigDataContract configData;

        /// <summary>
        /// Story data
        /// </summary>
        private StoryDataContract storyData;

        /// <summary>
        /// Save states
        /// </summary>
        private List<SaveStateDataContract> saveStates;

        /// <summary>
        /// Save states path
        /// </summary>
        private string saveStatesPath;

        /// <summary>
        /// Name
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// Lastname
        /// </summary>
        private string lastname = string.Empty;

        /// <summary>
        /// State
        /// </summary>
        private string state = string.Empty;

        /// <summary>
        /// States
        /// </summary>
        private List<Tuple<string, StateDataContract>> states = new List<Tuple<string, StateDataContract>>();

        /// <summary>
        /// Current options
        /// </summary>
        private Dictionary<string, string> currentOptions = new Dictionary<string, string>();

        /// <summary>
        /// Window
        /// </summary>
        private Window window;

        /// <summary>
        /// Header panel
        /// </summary>
        private Panel headerPanel;

        /// <summary>
        /// Main menu panel
        /// </summary>
        private Panel mainMenuPanel;

        /// <summary>
        /// Description text animator
        /// </summary>
        private TextAnimator descriptionTextAnimator;

        /// <summary>
        /// Title label
        /// </summary>
        private ColoredLabel titleLabel;

        /// <summary>
        /// Author name label
        /// </summary>
        private ColoredLabel authorNameLabel;

        /// <summary>
        /// License label
        /// </summary>
        private ColoredLabel licenseLabel;

        /// <summary>
        /// Description text field
        /// </summary>
        private ColoredTextField descriptionTextField;

        /// <summary>
        /// Game panel
        /// </summary>
        private Panel gamePanel;

        /// <summary>
        /// Game text field
        /// </summary>
        private ColoredTextField gameTextField;

        /// <summary>
        /// Game text animator
        /// </summary>
        private TextAnimator gameTextAnimator;

        /// <summary>
        /// Command line panel
        /// </summary>
        private Panel commandLinePanel;

        /// <summary>
        /// Command line input field
        /// </summary>
        private InputField commandLineInputField;

        /// <summary>
        /// Main menu
        /// </summary>
        private Menu mainMenu;

        /// <summary>
        /// Load save state menu
        /// </summary>
        private Menu loadSaveStateMenu;

        /// <summary>
        /// Save save state menu
        /// </summary>
        private Menu saveSaveStateMenu;

        /// <summary>
        /// Pause menu
        /// </summary>
        private Menu pauseMenu;

        /// <summary>
        /// Dialog
        /// </summary>
        private Dialog dialog;

        /// <summary>
        /// Menu state
        /// </summary>
        private EMenuState menuState = EMenuState.Nothing;

        /// <summary>
        /// Configuration data
        /// </summary>
        private ConfigDataContract ConfigData
        {
            get
            {
                if (configData == null)
                {
                    try
                    {
                        if (File.Exists(configPath))
                        {
                            using (FileStream file_stream = File.OpenRead(configPath))
                            {
                                configData = configDataSerializer.ReadObject(file_stream) as ConfigDataContract;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(e);
                    }
                    if (configData == null)
                    {
                        configData = new ConfigDataContract();
                    }
                }
                return configData;
            }
        }

        /// <summary>
        /// Save states
        /// </summary>
        private IReadOnlyList<SaveStateDataContract> SaveStates
        {
            get
            {
                if (saveStates == null)
                {
                    try
                    {
                        using (FileStream file_stream = File.OpenRead(ConfigData.SaveStatesPath))
                        {
                            saveStates = saveStatesSerializer.ReadObject(file_stream) as List<SaveStateDataContract>;
                            if (saveStates != null)
                            {
                                for (int i = saveStates.Count - 1; i >= 0; i--)
                                {
                                    if (saveStates[i] == null)
                                    {
                                        saveStates.RemoveAt(i);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(e);
                    }
                    if (saveStates == null)
                    {
                        saveStates = new List<SaveStateDataContract>();
                    }
                }
                return saveStates;
            }
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (value != null)
                {
                    name = value;
                }
            }
        }

        /// <summary>
        /// State
        /// </summary>
        public string State
        {
            get => state;
            set
            {
                if (value != null)
                {
                    string[] parent_states = value.Split('/');
                    StringBuilder real_parent_state_builder = new StringBuilder();
                    StringBuilder state_builder = new StringBuilder();
                    if (value.StartsWith("/"))
                    {
                        states.Clear();
                    }
                    foreach (string parent_state in parent_states)
                    {
                        if (parent_state.EndsWith('\\'))
                        {
                            if (parent_state.Length > 1)
                            {
                                real_parent_state_builder.Append(parent_state.Substring(0, parent_state.Length - 1));
                            }
                        }
                        else
                        {
                            real_parent_state_builder.Append(parent_state);
                            string real_parent_state = real_parent_state_builder.ToString();
                            real_parent_state_builder.Clear();
                            if (real_parent_state.Length > 0)
                            {
                                if (real_parent_state == ".")
                                {
                                    // ...
                                }
                                else if (real_parent_state == "..")
                                {
                                    if (states.Count > 0)
                                    {
                                        states.RemoveAt(states.Count - 1);
                                    }
                                }
                                else if (SelectedState.States.ContainsKey(real_parent_state))
                                {
                                    states.Add(new Tuple<string, StateDataContract>(real_parent_state, SelectedState.States[real_parent_state]));
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    foreach (Tuple<string, StateDataContract> state in states)
                    {
                        state_builder.Append("/");
                        state_builder.Append(state.Item1);
                    }
                    state = state_builder.ToString();
                }
            }
        }

        /// <summary>
        /// Selected state
        /// </summary>
        private StateDataContract SelectedState => ((states.Count > 0) ? states[states.Count - 1].Item2 : storyData.InitialState);

        /// <summary>
        /// Menu state
        /// </summary>
        public EMenuState MenuState
        {
            get => menuState;
            set
            {
                if (menuState != value)
                {
                    switch (menuState)
                    {
                        case EMenuState.SplashScreen:
                            descriptionTextField.IsVisible = false;
                            break;
                        case EMenuState.MainMenu:
                            mainMenuPanel.IsVisible = false;
                            break;
                        case EMenuState.LoadSaveStates:
                            loadSaveStateMenu.IsVisible = false;
                            break;
                        case EMenuState.Game:
                            commandLinePanel.IsVisible = false;
                            break;
                        case EMenuState.Pause:
                            gamePanel.IsVisible = false;
                            pauseMenu.IsVisible = false;
                            break;
                        case EMenuState.EndGame:
                            gamePanel.IsVisible = false;
                            commandLinePanel.IsVisible = false;
                            break;
                    }
                    switch (value)
                    {
                        case EMenuState.SplashScreen:
                            mainMenuPanel.IsVisible = true;
                            descriptionTextAnimator?.Reset();
                            break;
                        case EMenuState.MainMenu:
                            mainMenuPanel.IsVisible = true;
                            mainMenu.IsVisible = true;
                            break;
                        case EMenuState.LoadSaveStates:
                            string[] load_save_states = new string[SaveStates.Count];
                            for (int i = 0; i < load_save_states.Length; i++)
                            {
                                load_save_states[i] = SaveStates[i].ToString();
                            }
                            loadSaveStateMenu.Items = load_save_states;
                            loadSaveStateMenu.IsVisible = true;
                            break;
                        case EMenuState.NewGame:
                            Name = string.Empty;
                            State = "/";
                            UpdateNewGameInterface();
                            gamePanel.IsVisible = true;
                            commandLinePanel.IsVisible = true;
                            break;
                        case EMenuState.Game:
                            gamePanel.IsVisible = true;
                            commandLinePanel.IsVisible = true;
                            UpdateGameInterface();
                            break;
                        case EMenuState.Pause:
                            pauseMenu.IsVisible = true;
                            break;
                        case EMenuState.SaveSaveState:
                            string[] save_save_states = new string[SaveStates.Count + 1];
                            save_save_states[0] = "Create new";
                            for (int i = 1; i <= save_save_states.Length; i++)
                            {
                                save_save_states[i] = SaveStates[i].ToString();
                            }
                            saveSaveStateMenu.Items = save_save_states;
                            saveSaveStateMenu.IsVisible = true;
                            break;
                        case EMenuState.EndGame:
                            commandLinePanel.IsVisible = true;
                            break;
                    }
                    menuState = value;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storyData">Story data</param>
        /// <param name="saveStates">Save states</param>
        /// <param name="saveStatesPath">Save states path</param>
        private Story(StoryDataContract storyData, List<SaveStateDataContract> saveStates, string saveStatesPath)
        {
            this.storyData = storyData;
            this.saveStates = saveStates;
            this.saveStatesPath = saveStatesPath;
            window = new Window();
            headerPanel = window.AddControl<Panel>(1, 1, 0, 7);
            titleLabel = headerPanel.AddControl<ColoredLabel>(1, 0, 0, 1);
            titleLabel.Text = storyData.Title;
            titleLabel.TextAlignment = ETextAlignment.TopCenter;
            authorNameLabel = headerPanel.AddControl<ColoredLabel>(1, 2, 0, 1);
            authorNameLabel.Text = "Author: " + storyData.Author;
            authorNameLabel.TextAlignment = ETextAlignment.TopRight;
            licenseLabel = headerPanel.AddControl<ColoredLabel>(1, 4, 0, 1);
            licenseLabel.Text = "License: " + storyData.License;
            mainMenuPanel = window.AddControl<Panel>(1, 7, 0, 0);
            mainMenuPanel.IsTopOpen = true;
            descriptionTextField = mainMenuPanel.AddControl<ColoredTextField>(1, 0, 0, 1);
            descriptionTextField.Text = storyData.Description;
            gamePanel = window.AddControl<Panel>(1, 7, 0, 0);
            gamePanel.IsVisible = false;
            gamePanel.IsTopOpen = true;
            gameTextField = gamePanel.AddControl<ColoredTextField>(1, 0, 0, 1);
            commandLinePanel = window.AddControl<Panel>(1, 0, 0, 3);
            commandLinePanel.IsVisible = false;
            commandLinePanel.IsTopOpen = true;
            commandLineInputField = commandLinePanel.AddControl<InputField>(1, 0, 0, 1);
            commandLineInputField.Hint = "...";
            mainMenu = mainMenuPanel.AddControl<Menu>(RectInt.zero);
            mainMenu.IsVisible = false;
            mainMenu.Items = new string[] { "New game", "Load game (not implemented)", "Exit" };
            mainMenu.OnItemSelected += MainMenuItemSelectedEvent;
            pauseMenu = gamePanel.AddControl<Menu>(RectInt.zero);
            pauseMenu.IsVisible = false;
            pauseMenu.Items = new string[] { "Resume game", "Save game (not implemented)", "Exit" };
            pauseMenu.OnItemSelected += PauseMenuItemSelectedEvent;
            loadSaveStateMenu = mainMenuPanel.AddControl<Menu>(1, 3, 0, 0);
            loadSaveStateMenu.IsVisible = false;
            loadSaveStateMenu.OnItemSelected += LoadSaveStateMenuItemSelectedEvent;
            saveSaveStateMenu = gamePanel.AddControl<Menu>(1, 3, 0, 0);
            saveSaveStateMenu.IsVisible = false;
            saveSaveStateMenu.OnItemSelected += SaveSaveStateMenuItemSelectedEvent;
            window.OnKeyPressed += KeyPressedEvent;
            window.OnWindowResized += WindowResizedEvent;
            descriptionTextAnimator = new TextAnimator(storyData.Description + "\n\n<Default>Press <Green>Enter <Default>to continue!", true, ConfigData.LetterDelay, ConfigData.NumberDelay, ConfigData.SymbolDelay);
            gameTextAnimator = new TextAnimator(string.Empty, true, ConfigData.LetterDelay, ConfigData.NumberDelay, ConfigData.SymbolDelay);
        }

        /// <summary>
        /// Mein menu item selected event
        /// </summary>
        /// <param name="itemIndex">Item index</param>
        private void MainMenuItemSelectedEvent(uint itemIndex)
        {
            switch (itemIndex)
            {
                case 0U:
                    MenuState = EMenuState.NewGame;
                    break;
                //case 1U:
                //    MenuState = EMenuState.LoadSaveStates;
                //    break;
                case 2U:
                    MenuState = EMenuState.Nothing;
                    break;
            }
        }

        /// <summary>
        /// Pause menu item selected event
        /// </summary>
        /// <param name="itemIndex">Item index</param>
        private void PauseMenuItemSelectedEvent(uint itemIndex)
        {
            switch (itemIndex)
            {
                case 0U:
                    MenuState = EMenuState.Game;
                    break;
                //case 1U:
                //    MenuState = EMenuState.SaveSaveState;
                //    break;
                case 2U:
                    MenuState = EMenuState.Nothing;
                    break;
            }
        }

        /// <summary>
        /// Load save states menu item selected event
        /// </summary>
        /// <param name="itemIndex">Item index</param>
        private void LoadSaveStateMenuItemSelectedEvent(uint itemIndex)
        {
            if (itemIndex < SaveStates.Count)
            {
                LoadSaveState(SaveStates[(int)itemIndex]);
            }
        }

        /// <summary>
        /// Save save states menu item selected event
        /// </summary>
        /// <param name="itemIndex">Item index</param>
        private void SaveSaveStateMenuItemSelectedEvent(uint itemIndex)
        {
            SaveStateDataContract save_state = new SaveStateDataContract(Name, State, DateTime.Now);
            if (itemIndex == 0U)
            {
                saveStates.Insert(0, save_state);
            }
            else if (itemIndex <= SaveStates.Count)
            {
                saveStates[(int)itemIndex - 1] = save_state;
            }
            SaveSaveStates();
        }

        /// <summary>
        /// Load story
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>Story if successful, otherwise "null"</returns>
        public static Story LoadStory(string path)
        {
            Story ret = null;
            if (path != null)
            {
                StoryDataContract story_data = null;
                try
                {
                    if (File.Exists(path))
                    {
                        using (FileStream file_stream = File.OpenRead(path))
                        {
                            story_data = storyDataSerializer.ReadObject(file_stream) as StoryDataContract;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
                if (story_data != null)
                {
                    List<SaveStateDataContract> save_states = null;
                    string save_states_path = "./Saves/" + Path.GetFileNameWithoutExtension(path) + ".saves.json";
                    try
                    {
                        if (File.Exists(save_states_path))
                        {
                            using (FileStream file_stream = File.OpenRead(save_states_path))
                            {
                                save_states = saveStatesSerializer.ReadObject(file_stream) as List<SaveStateDataContract>;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(e);
                    }
                    if (save_states == null)
                    {
                        save_states = new List<SaveStateDataContract>();
                    }
                    ret = new Story(story_data, save_states, save_states_path);
                }
            }
            return ret;
        }

        /// <summary>
        /// Update new game menu
        /// </summary>
        private void UpdateNewGameInterface()
        {
            commandLineInputField.Text = string.Empty;
            commandLineInputField.ClearAutoCompletion();
            if (Name.Length > 0)
            {
                gameTextAnimator.Text = "Do you want to name your character <Green>\"" + Name + "\"<Default>?\n\nType <Green>\"Yes\" <Default>or <Red>\"No\"";
                commandLineInputField.AddAutoCompletion("No");
                commandLineInputField.AddAutoCompletion("Yes");
            }
            else
            {
                gameTextAnimator.Text = "Please give your <Green>character <Default>a name.";
            }
        }

        /// <summary>
        /// Update game menu
        /// </summary>
        private void UpdateGameInterface()
        {
            currentOptions.Clear();
            commandLineInputField.Text = string.Empty;
            commandLineInputField.ClearAutoCompletion();
            if (SelectedState != null)
            {
                StringBuilder game_text_builder = new StringBuilder();
                game_text_builder.Append(SelectedState.Text);
                if (SelectedState.Options.Count > 0)
                {
                    game_text_builder.Append("\n\n[Default]<Default>What do you want to do?\n");
                    foreach (KeyValuePair<string, string> option in SelectedState.Options)
                    {
                        string key = option.Key.Trim().ToLower();
                        if (key.Length > 0)
                        {
                            if (option.Key.StartsWith("?"))
                            {
                                key = key.Substring(1).Trim();
                                if (key.Length > 0)
                                {
                                    if (currentOptions.ContainsKey(key))
                                    {
                                        currentOptions[key] = option.Value;
                                    }
                                    else
                                    {
                                        currentOptions.Add(key, option.Value);
                                    }
                                }
                            }
                            else
                            {
                                game_text_builder.Append("\n<Default>- <Green>");
                                game_text_builder.Append(option.Key);
                                if (currentOptions.ContainsKey(key))
                                {
                                    currentOptions[key] = option.Value;
                                }
                                else
                                {
                                    currentOptions.Add(key, option.Value);
                                }
                                commandLineInputField.AddAutoCompletion(option.Key);
                            }
                        }
                    }
                }
                if (SelectedState.Options.Count <= 0)
                {
                    MenuState = EMenuState.EndGame;
                    game_text_builder.Append("\n\n[Default]");
                    game_text_builder.Append(SelectedState.Success ? "<Green>You won the game!" : "<Red>You lost the game");
                    game_text_builder.Append("\n\nGAME OVER!");
                }
                gameTextAnimator.Text = string.Format(game_text_builder.ToString(), Name);
                gameTextAnimator.Reset();
            }
        }

        /// <summary>
        /// Submit
        /// </summary>
        private void Submit()
        {
            string input;
            switch (menuState)
            {
                case EMenuState.NewGame:
                    input = commandLineInputField.Text.Trim();
                    if (Name.Length > 0)
                    {
                        input = input.ToLower();
                        if (yesNoDictionary.ContainsKey(input))
                        {
                            if (yesNoDictionary[input])
                            {
                                MenuState = EMenuState.Game;
                            }
                            else
                            {
                                Name = string.Empty;
                                UpdateNewGameInterface();
                            }
                        }
                    }
                    else if (input.Length > 0)
                    {
                        Name = input;
                        UpdateNewGameInterface();
                    }
                    break;
                case EMenuState.Game:
                    input = commandLineInputField.Text.Trim().ToLower();
                    if (currentOptions.ContainsKey(input))
                    {
                        State = currentOptions[input];
                        UpdateGameInterface();
                    }
                    break;
                case EMenuState.SplashScreen:
                case EMenuState.EndGame:
                    MenuState = EMenuState.MainMenu;
                    break;
            }
        }

        /// <summary>
        /// Cancel
        /// </summary>
        private void Cancel()
        {
            switch (menuState)
            {
                case EMenuState.SplashScreen:
                case EMenuState.NewGame:
                case EMenuState.LoadSaveStates:
                case EMenuState.EndGame:
                    MenuState = EMenuState.MainMenu;
                    break;
                case EMenuState.Game:
                case EMenuState.SaveSaveState:
                    MenuState = EMenuState.Pause;
                    break;
                case EMenuState.Pause:
                    MenuState = EMenuState.Game;
                    break;
            }
        }

        /// <summary>
        /// Key pressed event
        /// </summary>
        /// <param name="keyInfo">Key info</param>
        private void KeyPressedEvent(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Submit();
            }
            else if (keyInfo.Key == ConsoleKey.Escape)
            {
                Cancel();
            }
        }

        /// <summary>
        /// Window resized event
        /// </summary>
        /// <param name="size">Size</param>
        private void WindowResizedEvent(Vector2Int size)
        {
            headerPanel.Width = size.X - 2;
            titleLabel.Width = headerPanel.Size.X - 4;
            authorNameLabel.Width = headerPanel.Size.X - 4;
            licenseLabel.Width = headerPanel.Size.X - 4;
            mainMenuPanel.Size = new Vector2Int(size.X - 2, size.Y - 8);
            descriptionTextField.Size = new Vector2Int(mainMenuPanel.Size.X - 4, mainMenuPanel.Size.Y - 4);
            gamePanel.Size = new Vector2Int(size.X - 2, size.Y - 10);
            gameTextField.Size = new Vector2Int(gamePanel.Size.X - 4, gamePanel.Size.Y - 4);
            mainMenu.Size = gamePanel.Size - Vector2Int.two;
            loadSaveStateMenu.Size = new Vector2Int(mainMenuPanel.Size.X - 4, mainMenuPanel.Size.Y - 4);
            saveSaveStateMenu.Size = new Vector2Int(gamePanel.Size.X - 4, gamePanel.Size.Y - 4);
            commandLinePanel.Y = size.Y - 4;
            commandLinePanel.Width = size.X - 2;
            commandLineInputField.Width = commandLinePanel.Size.X - 4;
            pauseMenu.Size = gamePanel.Size - Vector2Int.two;
            if (dialog != null)
            {
                dialog.Position = (window.Size - dialog.Size) / 2;
            }
        }

        /// <summary>
        /// Show dialog
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="items">Items</param>
        private void ShowDialog(string title, string[] items, ItemSelectedDelegate onItemSelected, CanceledDelegate onCanceled)
        {
            Vector2Int size = new Vector2Int(title.Length + 4, items.Length + 5);
            Vector2Int position = (window.Size - size) / 2;
            HideDialog();
            dialog = window.AddControl<Dialog>(position, size);
            dialog.Title = title;
            dialog.Items = items;
            if (onItemSelected != null)
            {
                dialog.OnItemSelected += onItemSelected;
            }
            dialog.OnItemSelected += (item) =>
            {
                HideDialog();
            };
            if (onCanceled != null)
            {
                dialog.OnCanceled += onCanceled;
            }
            dialog.OnCanceled += HideDialog;
        }

        /// <summary>
        /// Hide dialog
        /// </summary>
        private void HideDialog()
        {
            if (dialog != null)
            {
                window.RemoveControl(dialog);
                dialog = null;
            }
        }

        /// <summary>
        /// Start new game
        /// </summary>
        private void StartNewGame()
        {
            MenuState = EMenuState.NewGame;
        }

        /// <summary>
        /// Load save state
        /// </summary>
        /// <param name="saveState">Save state</param>
        private void LoadSaveState(SaveStateDataContract saveState)
        {
            if (saveState != null)
            {
                Name = saveState.Name;
                State = saveState.State;
                MenuState = EMenuState.Game;
            }
        }

        /// <summary>
        /// Save save states
        /// </summary>
        private void SaveSaveStates()
        {
            try
            {
                using (FileStream file_stream = File.OpenWrite(saveStatesPath))
                {
                    saveStatesSerializer.WriteObject(file_stream, saveStates);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

        /// <summary>
        /// Run story
        /// </summary>
        public void Run()
        {
            MenuState = EMenuState.SplashScreen;
            while (MenuState != EMenuState.Nothing)
            {
                try
                {
                    Console.SetWindowPosition(0, 0);
                    Console.CursorVisible = false;
                }
                catch
                {
                    // ...
                }
                descriptionTextField.Text = descriptionTextAnimator.Text;
                gameTextField.Text = gameTextAnimator.Text;
                window.Refresh();
            }
            Console.CursorVisible = true;
            Console.ResetColor();
            Console.Clear();
        }
    }
}
