using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour {

    #region Enum

    /// <summary>
    /// Enum controlling what screen is being shown.
    /// </summary>
    private enum TitleState {
        /// <summary>
        /// The Title screen that is just a click to continue screen.
        /// </summary>
        Title, 
        /// <summary>
        /// The screen where we enter dan's name.
        /// </summary>
        NameDan,
        /// <summary>
        /// The Dan intro screen, clicking this will hide the title screen and go to to the main game.
        /// </summary>
        DanIntro,
        /// <summary>
        /// The End screen.
        /// </summary>
        EndScreen
    }

    #endregion

    #region Data

    /// <summary>
    /// The main title screen.
    /// </summary>
    [SerializeField]
    private GameObject mainTitle = null;

    /// <summary>
    /// The screen where we name dan.
    /// </summary>
    [SerializeField]
    private NameDanScreen nameDanScreen = null;

    /// <summary>
    /// The screen where we see the dan intro.
    /// </summary>
    [SerializeField]
    private DanIntroductionScreen danIntroScreen = null;

    /// <summary>
    /// The completion screen.
    /// </summary>
    [SerializeField]
    private GameCompletionScreen completionScreen = null;

    /// <summary>
    /// The name of the dan.
    /// </summary>
    private string danName = null;

    /// <summary>
    /// The current title state of the title screen.
    /// </summary>
    private TitleState currentTitleState = TitleState.Title;

    #endregion

    #region Monobehaviour

    /// <summary>
    /// Handles starting up the dan game.
    /// </summary>
    private void Start() {
        this.SetTitleState(TitleState.Title);
    }

    #endregion

    #region Title Screen Control

    /// <summary>
    /// Handles the title screen being tapped.
    /// </summary>
    public void HandleTitleScreenTapped() {
        this.SetTitleState(TitleState.NameDan);
    }

    /// <summary>
    /// Handles registering the dan name.
    /// </summary>
    /// <param name="enteredName">Entered name.</param>
    private void RegisterDanName(string enteredName) {
        this.danName = enteredName;
        this.SetTitleState(TitleState.DanIntro);
    }

    /// <summary>
    /// Shows the dan intro.
    /// </summary>
    /// <param name="danName">Dan name.</param>
    public void ShowDanIntro(string danName) {
        this.danIntroScreen.gameObject.SetActive((true));
    }

    /// <summary>
    /// Ensures that the correct screen is visible.
    /// </summary>
    /// <param name="titleState">Title state.</param>
    private void SetTitleState(TitleState titleState) {
        this.mainTitle.gameObject.SetActive(titleState == TitleState.Title);
        this.nameDanScreen.gameObject.SetActive(titleState == TitleState.NameDan);
        this.danIntroScreen.gameObject.SetActive(titleState == TitleState.DanIntro);
        this.completionScreen.gameObject.SetActive(titleState == TitleState.EndScreen);
    }

    #endregion

}
