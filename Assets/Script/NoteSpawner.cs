using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
	public Scroller scroller;
	public Transform startPos;
	public Transform targetPos;
	public List<string> matchedPayloads = new List<string>();
	List<KoreographyEvent> laneEvents = new List<KoreographyEvent>();
	[EventID]
	public string eventID;
	private bool isSpawning = false;
	public float sampleRate;
	public float advanceTime;	
	public int spawnSampleOffset;

	private int processEventId;

	public GameObject noteTemplate;

	public float distancePerSample;

	public Vector3 direction;

	private void Start()
	{
		var playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(0);

		// Grab all the events out of the Koreography.
		KoreographyTrack rhythmTrack = playingKoreo.GetTrackByID(eventID);
		List<KoreographyEvent> rawEvents = rhythmTrack.GetAllEvents();

		laneEvents.AddRange(rawEvents.Where(x => DoesMatchPayload(x.GetTextValue()))
									.Select(x => x));
		distancePerSample = scroller.speed / (playingKoreo.SampleRate);
		direction = (targetPos.position - startPos.position).normalized;
	}
	private int GetSpawnSampleOffset()
	{
		// Given the current speed, what is the sample offset of our current.
		float spawnDistToTarget = (startPos.position - targetPos.position).magnitude;

		// At the current speed, what is the time to the location?
		double spawnSecsToTarget = (double)spawnDistToTarget / (double)scroller.speed;

		// Figure out the samples to the target.
		return (int)(spawnSecsToTarget * this.sampleRate);
	}

	private bool DoesMatchPayload(string payload)
	{
		return true;
		//return matchedPayloads.Any(x => x == payload);
	}

	public void StartSpawn(float advanceTime, float sampleRate)
	{
		this.sampleRate = sampleRate;
		this.advanceTime = advanceTime;
		this.spawnSampleOffset = GetSpawnSampleOffset();
	}

	public void UpdateSpawning(int currentSample)
	{
		CheckSpawnNext(currentSample);
	}

	private void CheckSpawnNext(int currentSample)
	{
		// Spawn for all events within range.
		while (processEventId < laneEvents.Count &&
			   laneEvents[processEventId].StartSample < currentSample + spawnSampleOffset)
		{
			var sampleRelative = currentSample - laneEvents[processEventId].StartSample;
			var pos = (sampleRelative * distancePerSample * direction) + targetPos.position;
			var newObj = Instantiate<GameObject>(noteTemplate);
			newObj.transform.SetParent(this.transform);
			newObj.transform.position = pos;
			newObj.SetActive(true);
			processEventId++;
		}
	}

}
