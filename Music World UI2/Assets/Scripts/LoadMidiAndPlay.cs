using MidiPlayerTK;
using UnityEngine;

public class LoadMidiAndPlay : MonoBehaviour
{
    public MidiFilePlayer midiFilePlayer;

    private void Awake()
    {
        // Find a MidiFilePlayer added to the scene or set it directly in the inspector.
        if (midiFilePlayer == null)
            midiFilePlayer = FindFirstObjectByType<MidiFilePlayer>();
    }

    public void StartAIPlay()
    {
        if (midiFilePlayer == null)
        {
            Debug.LogWarning("No MidiFilePlayer Prefab found in the current Scene Hierarchy. See 'Maestro / Add Prefab' in the menu.");
        }
        else
        {
            // Index of the MIDI file from the MIDI database (find it using 'Midi File Setup' from the Maestro menu).
            // Optionally, the MIDI file to load can also be defined in the inspector. Uncomment to select the MIDI programmatically.
            midiFilePlayer.MPTK_MidiIndex = 0;

            // Load the MIDI without playing it.
            MidiLoad midiloaded = midiFilePlayer.MPTK_Load();

            if (midiloaded != null)
            {
                Debug.Log($"Duration: {midiloaded.MPTK_Duration.TotalSeconds} seconds, Initial Tempo: {midiloaded.MPTK_InitialTempo}, MIDI Event Count: {midiloaded.MPTK_ReadMidiEvents().Count}");

                foreach (MPTKEvent mptkEvent in midiloaded.MPTK_MidiEvents)
                {
                    if (mptkEvent.Command == MPTKCommand.MetaEvent && mptkEvent.Meta == MPTKMeta.SetTempo)
                    {
                        // The value contains Microseconds Per Beat, convert it to BPM for clarity.
                        double bpm = MPTKEvent.QuarterPerMicroSecond2BeatPerMinute(mptkEvent.Value);
                        // Double the tempo and convert back to Microseconds Per Beat.
                        mptkEvent.Value = MPTKEvent.BeatPerMinute2QuarterPerMicroSecond(bpm * 1);
                        Debug.Log($"   Tempo doubled at tick position {mptkEvent.Tick} and {mptkEvent.RealTime / 1000f:F2} seconds. New tempo: {MPTKEvent.QuarterPerMicroSecond2BeatPerMinute(mptkEvent.Value)} BPM");
                    }
                }

                // Start playback.
                midiFilePlayer.MPTK_Play(alreadyLoaded: true);
            }
        }
    }
}