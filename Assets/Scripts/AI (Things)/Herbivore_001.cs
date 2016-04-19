﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Herbivore_001 : AIEntity_Animal
{

	override public void Start() {
		base.Start ();
	}


	override public bool checkHealth() {
		
		float daysSinceEaten = dayclock.secondsToDays (Time.time - lastTimeEaten);
		if (daysSinceEaten > numDaysWithoutFood) {
			doDamage (30);
			daysSinceEaten = Time.time;
		}
		//case: Old age
		if (DaysOld > lifeSpanInDays) {
			Destroy (this);
			return true;
		}
		//case: Health < 0
		if (health <= 0) {
			Destroy (this);
			return true;
		}	

		return false;
	}
}

