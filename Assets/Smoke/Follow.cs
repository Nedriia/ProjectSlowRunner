using UnityEngine;

namespace MirzaBeig
{

    namespace ParticleSystems
    {

        namespace Demos
        {

            // =================================	
            // Classes.
            // =================================

            public class Follow : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...
                public GameObject ball;
                public float speed = 8.0f;
                public float distanceFromCamera = 5.0f;

                public bool ignoreTimeScale;

                public GameObject[] trails;
                public GameObject smoke;
                // =================================	
                // Functions.
                // =================================

                // ...

                void Awake()
                {

                }

                // ...

                void Start()
                {

                }

                // ...

                void Update()
                {
                    Vector3 mousePosition = Input.mousePosition;
                    mousePosition.z = distanceFromCamera;

                    Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(mousePosition);

                    float deltaTime = !ignoreTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;
                    Vector3 position = Vector3.Lerp(transform.position, ball.transform.position, 1.0f - Mathf.Exp(-speed * deltaTime));

                    transform.position = new Vector3(ball.transform.position.x,ball.transform.position.y, ball.transform.position.z);
                    if(ball.activeSelf == false)
                    {
                        Invoke("Deactivate", 1);
                    }
                }
                //
                public void TrailType(bool fire)
                {
                    if (fire)
                    {
                        smoke.GetComponent<ParticleSystem>().Play();
                        foreach (GameObject g in trails)
                        {
                            g.GetComponent<ParticleSystem>().Stop();
                        }
                    }
                    else
                    {
                        smoke.GetComponent<ParticleSystem>().Stop();
                        foreach (GameObject g in trails)
                        {
                            g.GetComponent<ParticleSystem>().Play();
                        }
                    }
                } 
                // ...
                void Deactivate()
                {
                    gameObject.SetActive(false);
                }
                void LateUpdate()
                {

                }

                // =================================	
                // End functions.
                // =================================

            }

            // =================================	
            // End namespace.
            // =================================

        }

    }

}

// =================================	
// --END-- //
// =================================
