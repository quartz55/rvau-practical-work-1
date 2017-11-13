using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class InventoryTargetManager : MonoBehaviour, ITrackableEventHandler
{

	private TrackableBehaviour _trackable;

	private void Start () {
		_trackable = GetComponent<TrackableBehaviour>();
		if (_trackable) _trackable.RegisterTrackableEventHandler(this);
	}
	
	public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED ||
		    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			UberDebug.LogChannel("inventory", "Inventory opened");
			OnOpen();
		}
		else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
		         newStatus == TrackableBehaviour.Status.NOT_FOUND)
		{
			UberDebug.LogChannel("inventory", "Inventory closed");
			OnClose();
		}
		else
		{
			OnClose();
		}
	}

	private void OnOpen()
	{
		var renderers = GetComponentsInChildren<Renderer>(true);

		foreach (var renderer in renderers)
		{
			renderer.enabled = true;
		}
	}
	
	private void OnClose()
	{
		var renderers = GetComponentsInChildren<Renderer>(true);

		foreach (var renderer in renderers)
		{
			renderer.enabled = false;
		}
	}
}
