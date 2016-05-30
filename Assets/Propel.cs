using UnityEngine;
using System.Collections;

public class Propel : MonoBehaviour {

	enum Mode { 
		Ready,
		Yanking,
		Holding,
	}

	Mode currentMode = Mode.Ready;

	float lerpProgress = 0f;

	Rigidbody target = null;
	Vector3 targetStartPosition;

	Transform heldObjectLocation;

	// Use this for initialization
	void Start () {
		heldObjectLocation = GameObject.Find("Player/FirstPersonCharacter/HeldObjectPosition").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
		switch(currentMode) {
		case Mode.Ready:
			if(Input.GetKeyUp(KeyCode.E)) {
				BeginYank();
			}
			break;
		case Mode.Yanking:
			lerpProgress += Time.deltaTime;
			target.MovePosition(Vector3.Lerp(targetStartPosition, heldObjectLocation.position, lerpProgress));
			if(lerpProgress >= 1) { //Magic number of seconds that yank lasts
				CompleteYank();
			}
			break;
		case Mode.Holding:
			if(Input.GetMouseButtonDown(0)) {
				Throw();
			} else {
				target.MovePosition(heldObjectLocation.position);

			}


			break;
		}
	}

	private void BeginYank() {
		print("begin yank");
		//Identify target object
		target = GetTargetFromReticle();
		if(target == null) return;

		//Begin lerp of target to position in front of character 
		lerpProgress = 0;
		currentMode = Mode.Yanking;
		targetStartPosition = target.position;

	}

	private void CompleteYank() {
		currentMode = Mode.Holding;
		target.isKinematic = true;
	}

	private void Throw() {
		currentMode = Mode.Ready;
		target.isKinematic = false;
		target.AddForce(transform.forward * 1000);
		target = null;
	}

	private Rigidbody GetTargetFromReticle() {
		//Raycast forward
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit)) {
			return hit.collider.attachedRigidbody;
		} else {
			return null;
		}
	}


}
