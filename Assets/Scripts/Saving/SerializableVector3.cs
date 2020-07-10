using UnityEngine;

namespace RPG.Saving
{

    [System.Serializable]
    public class SerializableVector3
    {
        private float x, y, z;

        public SerializableVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public SerializableVector3(Vector3 vector)
               : this(vector.x, vector.y, vector.z)
        {
        }

        public Vector3 GetVector3()
        {
            return new Vector3(x, y, z);
        }

    }
}