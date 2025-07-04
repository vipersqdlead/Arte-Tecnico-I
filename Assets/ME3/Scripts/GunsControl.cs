using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunsControl : MonoBehaviour
{
    [SerializeField] AircraftHub aircraftHub;
    public Gun[] guns;
    public bool isPlayer;
    public bool trigger;
	
	public float baseVelocity;
    public bool useConvergence;
    public bool hasDynamicConvergence;
    public float gunConvergenceDistance = 500f;
    public Vector3 convergencePoint;
	public int mainGunroundsFired, AGRoundsFired, totalRoundsFired;

    public Gun[] additionalGuns;
    public GameObject[] additionalGunsGO;
    public bool enableAG;
    [SerializeField] float AGTimer;

    private void Awake()
    {
        if (aircraftHub == null)
        {
            aircraftHub = GetComponent<AircraftHub>();
        }
    }

    private void Update()
    {
        if (isPlayer)
        {
            if (Input.GetAxis("FireCannon") != 0)
            {
                trigger = true;
            }
            else
            {
                trigger = false;
            }
        }
		
        ApplyConvergence();
		baseVelocity = aircraftHub.rb.linearVelocity.magnitude;
        if(trigger)
        {
            FireGuns();
        }

        if (enableAG)
        {
            AGTimer -= Time.deltaTime;
            if(AGTimer <= 0f)
            {
                foreach (GameObject AGgun in additionalGunsGO)
                {
                    AGgun.SetActive(false);
                }
                enableAG = false;
            }
        }
    }

    [SerializeField] FlightModel sightLockedTarget;
    void ApplyConvergence()
    {
        if(useConvergence)
        {
            if (!hasDynamicConvergence)
            {
                convergencePoint = transform.position + (transform.forward * gunConvergenceDistance);
                foreach(Gun gun in guns)
                {
                    //Quaternion lookRot = Quaternion.LookRotation(gun.transform.forward, convergencePoint);
                    //gun.transform.rotation = Quaternion.Euler()
                    gun.transform.LookAt(convergencePoint);
                }
				foreach(Gun gun in additionalGuns)
                {
                    //Quaternion lookRot = Quaternion.LookRotation(gun.transform.forward, convergencePoint);
                    //gun.transform.rotation = Quaternion.Euler()
                    if(gun != null)
					{
						gun.transform.LookAt(convergencePoint);
					}
                }
            }

            if (hasDynamicConvergence)
            {
                if (aircraftHub == null)
                {
                    aircraftHub = GetComponent<AircraftHub>();
                }
                if (sightLockedTarget == null)
                {
                    sightLockedTarget = aircraftHub.fm.target;
                }
                if (sightLockedTarget != null)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, sightLockedTarget.transform.position);
                    convergencePoint = transform.position + (transform.forward * distanceToTarget);
                    foreach (Gun gun in guns)
                    {
                        //Quaternion lookRot = Quaternion.LookRotation(gun.transform.forward, convergencePoint);
                        //gun.transform.rotation = Quaternion.Euler()
                        gun.transform.LookAt(convergencePoint);
                    }
					foreach(Gun gun in additionalGuns)
					{
						//Quaternion lookRot = Quaternion.LookRotation(gun.transform.forward, convergencePoint);
						//gun.transform.rotation = Quaternion.Euler()
						if(gun != null)
						{
							gun.transform.LookAt(convergencePoint);
						}
					}
                }
                else
                {
                    convergencePoint = transform.position + (transform.forward * 600f);
                    foreach (Gun gun in guns)
                    {
                        //Quaternion lookRot = Quaternion.LookRotation(gun.transform.forward, convergencePoint);
                        //gun.transform.rotation = Quaternion.Euler()
                        gun.transform.LookAt(convergencePoint);
                    }					
					foreach(Gun gun in additionalGuns)
					{
						//Quaternion lookRot = Quaternion.LookRotation(gun.transform.forward, convergencePoint);
						//gun.transform.rotation = Quaternion.Euler()
						if(gun != null)
						{
							gun.transform.LookAt(convergencePoint);
						}
					}
                }
                // algo
            }
        }
    }

    void FireGuns()
    {
		int _tempShotsFiredMG = 0;
		foreach(Gun gun in guns)
		{
			if(gun == null)
					continue;
			gun.baseVelocity = baseVelocity;
            gun.Fire();
			//mainGunroundsFired = gun.shotsFired - mainGunroundsFired;
			_tempShotsFiredMG += gun.shotsFired;
		}
		mainGunroundsFired = _tempShotsFiredMG;
		
        if(enableAG)
        {
			int _tempShotsFiredAG = 0;
			foreach(Gun gun in additionalGuns)
			{
				if(gun == null)
					continue;
				gun.baseVelocity = baseVelocity;
				gun.Fire();
				_tempShotsFiredAG += gun.shotsFired;
			}
			AGRoundsFired = _tempShotsFiredAG;
        }
    }

    public void EnableAG()
    {
        enableAG = true;
        AGTimer += 240f;
        foreach (GameObject AGgun in additionalGunsGO)
        {
            AGgun.SetActive(true);
        }
    }
}
