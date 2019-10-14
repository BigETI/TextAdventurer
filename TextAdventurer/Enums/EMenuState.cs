/// <summary>
/// Text Adventurer namespace
/// </summary>
namespace TextAdventurer
{
    /// <summary>
    /// Menu state enumerator
    /// </summary>
    internal enum EMenuState
    {
        /// <summary>
        /// Nothing
        /// </summary>
        Nothing,

        /// <summary>
        /// Splash screen
        /// </summary>
        SplashScreen,

        /// <summary>
        /// Mein menu
        /// </summary>
        MainMenu,

        /// <summary>
        /// Load save states (not implemented)
        /// </summary>
        LoadSaveStates,

        /// <summary>
        /// New game
        /// </summary>
        NewGame,

        /// <summary>
        /// Game
        /// </summary>
        Game,

        /// <summary>
        /// Pause
        /// </summary>
        Pause,

        /// <summary>
        /// Save save states (not implemented)
        /// </summary>
        SaveSaveState,

        /// <summary>
        /// End game
        /// </summary>
        EndGame
    }
}
