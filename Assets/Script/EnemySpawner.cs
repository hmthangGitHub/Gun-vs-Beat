using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float targetDuration = 2.0f;
	public int spawnSampleOffset;
    public int sampleRate;
	public EnemyController enemyTemplate;
	public TargetSpawner targetSpawner;
	public Transform spawnPosition;
	public float radius = 30;

	public bool isStartedSpawn = false;
	private int processEventId = 0;
	List<KoreographyEvent> laneEvents = new List<KoreographyEvent>();
	[EventID]
	public string eventID;

	private void Awake()
    {
		this.enemyTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
		var playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(0);

		// Grab all the events out of the Koreography.
		KoreographyTrack rhythmTrack = playingKoreo.GetTrackByID(eventID);
		List<KoreographyEvent> rawEvents = rhythmTrack.GetAllEvents();

		laneEvents.AddRange(rawEvents.Select(x => x));
	}

    public void StartSpawn(float advanceTime, int sampleRate)
	{
        this.sampleRate = sampleRate;
		isStartedSpawn = true;
		//this.advanceTime = advanceTime;
		this.spawnSampleOffset = GetSpawnSampleOffset();
	}

	private int GetSpawnSampleOffset()
	{
		return (int)(targetDuration * this.sampleRate);
	}

	public void Spawn(float duration)
	{
		var enemy = Instantiate<EnemyController>(enemyTemplate);
		enemy.transform.parent = enemyTemplate.transform.parent;

		var randomPosition = UnityEngine.Random.onUnitSphere * radius + this.spawnPosition.position;

		enemy.gameObject.SetActive(true);
		enemy.transform.position = randomPosition;

		targetSpawner.SpawnTarget(enemy, duration);
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
			var duration = MathUltility.MapRange(sampleRelative, new Vector2(-spawnSampleOffset, 0), new Vector2(targetDuration, 0));
			Spawn(duration);
			processEventId++;
		}
	}
}
