using UnityEngine;
using UnityEngine.Assertions;

namespace Decisions
{
    public class WanderDecision : Decision
    {
        public float GiveUpDistance = 0.25f;
        public float MaxWanderDistance = 0.1f;
        public float MinWanderDistance = 0.01f;
        public float WanderSpeed = 0.1f;
        public float MaxWanderAngleChange = 60.0f; //Degrees
        public GameObject TargetIndicatorPrefab;

        private Vector3 wanderDestination;
        private Gravity3D gravity3D;
        private Transform modelTransform;
        private Vector3 previousWanderDirection;
        private GameObject targetIndicator;

        // Use this for initialization
        void Start()
        {
            gravity3D = GetComponent<Gravity3D>();
            modelTransform = gravity3D.ModelTransform;
        }

        // Update is called once per frame
        void Update() {}

        public override bool Condition()
        {
            RandomizeDestination();
            return true;
        }

        public override bool UpdateDecision()
        {
            Vector3 toDestination = wanderDestination - modelTransform.position;
            float wanderDistance = WanderSpeed * Time.deltaTime;
            float distanceSquared = toDestination.sqrMagnitude;

            if (distanceSquared >= GiveUpDistance * GiveUpDistance)
            {
                previousWanderDirection = Vector3.zero;
                if (targetIndicator != null)
                {
                    GameObject.Destroy(targetIndicator);
                }
                return true;
            }

            if (distanceSquared <= wanderDistance * wanderDistance)
            {
                gravity3D.SetPosition(wanderDestination);
                if (targetIndicator != null)
                {
                    GameObject.Destroy(targetIndicator);
                }
                return true;
            }

            toDestination.Normalize();
            gravity3D.SetPosition(modelTransform.position + toDestination * wanderDistance);

            return false;
        }

        private void RandomizeDestination()
        {
            Vector3 surfaceNormal = modelTransform.position - transform.position;
            surfaceNormal.Normalize();
            //find two basis vectors perpendicular to the surface
            Vector3 basisVector1 = new Vector3(surfaceNormal.y, -surfaceNormal.x, 0);
            Vector3 basisVector2 = Vector3.Cross(surfaceNormal, basisVector1);

            Assert.AreApproximatelyEqual(Vector3.Dot(surfaceNormal, basisVector1), 0);
            Assert.AreApproximatelyEqual(Vector3.Dot(surfaceNormal, basisVector2), 0);
            Assert.AreApproximatelyEqual(Vector3.Dot(basisVector1, basisVector2), 0);

            //create a random vector on the tangent plane
            Vector3 wanderDirection;

            if (previousWanderDirection.sqrMagnitude < Mathf.Epsilon)
            {
                do
                {
                    wanderDirection = Random.Range(-1, 1) * basisVector1 + Random.Range(-1, 1) * basisVector2;
                } while (wanderDirection.sqrMagnitude < Mathf.Epsilon); //Throw out zero vector
            }
            else
            {
                //pick rotate previous vector by random angle
                float angleOffset = Random.Range(-MaxWanderAngleChange, MaxWanderAngleChange);
                Quaternion rotation = Quaternion.AngleAxis(angleOffset, surfaceNormal);
                wanderDirection = rotation * previousWanderDirection;
            }
            Assert.AreNotApproximatelyEqual(wanderDirection.sqrMagnitude, 0);
            wanderDirection.Normalize();
            
            //calculate wanderDestination and snap it to surface
            float distance = Random.Range(MinWanderDistance, MaxWanderDistance);
            wanderDestination = modelTransform.position + wanderDirection * distance;

            Vector3 toWanderDestination = wanderDestination - transform.position;
            wanderDestination = toWanderDestination.normalized * gravity3D.Radius * transform.lossyScale.x + transform.position;

            previousWanderDirection = wanderDestination - modelTransform.position;
            
            if (targetIndicator != null)
            {
                GameObject.Destroy(targetIndicator);
            }

            targetIndicator = GameObject.Instantiate(TargetIndicatorPrefab);
            targetIndicator.transform.position = wanderDestination;
        }
    }
}