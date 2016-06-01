﻿using UnityEngine;
using System.Collections;

public class Timing : MonoBehaviour {

    const float TICK_PERIOD_SECONDS = .01f; // The time period between ticks, in seconds. A smaller number = more frequent updates

    const float MINUTES_PER_SECOND = 1.0f / 60.0f;

    static Song Song; // the current song
    static int SongId = -1;

    public static int BeatsPerMeasure;
    public static int BeatUnit;
    public static int CurrentBar; // The current bar in the song. The first bar is 1.
    public static float CurrentBeat; // The current position in the bar, in beats. The first beat is 1.
    public static float CurrentBPM; // The current BPM of the song.

    public static bool IsPaused = false;
    static float LastTime;

    // Use this for initialization
    void Start () {
        Song = Song.GetCurrentSong();
        StartCoroutine(UpdateTiming());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator UpdateTiming()
    {
        while (true)
        {
            // Check if song is null, and continue checking until it is not
            Song = Song.GetCurrentSong();
            if (Song != null)
            {
                // If the song has changed, reset our timing and update new BPM information
                if (Song.GetId() != SongId)
                {
                    UpdateNewSong();
                }

                float currentTime = Time.time;
                if (!IsPaused)
                {
                    float deltaTime = currentTime - LastTime;

                    // Update the current beat based on the BPM and tick period
                    CurrentBeat += (CurrentBPM) * MINUTES_PER_SECOND * deltaTime;

                    // Update the bar if needed
                    if (CurrentBeat > BeatsPerMeasure + 1)
                    {
                        CurrentBeat -= BeatsPerMeasure;
                        CurrentBar++;
                    }

                    
                }
                LastTime = currentTime;
            }

            yield return new WaitForSeconds(TICK_PERIOD_SECONDS);
        }
    }

    // Called when a new song is selected, updating timing information
    static void UpdateNewSong()
    {
        LastTime = Time.time;
        SongId = Song.GetId();

        BeatsPerMeasure = Song.GetBeatsPerMeasure();
        BeatUnit = Song.GetBeatUnit();

        CurrentBar = 1;
        CurrentBeat = 1;
        CurrentBPM = (float) Song.GetCurrentBPM();
        Debug.Log("Updating new song. Time signature: " + BeatsPerMeasure + "/" + BeatUnit + ", BPM: " + CurrentBPM);
    }

    // Pause timing
    public static void Pause()
    {
        IsPaused = true;
    }

    // Unpause timing
    public static void Unpause()
    {
        IsPaused = false;
    }

    // Set the currently active bar, resetting the beat to 1.
    public static void SetBar(int bar)
    {
        CurrentBar = bar;
        CurrentBeat = 1;
    }

    // Set the current BPM. For use with metronome adjustment
    public static void SetBPM(int bpm)
    {
        CurrentBPM = bpm;
    }

    public static float GetCurrentBeat()
    {
        return CurrentBeat;
    }

    public static int GetCurrentBar()
    {
        return CurrentBar;
    }
}