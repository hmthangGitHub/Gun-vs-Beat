using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public Transform startPos;
    public Transform targetPos;
    public List<string> matchedPayloads = new List<string>();
    List<KoreographyEvent> laneEvents = new List<KoreographyEvent>();
	[EventID]
	public string eventID;
	private void Start()
    {
		var playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(0);

		// Grab all the events out of the Koreography.
		KoreographyTrack rhythmTrack = playingKoreo.GetTrackByID(eventID);
		List<KoreographyEvent> rawEvents = rhythmTrack.GetAllEvents();

		laneEvents.AddRange(rawEvents.Where(x => DoesMatchPayload(x.GetTextValue()))
									.Select(x => x));
	}

	public bool DoesMatchPayload(string payload)
	{
		return matchedPayloads.Any(x => x == payload);
	}
}
