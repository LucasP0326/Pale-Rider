using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DoorScript
{
	[RequireComponent(typeof(AudioSource))]


public class Door : MonoBehaviour{
	public bool open;
	public bool locked;
	public float smooth = 1.0f;
	float DoorOpenAngle = -90.0f;
    float DoorCloseAngle = 0.0f;
	public AudioSource asource;
	public AudioClip openDoor,closeDoor;
	// Use this for initialization
	void Start () {
		asource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (open)
		{
            var target = Quaternion.Euler (0, DoorOpenAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
		}
		else
		{
            var target1= Quaternion.Euler (0, DoorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth);
		}  
	}

	public void OpenDoor()
	{
		if (!locked)
		{
			StartCoroutine(DoorColliderSwitch());
			open =!open;
			asource.clip = open?openDoor:closeDoor;
			asource.Play ();
		}
		else
			Debug.Log("Door is Locked");
	}

	private IEnumerator DoorColliderSwitch()
	{
		GetComponent<Collider>().enabled = false;
		yield return new WaitForSeconds(1);
		GetComponent<Collider>().enabled = true;
	}
}
}