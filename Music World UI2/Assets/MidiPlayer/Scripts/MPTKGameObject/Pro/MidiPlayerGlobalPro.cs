using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MidiPlayerTK
{
    // Singleton class to manage all global features of MPTK.
    public partial class MidiPlayerGlobal : MonoBehaviour
    {
        /// <summary>@brief
        /// Get or set the full path to SoundFont file (.sf2) or URL to loaded. 
        /// Defined in the MidiPlayerGlobal editor inspector. 
        /// Must start with file:// or http:// or https://.
        /// @version Maestro Pro 
        /// </summary>
        public string MPTK_LiveSoundFont;

        /// <summary>@brief 
        /// Status of the last SoundFonrloaded. The status is updated in a coroutine, so the status can change at each frame.
        /// @version 2.11.2
        /// </summary>
        [HideInInspector]
        public static LoadingStatusSoundFontEnum MPTK_StatusLastSoundFontLoaded;


        /// <summary>@brief
        /// Change the current Soundfont on fly. If MidiFilePlayer are running, they are stopped and optionally restarted.
        /// @version Maestro Pro 
        /// </summary>
        /// <param name="name">SoundFont name</param>
        /// <param name="restartPlayer">if a MIDI is playing, restart the current playing midi</param>
        public static void MPTK_SelectSoundFont(string name, bool restartPlayer = true)
        {
            if (Application.isPlaying)
                Routine.RunCoroutine(SelectSoundFontThread(name, restartPlayer), Segment.RealtimeUpdate);
            else
                SelectSoundFont(name);
        }

        /// <summary>@brief
        /// Set default soundfont
        /// @version Maestro Pro 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="restartPlayer"></param>
        /// <returns></returns>
        private static IEnumerator<float> SelectSoundFontThread(string name, bool restartPlayer = true)
        {
            if (!string.IsNullOrEmpty(name))
            {
                int index = CurrentMidiSet.SoundFonts.FindIndex(s => s.Name == name);
                if (index >= 0)
                {
                    MidiPlayerGlobal.CurrentMidiSet.SetActiveSoundFont(index);
                    MidiPlayerGlobal.CurrentMidiSet.Save();
                }
                else
                {
                    Debug.LogWarning("SoundFont not found: " + name);
                    yield return 0;
                }
            }
            // Load selected soundfont
            yield return Routine.WaitUntilDone(Routine.RunCoroutine(LoadSoundFontThread(restartPlayer), Segment.RealtimeUpdate));
        }

        /// <summary>@brief
        /// Select and load a SF when editor
        /// @version Maestro Pro 
        /// </summary>
        /// <param name="name"></param>
        private static void SelectSoundFont(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                int index = CurrentMidiSet.SoundFonts.FindIndex(s => s.Name == name);
                if (index >= 0)
                {
                    MidiPlayerGlobal.CurrentMidiSet.SetActiveSoundFont(index);
                    MidiPlayerGlobal.CurrentMidiSet.Save();
                    // Load selected soundfont
                    LoadSoundFont();
                }
                else
                {
                    Debug.LogWarning("SoundFont not found " + name);
                }
            }
        }

        /// 
        /// <summary>
        /// Loads a SoundFont dynamically while the application is running.
        /// The SoundFont can be loaded from a local file, a web resource, or a cache.
        /// If any MIDI files are currently playing, they will be restarted.
        /// Loading is performed in the background (coroutine), so the method returns immediately.
        /// @version Maestro Pro - updated 2.11.2
        /// @note See also:
        ///     - #MPTK_LiveSoundFont
        ///     - #MPTK_StatusLastSoundFontLoaded
        ///     - #MPTK_LoadSoundFontAtStartup
        ///     - #MPTK_PathSoundFontCache
        ///     - #MPTK_SoundFontLoaded
        ///     - #MPTK_SoundFontIsReady
        /// </summary>
        /// <param name="pPathSF">The full path to the SoundFont file. Must start with file:// for local desktop loading, or with http:// or https:// for loading from a web resource.
        /// If null, the path defined in MPTK_LiveSoundFont is used.</param>
        /// <param name="defaultBank">The default bank to use for instruments. Set to -1 to select the first bank.</param>
        /// <param name="drumBank">The bank to use for the drum kit. Set to -1 to select the last bank.</param>
        /// <param name="restartPlayer">Whether to restart the MIDI player if needed. Default is true.</param>
        /// <param name="useCache">Whether to reuse previously downloaded SoundFonts if available. Default is true.</param>
        /// <param name="saveCache">V1.14.0 - Whether to store the loaded SoundFont in a local cache. Default is true. If set to false, the SoundFont is deleted after loading.</param>
        /// <param name="log">Whether to display log messages. Default is false.</param>
        /// <returns>
        ///     - true if loading is in progress. Use OnEventPresetLoaded to get information when loading is complete, for example, MPTK_StatusLastSoundFontLoaded.
        ///     - false if an error is detected in the parameters. The callback OnEventPresetLoaded is not called if the return value is false.
        /// </returns>

        static public bool MPTK_LoadLiveSF(string pPathSF = null, int defaultBank = -1, int drumBank = -1, bool restartPlayer = true, bool useCache = true, bool saveCache=true,  bool log = false)
        {
            MPTK_StatusLastSoundFontLoaded = LoadingStatusSoundFontEnum.InProgress;
            timeToDownloadSoundFont = TimeSpan.Zero;
            timeToLoadSoundFont = TimeSpan.Zero;
            timeToLoadWave = TimeSpan.Zero;
            MPTK_CountWaveLoaded = 0;

            if (!string.IsNullOrEmpty(pPathSF))
                instance.MPTK_LiveSoundFont = pPathSF;

            if (string.IsNullOrEmpty(instance.MPTK_LiveSoundFont))
            {
                MPTK_StatusLastSoundFontLoaded = LoadingStatusSoundFontEnum.InvalidURL;
                Debug.LogWarning("MPTK_LoadLiveSF: SoundFont path not defined");
            }
            else if (!instance.MPTK_LiveSoundFont.ToLower().StartsWith("file://") &&
                     !instance.MPTK_LiveSoundFont.ToLower().StartsWith("http://") &&
                     !instance.MPTK_LiveSoundFont.ToLower().StartsWith("https://"))
            {
                MPTK_StatusLastSoundFontLoaded = LoadingStatusSoundFontEnum.InvalidURL;
                Debug.LogWarning("MPTK_LoadLiveSF: path to SoundFont must start with file:// or http:// or https:// - found: '" + instance.MPTK_LiveSoundFont + "'");
            }
            else
            {
                MidiSynth[] synths = FindObjectsByType<MidiSynth>(FindObjectsSortMode.None);
                if (Application.isPlaying)
                    Routine.RunCoroutine(ImSoundFont.LoadLiveSF(instance.MPTK_LiveSoundFont, defaultBank, drumBank, synths, restartPlayer, useCache, saveCache, log), Segment.RealtimeUpdate);
                else
                    Routine.RunCoroutine(ImSoundFont.LoadLiveSF(instance.MPTK_LiveSoundFont, defaultBank, drumBank, synths, restartPlayer, useCache, saveCache, log), Segment.EditorUpdate);
                return true;
            }
            return false;
        }

        // not yet available ... perhaps never!
        static public bool MPTK_MergeLiveSF(string pPathSF)
        {
            string pathSF = string.IsNullOrEmpty(pPathSF) ? instance.MPTK_LiveSoundFont : pPathSF;

            if (string.IsNullOrEmpty(pathSF))
                Debug.LogWarning("MPTK_MergeLiveSF: SoundFont path not defined");
            else if (!pathSF.ToLower().StartsWith("file://") &&
                     !pathSF.ToLower().StartsWith("http://") &&
                     !pathSF.ToLower().StartsWith("https://"))
                Debug.LogWarning("MPTK_MergeLiveSF: path to SoundFont must start with file:// or http:// or https:// - found: '" + pathSF + "'");
            else
            {
                //           Routine.RunCoroutine(ImSoundFont.MergeLiveSF(pathSF), Segment.RealtimeUpdate);
                return true;
            }
            return false;
        }
    }
}
