using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    // Forces executing in the editor mode.
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        /**
         * (SerializeField) -> Useful for entities that persist across various scenes.
         * For example, it can be used to track player stats, and UUID must be set 
         * directily in a player prefab.
         */
        [SerializeField] string uniqueIdentifier = "";
        // For resolving duplicated UUIDs
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        /**
         * <see cref="UnityEditor"/> namespace can not be used in the build.
         */
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            /**
             * Prefabs don't have path in the scene.
             * This is a neat trick to avoid generating UUIDs on the prefabs.
             * Using UUIDs directly on prefab is only allowed for objects that persist
             * across scenes and it must be done manually.
             */
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            // TODO: skuziti zasto ovo je ovako!
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;
        }
#endif

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }

        /**
         * Duplicated UUIDs can happen,
         * especially if we duplicate objects directly in the scene.
         */
        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate)) return true;
            if (globalLookup[candidate] == this) return true;
            /**
             * Each time one scene reloads its objects will change references (destroy/create).
             * Deleted objects will now have null reference in a lookup table.
             */
            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            /**
             * Just in case if UUID has been changed in the editor.
             */
            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
            }

            return false;
        }
    }
}
