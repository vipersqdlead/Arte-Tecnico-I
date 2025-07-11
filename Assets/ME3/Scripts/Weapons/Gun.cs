using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float shellTimer = 2f;
    public GameObject[] shells = new GameObject[4]; // For the gun belts mechanic (the gun cycles through four types of bullets that are set in the inspector. Example: incendiary/explosive/tracer/explosive. So the gun will fire in that pattern
	public List<Shell> shellList;
    int index = 0; // Index for the particular bullet that will be fired from shells[].
	int poolIndex;
    public float rateOfFireRPM = 0; // This is the reference RPM (rounds per minute) the gun fires. I just take a reference value from the internet, as it depends on the specific gun
    public float muzzleVelocity; // This is how fast the bullet goes when it's fired.
	public float baseVelocity = 0f; // Base velocity when fired from a plane or mobile platform. Its set from Gun Controller or an AA Controler.
    [SerializeField] float accuracyError = 0.0001f; // Used for gun spread
    [SerializeField] AudioSource shot; // piu piu sound
    public float rateOfFire; // This is used later, when firing.
    [SerializeField] float rofTimer; // Similar as the one above(?
	
	[SerializeField] GameObject muzzleFlash;
	[SerializeField] float muzzlePositionZOffset;

    [SerializeField]float overheatTimer;
    [SerializeField] bool overheated;
    [SerializeField] float overheatResetTimer;
    [SerializeField] float overheatedRoF;
	
	public int shotsFired;

    [SerializeField] KillCounter killCounter;


    // Start is called before the first frame update
    void Start()
    {
        rateOfFire = 1 / (rateOfFireRPM / 60); // This turns the reference RPM into a small float (how much time happens between bullets being fired)
        overheatedRoF = rateOfFire * 5f;
		InitializePool();
    }

    private void Update()
    {
        if(rofTimer <= rateOfFire)
        {
            rofTimer += Time.deltaTime; // just your typical timer
        }
		else
		{
			rofTimer = rateOfFire;
		}
			

        if (overheated)
        {
            overheatResetTimer += Time.deltaTime;
            if (overheatResetTimer >= 10f)
            {   
                overheatTimer = 0;
                overheatResetTimer = 0f;
                overheated = false;
            }
        }

        if (!overheated && overheatTimer > 0)
        {
            overheatTimer -= Time.deltaTime;
        }
    }

    public void OldFire()
    {
        if(overheatTimer <= 12f)
        {
            overheatTimer += Time.deltaTime * 2;
        }
        else
        {
            overheated = true;
        }

        if (!overheated)
        {
            if (rofTimer >= rateOfFire)
            {
                Vector3 error = new Vector3(Random.Range(-accuracyError, accuracyError), Random.Range(-accuracyError, accuracyError), 0);
                GameObject shell = Instantiate(shells[index], transform.position, transform.rotation); // Instantiates the bullet...
                shell.GetComponent<Rigidbody>().AddForce((transform.forward + error) * (muzzleVelocity + baseVelocity), ForceMode.VelocityChange); ; // Gets the rigidbody & shoots it + error)
                Shell sh = shell.GetComponent<Shell>();
                sh.SetKillEnemyDelegate(EnemyKilled); // Sets the delegate
                sh.SetHitBonusDelegate(HitBonus);
                if(shot != null)
				{
					if(shot.enabled == true)
					{
						shot.PlayOneShot(shot.clip); // Just a small audio
					}
                }
                Destroy(shell, shellTimer); // And destroy it after a pair of seconds if it didn't hit anything

                // Controlling the index so the bullets fire in the correct pattern
                if (index < shells.Length - 1)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }

                rofTimer = 0f;
            }
        }
        else if (overheated)
        {
            if (rofTimer >= overheatedRoF)
            {
                Vector3 error = new Vector3(Random.Range(-0.005f, 0.005f), Random.Range(-0.005f, 0.005f), 0);
                GameObject shell = Instantiate(shells[index], transform.position, transform.rotation); // Instantiates the bullet...
                shell.GetComponent<Rigidbody>().AddForce((transform.forward + error) * (muzzleVelocity + baseVelocity), ForceMode.VelocityChange); ; // Gets the rigidbody & shoots it + error)
                Shell sh = shell.GetComponent<Shell>();
                sh.SetKillEnemyDelegate(EnemyKilled); // Sets the delegate
                sh.SetHitBonusDelegate(HitBonus);
                if (shot != null)
                {
                    shot.PlayOneShot(shot.clip); // Just a small audio
                }
                Destroy(shell, shellTimer); // And destroy it after a pair of seconds if it didn't hit anything

                // Controlling the index so the bullets fire in the correct pattern
                if (index < shells.Length - 1)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }
                rofTimer = 0f;
            }
        }
        
    }
	
	public void Fire()
    {
        if(overheatTimer <= 12f)
        {
            overheatTimer += Time.deltaTime * 2;
        }
        else
        {
            overheated = true;
        }

        if (!overheated)
        {
			int bulletsToFire = Mathf.FloorToInt(rofTimer / rateOfFire);
			
			if(bulletsToFire > 0)
			{
				for(int i = 0; i < bulletsToFire; i++)
				{
					float spawnTimeOffset = Mathf.Abs(rofTimer - (rateOfFire * (bulletsToFire - i)));
					float distanceToMuzzle = muzzleVelocity * spawnTimeOffset;
					{
						FireBullet(shellList[poolIndex], accuracyError, distanceToMuzzle);

						// Controlling the index so the bullets fire in the correct pattern
						if (poolIndex < shellList.Count - 1)
						{
							poolIndex++;
						}
						else
						{
							poolIndex = 0;
						}
					}
					rofTimer %= rateOfFire;
				}
			}
        }
        else if (overheated)
        {
            if (rofTimer >= overheatedRoF)
            {
				FireBullet(shellList[poolIndex], accuracyError * 5f, 0f);
                if (index < shellList.Count - 1)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }
                rofTimer -= overheatedRoF;
            }
        }
        
    }

    public void SetKillCounter(KillCounter kc)
    {
        killCounter = kc;
    }

    public void EnemyKilled(bool countsAsKill, int points)
    {
        if(killCounter != null)
        {
            killCounter.GiveKill(countsAsKill, points);
        }
    }

    public void HitBonus(int points)
    {
        if (killCounter != null)
        {
            killCounter.GivePoints(points);
        }
    }
	
	public void FireBullet(Shell shell, float accuracyError, float distanceFromSpawn)
	{	         
		if(shell.gameObject.activeSelf == true)
		{
			shell.Disable(transform);
		}
        Vector3 error = new Vector3(Random.Range(-accuracyError, accuracyError), Random.Range(-accuracyError, accuracyError), 0);	
		shell.gameObject.SetActive(true);
		Vector3 pos = transform.position + (transform.forward * distanceFromSpawn);
		Vector3 force = (transform.forward + error) * (muzzleVelocity + baseVelocity);
		shell.Enable(pos, transform.rotation, force, shellTimer, transform);
		
		if(muzzleFlash != null)
		{
			Instantiate(muzzleFlash, transform.position + (transform.forward * -muzzlePositionZOffset), transform.rotation, gameObject.transform);
		}
		
        if(shot != null)
		{
			if(shot.enabled == true)
			{
				shot.PlayOneShot(shot.clip); // Just a small audio
			}
        }
		
		shotsFired++;
	}
	
	public void InitializePool()
	{
		int totalBullets = (int)(rateOfFireRPM / 60f * shellTimer);
		int _index = 0;
		
		for(int i = 0; i < totalBullets; i++)
		{
			shellList.Add(InstantiateBullet(_index));
			if (_index < shells.Length - 1)
            {
                _index++;
            }
            else
            {
                _index = 0;
            }
		}
	}
	
	public Shell InstantiateBullet(int index)
	{	        
		GameObject shell = Instantiate(shells[index], transform.position, transform.rotation, gameObject.transform); // Instantiates the bullet...
        Shell sh = shell.GetComponent<Shell>();
		sh.SetKillEnemyDelegate(EnemyKilled); // Sets the delegate
        sh.SetHitBonusDelegate(HitBonus);
		sh.Disable(transform);
		return sh;
	}
}
