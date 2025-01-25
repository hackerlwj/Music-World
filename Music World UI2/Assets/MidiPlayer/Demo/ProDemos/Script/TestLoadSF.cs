/// Simple script to dynamically SoundFont at startup
/// To be done in next Maestro version:
///     - Try to get ionformation from the SF: MidiPlayerGlobal.ImSFCurrent.SoundFontName is not defined
///     - Create a local cache of dowloaded SF to avoid downloading at each run

using MidiPlayerTK;
using System;
using UnityEngine;
using UnityEngine.UI;


public class TestLoadSF : MonoBehaviour
{
    public MidiFilePlayer midiFilePlayer; // must be set with the inspector
    public MidiStreamPlayer midiStreamPlayer; // must be set with the inspector

    // URL defined in the inspector
    public string URLSoundFontAtStart; // example: "https://mptkapi.paxstellar.com/Piano.sf2";
    public string URLSoundFontAtRun; // example:  "https://mptkapi.paxstellar.com/GSv1471.sf2";

    // display or interreact with the UI
    public Text TextInitialSoundFont;
    public InputField InputURLSoundFontAtRun;
    public Text TextStatus;

    public Toggle ToggleSaveToCache;
    public Toggle ToggleLoadFromFontCache;

    MPTKEvent midiEvent;

    // Start is called before the first frame update
    void Start()
    {
        MidiPlayerGlobal.MPTK_StatusLastSoundFontLoaded = LoadingStatusSoundFontEnum.Unknown;

        if (midiStreamPlayer == null)
            Debug.LogWarning("Can't find a MidiStreamPlayer Prefab in the current scene hierarchy. You can add it with the Maestro menu in Unity editor.");
        else
            midiStreamPlayer.MPTK_LogEvents = true;

        if (midiFilePlayer == null)
            Debug.LogWarning("Can't find a midiFilePlayer Prefab in the current scene hierarchy. You can add it with the Maestro menu in Unity editor.");
        else
            // For unknown reason, MIDI thread must be hosted by a dedicated (in progress)
            midiFilePlayer.AudioThreadMidi = true;

        // Set OnEventPresetLoaded Unity Start event (not in Awake because MidiPlayerGlobal could be not yet available)
        MidiPlayerGlobal.OnEventPresetLoaded.AddListener(EndLoadingSF);

        if (string.IsNullOrEmpty(URLSoundFontAtStart))
            TextInitialSoundFont.text = "No SF defined at start, see LoadingSoundFontAtRuntime inspector.";
        else
            TextInitialSoundFont.text = URLSoundFontAtStart;
        InputURLSoundFontAtRun.text = URLSoundFontAtRun;

        // Start downloading the SF defined in the inspector
        if (!string.IsNullOrEmpty(URLSoundFontAtStart))
        {
            Debug.Log($"Demo - The SoundFont {URLSoundFontAtStart} is defined in the inspector for LoadingSoundFontAtRuntime, load it at start.");
            MidiPlayerGlobal.MPTK_LoadLiveSF(pPathSF: URLSoundFontAtStart, useCache: ToggleLoadFromFontCache.isOn, saveCache: ToggleSaveToCache.isOn, log: true);
        }
    }

    /// <summary>@brief
    /// Run when SF is loaded.
    /// </summary>
    public void EndLoadingSF()
    {
        // when SoundFont is dynamically loaded, MidiPlayerGlobal.ImSFCurrent.SoundFontName not contains name - TBD

        Debug.LogFormat($"End loading: '{MidiPlayerGlobal.ImSFCurrent?.SoundFontName}' Status: {MidiPlayerGlobal.MPTK_StatusLastSoundFontLoaded}");
        Debug.Log("Load statistique");
        Debug.Log($"   Time To Download SF:     {Math.Round(MidiPlayerGlobal.MPTK_TimeToDownloadSoundFont.TotalSeconds, 3)} second");
        Debug.Log($"   Time To Load SoundFont:  {Math.Round(MidiPlayerGlobal.MPTK_TimeToLoadSoundFont.TotalSeconds, 3)} second");
        Debug.Log($"   Time To Load Samples:    {Math.Round(MidiPlayerGlobal.MPTK_TimeToLoadWave.TotalSeconds, 3).ToString()} second");
        Debug.Log($"   Presets Loaded: {MidiPlayerGlobal.MPTK_CountPresetLoaded}");
        Debug.Log($"   Samples Loaded: {MidiPlayerGlobal.MPTK_CountWaveLoaded}");
    }

    /// <summary>
    /// Activated by UI
    /// </summary>
    public void LoadSF()
    {
        // Load the SoundFont defined in the UI.
        // Just after the call, MidiPlayerGlobal.MPTK_SoundFontLoaded is set to false
        // Set to true when SF is loaded
        if (!MidiPlayerGlobal.MPTK_LoadLiveSF(pPathSF: InputURLSoundFontAtRun.text, useCache: ToggleLoadFromFontCache.isOn, saveCache: ToggleSaveToCache.isOn, log: true))
            Debug.LogWarning($"Error when loading the SoundFont");
        Debug.Log($"SoundFont folder: {MidiPlayerGlobal.MPTK_PathSoundFontCache}");
    }

    public void ShowCacheFolder()
    {
        Application.OpenURL("file://" + MidiPlayerGlobal.MPTK_PathSoundFontCache);
    }

    public void PlayOneNote(int note)
    {
        if (!MidiPlayerGlobal.MPTK_SoundFontLoaded)
            Debug.Log("SoundFont is not loaded");
        else
        {
            // In case of synth has been stopped
            midiStreamPlayer.MPTK_StartSynth();

            // playing a NoteOn
            midiEvent = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,
                Value = note,
                Channel = 0,
                Velocity = 0x64, // Sound can vary depending on the velocity
                Delay = 0,
                Duration = 2000
            };

            midiStreamPlayer.MPTK_PlayEvent(midiEvent);
        }
    }

    public void PlayMidi()
    {
        if (!MidiPlayerGlobal.MPTK_SoundFontLoaded)
            Debug.Log("SoundFont is not loaded");
        else
            midiFilePlayer.MPTK_Play();
    }
    // Update is called once per frame
    void Update()
    {
        // MPTK_SoundFontLoaded: loading status for SoundFont from external file or URL.
        //      False at start.
        //      True when a SF downloaded or loaded from the cache is ready.
        TextStatus.text = "MPTK_SoundFontLoaded (External): " + (MidiPlayerGlobal.MPTK_SoundFontLoaded ? "<color=green>Loaded</color>" : "<color=red>Not Loaded</color>");

        // MPTK_SoundFontIsReady: loading status for SoundFont from resources folder.
        //      False at start.
        //      True when a SF defined in the Maestro setup is ready.
        //      Stay false if no SF defined in Maestro
        TextStatus.text += "     -     MPTK_SoundFontIsReady (Maestro): " + (MidiPlayerGlobal.MPTK_SoundFontIsReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>");

        TextStatus.text += "\nMPTK_StatusLastSoundFontLoaded: " + MidiPlayerGlobal.MPTK_StatusLastSoundFontLoaded.ToString();
    }

    public void StopAllSounds()
    {
        if (midiEvent != null)
            midiEvent.StopEvent();
        midiStreamPlayer.MPTK_StopSynth();

        midiFilePlayer.MPTK_Stop(true);
        midiFilePlayer.MPTK_StopSynth();
    }
}
