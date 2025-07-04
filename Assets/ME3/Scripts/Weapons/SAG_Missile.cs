using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class SAG_Missile : MonoBehaviour
{
    public bool isPlayerTheLauncher = true;
    GameObject Missile;
    public GameObject LaunchingPlane;
    public float Distance;

    public float timerToFuze = 3.5f;
    Rigidbody rb;
    public float MaxTurn = 40f;
    public bool ProxyFuse;

    [SerializeField] GameObject missileAlert;

    // Start is called before the first frame update
    void Start()
    {
        if (isPlayerTheLauncher)
        {
            Guide = LaunchingPlane.GetComponent<SAGMissileControl>().GuidePoint.transform;
        }
        if (missileAlert == null)
        {
            missileAlert = GameObject.FindGameObjectWithTag("BattleUI/MissileAlert");
        }
        rb = GetComponent<Rigidbody>();
        Missile = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        timerToFuze -= Time.deltaTime;

        if (timerToFuze <= -30f)
        {
			GetComponent<RocketScript>().Explosion();
        }

        if (timerToFuze <= 0f)
        {
            ProxyFuse = true;
            if (Guide != null)
            {
                Guidance();
                Distance = Vector3.Distance(transform.position, Guide.transform.position);
                // RWRreturn();
            }
        }

        rb.linearVelocity = transform.forward * rb.linearVelocity.magnitude;

        CalculateGForce();
    }

    public Transform Guide;
    Quaternion rotation;
    void Guidance()
    {

        if (isPlayerTheLauncher)
        {
            rotation = Quaternion.LookRotation(Guide.position - transform.position);
        }
        else if (!isPlayerTheLauncher)
        {
            rotation = Quaternion.LookRotation(Utilities.FirstOrderIntercept(transform.position, rb.linearVelocity, rb.linearVelocity.magnitude, Guide.transform.position, Guide.GetComponent<Rigidbody>().linearVelocity) - transform.position);
        }
        float angle = Quaternion.Angle(transform.rotation, rotation);
        float timetocomplete = angle / MaxTurn;
        float donePercentage = Mathf.Min(1f, Time.fixedDeltaTime / timetocomplete);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, donePercentage);

        if (Guide == null)
        {
            return;
        }
    }



    void RWRreturn()
    {
        if (!isPlayerTheLauncher && missileAlert != null)
        {
            missileAlert.GetComponent<TMP_Text>().enabled = true;
            missileAlert.GetComponent<AudioSource>().enabled = true;
        }
    }

    private void OnDisable()
    {
        /*if (!isPlayerTheLauncher)
        {
            if(missileAlert != null)
            {
                missileAlert.GetComponent<TMP_Text>().enabled = false;
                missileAlert.GetComponent<AudioSource>().enabled = false;
                print("Disable RWR warning: " + missileAlert.GetComponent<TMP_Text>().enabled + " & " + missileAlert.GetComponent<AudioSource>().enabled);
            }
        }*/
    }

    Vector3 lastVelocity;
    Vector3 LocalGForce;
    [SerializeField] public float gForce;
	void CalculateGForce()
{
		// Get the change in velocity over time (acceleration)
		Vector3 acceleration = (rb.linearVelocity - lastVelocity) / Time.fixedDeltaTime;
		lastVelocity = rb.linearVelocity;

		// Remove the component of acceleration in the direction of velocity (i.e., forward acceleration)
		Vector3 velocityDir = rb.linearVelocity.normalized;
		Vector3 lateralAcceleration = Vector3.ProjectOnPlane(acceleration, velocityDir);

		// Compute G-force based only on lateral acceleration
		float lateralG = lateralAcceleration.magnitude / 9.81f;

		gForce = lateralG;
	}
}
