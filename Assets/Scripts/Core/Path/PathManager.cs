using System.Collections.Generic;
using Tower;
using UnityEngine;
using UnityEngine.Pool;
using UnitySingleton;

namespace Core.PathHandler
{
    public class PathManager : MonoSingleton<PathManager>
    {
        [SerializeField, PrefabObjectOnly] private Path pathRendererPrefab;

        private LineRenderer hintLine;
        private ObjectPool<Path> lineRendersPool;

        private List<Path> playerLineRenderers;

        [SerializeField, InLineEditor] private Material validMaterial;
        [SerializeField, InLineEditor] private Material inValidMaterial;
        [SerializeField] private PathVisual[] pathMaterial;

        protected override void OnInitialized()
        {
            lineRendersPool = new(OnCreateRenderers, OnGetRenderers, OnReleaseRenderers, OnDestroyRenderers, true, 20, 100);
            transform.position = Vector3.zero;
            playerLineRenderers = new();
        }

        private void OnEnable()
        {
            hintLine = transform.GetComponentInChildren<LineRenderer>();
            hintLine.transform.position = Vector3.zero;
        }

        /// <summary>
        /// Get and set hint line renderer
        /// </summary>
        /// <param name="origin"> origin of path in world coordinate</param>
        public void GetHintLine(Vector3 origin)
        {
            hintLine.enabled = true;
            hintLine.SetPosition(0, origin);
            hintLine.SetPosition(1, origin);
        }

        /// <summary>
        /// Update the enpoint of line renderer
        /// </summary>
        /// <param name="currentPosition">current position in world coordinate</param>
        /// <param name="isDrawablePath">whether the final path is drawable or not</param>
        public void UpdateHintLine(Vector3 currentPosition, bool isDrawablePath)
        {
            hintLine.SetPosition(1, currentPosition);
            if (isDrawablePath)
            {
                //TODO: Set the path color to blue
                hintLine.material = validMaterial;
            }
            else
            {
                //TODO: Set the path color to black or red
                hintLine.material = inValidMaterial;
            }
        }

        /// <summary>
        /// Remove the Hint Line
        /// </summary>
        public void RemoveHintLine()
        {
            hintLine.SetPosition(0, Vector3.zero);
            hintLine.SetPosition(1, Vector3.zero);
            hintLine.enabled = false;
        }

        /// <summary>
        /// Create the Path
        /// </summary>
        /// <param name="from">starting point in world coordinate</param>
        /// <param name="to">ending point in world Coordinate</param>
        /// <returns></returns>
        public Path CreatePath(Vector3 from, Vector3 to, TowerBase tower)
        {
            Path path = lineRendersPool.Get();
            Material pathMat = validMaterial;
            for (int i = 0; i < pathMaterial.Length; i++)
            {
                PathVisual visual = pathMaterial[i];
                if (visual.owner == tower.TowerOwner)
                {
                    pathMat = visual.material;
                    break;
                }
            }
            path.DrawPath(true, to, from, tower, pathMat);
            playerLineRenderers.Add(path);
            return path;
        }

        /// <summary>
        /// Remove the path between tower
        /// </summary>
        /// <param name="path">Which line renderer?</param>
        /// <returns></returns>
        public Path RemovePath(Path path)
        {
            lineRendersPool.Release(path);
            if (playerLineRenderers.Contains(path))
            {
                playerLineRenderers.Remove(path);
            }
            return path;
        }

        #region ObjectPoolingMethods

        private Path OnCreateRenderers()
        {
            return Instantiate(pathRendererPrefab, Vector3.zero, Quaternion.identity);
        }

        private void OnGetRenderers(Path path)
        {
            path.gameObject.SetActive(true);
            path.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        private void OnReleaseRenderers(Path path)
        {
            path.gameObject.SetActive(false);
        }

        private void OnDestroyRenderers(Path path)
        {
            Destroy(path.gameObject);
        }

        #endregion ObjectPoolingMethods
    }
}