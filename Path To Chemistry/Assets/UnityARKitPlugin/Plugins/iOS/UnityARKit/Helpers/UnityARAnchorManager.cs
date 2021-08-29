using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.XR.iOS
{
    public class UnityARAnchorManager
    {
        private readonly Dictionary<string, ARPlaneAnchorGameObject> planeAnchorMap;


        public UnityARAnchorManager()
        {
            planeAnchorMap = new Dictionary<string, ARPlaneAnchorGameObject>();
            UnityARSessionNativeInterface.ARAnchorAddedEvent += AddAnchor;
            UnityARSessionNativeInterface.ARAnchorUpdatedEvent += UpdateAnchor;
            UnityARSessionNativeInterface.ARAnchorRemovedEvent += RemoveAnchor;
        }


        public void AddAnchor(ARPlaneAnchor arPlaneAnchor)
        {
            var go = UnityARUtility.CreatePlaneInScene(arPlaneAnchor);
            go.AddComponent<DontDestroyOnLoad>(); //this is so these GOs persist across scene loads
            var arpag = new ARPlaneAnchorGameObject();
            arpag.planeAnchor = arPlaneAnchor;
            arpag.gameObject = go;
            planeAnchorMap.Add(arPlaneAnchor.identifier, arpag);
        }

        public void RemoveAnchor(ARPlaneAnchor arPlaneAnchor)
        {
            if (planeAnchorMap.ContainsKey(arPlaneAnchor.identifier))
            {
                var arpag = planeAnchorMap[arPlaneAnchor.identifier];
                Object.Destroy(arpag.gameObject);
                planeAnchorMap.Remove(arPlaneAnchor.identifier);
            }
        }

        public void UpdateAnchor(ARPlaneAnchor arPlaneAnchor)
        {
            if (planeAnchorMap.ContainsKey(arPlaneAnchor.identifier))
            {
                var arpag = planeAnchorMap[arPlaneAnchor.identifier];
                UnityARUtility.UpdatePlaneWithAnchorTransform(arpag.gameObject, arPlaneAnchor);
                arpag.planeAnchor = arPlaneAnchor;
                planeAnchorMap[arPlaneAnchor.identifier] = arpag;
            }
        }

        public void UnsubscribeEvents()
        {
            UnityARSessionNativeInterface.ARAnchorAddedEvent -= AddAnchor;
            UnityARSessionNativeInterface.ARAnchorUpdatedEvent -= UpdateAnchor;
            UnityARSessionNativeInterface.ARAnchorRemovedEvent -= RemoveAnchor;
        }

        public void Destroy()
        {
            foreach (var arpag in GetCurrentPlaneAnchors()) Object.Destroy(arpag.gameObject);

            planeAnchorMap.Clear();
            UnsubscribeEvents();
        }

        public List<ARPlaneAnchorGameObject> GetCurrentPlaneAnchors()
        {
            return planeAnchorMap.Values.ToList();
        }
    }
}