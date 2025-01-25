using UnityEngine;
using System;
using MidiPlayerTK;
using System.Threading;
using System.Diagnostics;
using MPTK.NAudio.Midi;
using System.Collections.Concurrent;

namespace DemoMPTK
{
    /// <summary>@brief
    /// Example of MVP implementation for reading MIDI event from a MIDI keyboard and play with MPTK
    /// No Unity thread needed to read MIDI event and play them, so the latency is reduced to this factors:
    ///    - MIDI input card (in particular with MIDI USB card)
    ///    - FMOD Player (try with lower buffer size)
    /// See here for detailed API doc.
    /// For the MIDI reader:
    ///     https://mptkapi.paxstellar.com/da/d70/class_midi_player_t_k_1_1_midi_keyboard.html
    /// For the MIDI player:
    ///     https://mptkapi.paxstellar.com/d9/d1e/class_midi_player_t_k_1_1_midi_stream_player.html
    ///       
    /// For testing:
    ///     - If not yet done, download and install MidiKeyboard https://paxstellar.fr/class-midikeyboard/
    ///     - Add a gameObject (empty or not) to your scene.
    ///     - Add this script to the gameObject.
    ///     - Connect your MIDI keyboard and run!
    ///     - Every MIDI event read from the keyboard is sent to the MPTK MIDI Synth.
    /// </summary>
    public class MidiKeyboardThread : MonoBehaviour
    {
        private bool midiKeyboardReady = false;
        private Thread midiThread;

        // This class is able to play MIDI event: play note, play chord, patch change, apply effect, ... see doc!
        private MidiStreamPlayer midiStreamPlayer;

        // For your future integration. add your specific code for visualization, game interaction, .... what you want in the Unity Update().
        // The music played on the MIDI keyboard will be played in background.
        private ConcurrentQueue<MPTKEvent> midiQueue = new ConcurrentQueue<MPTKEvent>();

        private void Awake()
        {
            // Search for an existing MIDI stream prefab in the scene
            midiStreamPlayer = FindFirstObjectByType<MidiStreamPlayer>();
            if (midiStreamPlayer == null)
                UnityEngine.Debug.Log("No MidiStreamPlayer Prefab found in the current Scene Hierarchy.");
        }

        private void Start()
        {
            // Midi Keyboard need to be initialized at start
            if (midiStreamPlayer != null && MidiKeyboard.MPTK_Init())
            {
                midiKeyboardReady = true;
                UnityEngine.Debug.Log(MidiKeyboard.MPTK_Version());
                // Open or refresh all input MIDI devices able to send MIDI message
                MidiKeyboard.MPTK_OpenAllInp();

                // Launch the thread able to read MIDI events and play
                midiThread = new Thread(ThreadMidiPlayer);
                midiThread.Start();
            }
        }

        private void ThreadMidiPlayer()
        {
            while (midiKeyboardReady)
            {
                try
                {
                    MidiKeyboard.PluginError status = MidiKeyboard.MPTK_LastStatus;
                    if (status != MidiKeyboard.PluginError.OK)
                        UnityEngine.Debug.LogWarning($"MIDI Keyboard error, status: {status}");

                    // Read a MIDI event if available
                    MPTKEvent midiEvent = MidiKeyboard.MPTK_Read();

                    if (midiEvent != null)
                    {
                        // Queuing for your specific code, useless for music playing.
                        midiQueue.Enqueue(midiEvent);

                        // Play!
                        midiStreamPlayer.MPTK_PlayEvent(midiEvent);
                    }

                    Thread.Sleep(1);
                }
                catch (System.Exception ex)
                {
                    UnityEngine.Debug.LogError($"ThreadMidiPlayer - {ex}");
                    break;
                }
            }
        }

        void Update()
        {
            MPTKEvent midiEvent;

            if (midiKeyboardReady && !midiQueue.IsEmpty)
            {
                if (midiQueue.TryDequeue(out midiEvent))
                {
                    // Add here your specific code for visualization, game interaction, .... what you want with full access to Unity!
                    // The music played on the MIDI keyboard will be played in background.
                    UnityEngine.Debug.Log(midiEvent.ToString());
                }
            }
        }
        private void OnApplicationQuit()
        {
            if (midiKeyboardReady)
                // Mandatory to avoid Unity crash!
                MidiKeyboard.MPTK_CloseAllInp();
            midiKeyboardReady = false;
        }
    }
}