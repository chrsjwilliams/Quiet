using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameSceneScript : Scene<TransitionData>
{
    public bool endGame;

    public static bool hasWon { get; private set; }

    public const int LEFT_CLICK = 0;
    public const int RIGHT_CLICK = 1;

    TaskManager _tm = new TaskManager();

    private bool m_introZoom = true;
    public bool IntroZoom {  get { return m_introZoom; } }

    [SerializeField]
    private PlayerFlock m_playerFlock;
    public PlayerFlock PlayerFlock { get { return m_playerFlock; } }

    private void Start()
    {
        
    }

    internal override void OnEnter(TransitionData data)
    {

    }

    public void StopIntroZoom()
    {
        m_introZoom = false;
    }

    public void EnterScene()
    {
        

    }

    public void SwapScene()
    {
        //Services.AudioManager.SetVolume(1.0f);
        Services.Scenes.Swap<TitleSceneScript>();
    }

    public void SceneTransition()
    {
        _tm.Do
        (
            new ActionTask(SwapScene)
        );
    }

    private void EndGame()
    {
        //Services.AudioManager.FadeAudio();

    }

    public void EndTransition()
    {

    }
    
	// Update is called once per frame
	void Update ()
    {
        _tm.Update();
	}
}
