﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_SyncRotation : NetworkBehaviour {

	[SyncVar] private Quaternion syncPlayerRotation;
	[SyncVar] private Quaternion syncCamRotation;

	[SerializeField] private Transform playerTransform;
	[SerializeField] private Transform camTransform;
	[SerializeField] private float lerpRate = 15;

	private Quaternion lastPlayerRot;
	private Quaternion lastCamRot;
	private float threshold = 5;

	// Use this for initialization
	void Start () {
	
	}
	

	void FixedUpdate () {
		TransmitRotations ();
		LerpRotations ();
	}

	void LerpRotations() {
		if (!isLocalPlayer) {
			playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
			camTransform.rotation = Quaternion.Lerp (camTransform.rotation, syncCamRotation, Time.deltaTime * lerpRate);
		}

	}


	[Command]
	void CmdProvideRotationsToServer (Quaternion playerRot, Quaternion camRot) {
		syncPlayerRotation = playerRot;
		syncCamRotation = camRot;
	}

	[ClientCallback]
	void TransmitRotations() {
		if (isLocalPlayer && 
			(Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > threshold || Quaternion.Angle(camTransform.rotation, lastCamRot) > threshold)) {
			CmdProvideRotationsToServer (playerTransform.rotation, camTransform.rotation);
			lastPlayerRot = playerTransform.rotation;
			lastCamRot = camTransform.rotation;
		}
	}
}
