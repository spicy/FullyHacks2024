using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class LevelMusic : MonoBehaviour
{
    public List<Sound> songList;

    void Start()
    {
        ClearQueueAndEnqueueSongs();
    }

    private void ClearQueueAndEnqueueSongs()
    {
        //MusicSystem.Instance.TryStopAndClearQueue();

        // Enqueue songs and log any failures
        foreach (Sound song in songList)
        {
            if (!MusicManager.Instance.TryEnqueue(song))
            {
                Debug.Log("Failed to Enqueue song: " + song.name);
            }
        }
    }
}