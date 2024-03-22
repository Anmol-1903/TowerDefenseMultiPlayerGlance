using Tower;
using UnityEngine;

namespace Core.PathHandler
{
    public class Path : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        [field: SerializeField] public string ID { get; private set; }

        [field: SerializeField] public TowerBase towerPathOwner { get; private set; }
        [SerializeField] private LayerMask towerLayer;

        //[SerializeField] private int pathCleanerInterval = 10;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            System.Guid guid = System.Guid.NewGuid();
            ID = guid.ToString()[..8];
        }

        public void DrawPath(bool useWorldSpace, Vector3 start, Vector3 end, TowerBase pathCreatorTower, Material pathMat)
        {
            lineRenderer.useWorldSpace = useWorldSpace;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            GenerateCollider(start, end);
            towerPathOwner = pathCreatorTower;
        }

        private void GenerateCollider(Vector3 from, Vector3 to)
        {
            BoxCollider box = GetComponentInChildren<BoxCollider>();
            Vector3 dir = to - from;
            box.transform.position = (from + to) / 2;
            box.transform.forward = dir.normalized;
            box.size = new(0.1f, 0.1f, dir.magnitude);
        }

        /*    private void Update()
            {
                if (Time.frameCount % pathCleanerInterval == 0)
                {
                    PathCheck();
                }
            }*/

        /*    private void PathCheck()
            {
                int index = ConnectedManager.FindPathIndex(GetComponent<LineRenderer>());
                if (index != -1)
                {
                    return;
                }
                PathManager.Instance.RemovePath(GetComponent<LineRenderer>());
            }*/
    }
}