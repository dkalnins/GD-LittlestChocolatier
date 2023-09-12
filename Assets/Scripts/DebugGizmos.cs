using System.Collections.Generic;
using UnityEngine;

public class DebugGizmos : MonoBehaviour
{
    #region SingletonRegion
    private static DebugGizmos _instance;
    public static DebugGizmos Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("DebugGizmos").AddComponent<DebugGizmos>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(_instance);
        else
            _instance = this;
    }
    #endregion

    class TemporaryGizmo
    {

        public float remainingDuration;
        public Vector3 renderLocation;
        public float radius;

        public TemporaryGizmo(Vector3 renderLocation, float radius, float duration)
        {
            remainingDuration = duration;
            this.radius = radius;
            this.renderLocation = renderLocation;
        }
    }

    private static List<TemporaryGizmo> _gizmos = new();

    public void RegisterNewDebugGizmo(Vector3 renderLocation, float radius, float duration)
    {
        _gizmos.Add(new TemporaryGizmo(renderLocation, radius, duration));
    }

    // Update is called once per frame
    void Update()
    {
        // Update timer on all Gizmos and get rid of any expired gizmos
        // We do this in reverse order to make the loop management easier
        // since we will remove expired gizmos as we go.

        int nextGizmo = _gizmos.Count - 1;
        while (nextGizmo >= 0)
        {
            TemporaryGizmo thisGizmo = _gizmos[nextGizmo];

            thisGizmo.remainingDuration -= Time.deltaTime;
            if (thisGizmo.remainingDuration <= 0)
                _gizmos.RemoveAt(nextGizmo);

            nextGizmo--;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        foreach (var gizmo in _gizmos)
        {
            Gizmos.DrawWireSphere(gizmo.renderLocation, gizmo.radius);
        }

    }

}
