namespace TreeCity
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Mapbox.Unity.MeshGeneration.Components;
    using Mapbox.Unity.MeshGeneration.Data;
    using Mapbox.Unity.MeshGeneration.Interfaces;
    using Mapbox.Unity.MeshGeneration.Modifiers;

    [CreateAssetMenu(menuName = "TreeCity/Modifiers/Tree Prefab Modifier")]
    public class TreePrefabModifier : GameObjectModifier
    {
        [SerializeField]
        private GameObject[] _prefabs;

        [SerializeField]
        private float _scale = 0.1f;

        [SerializeField]
        private List<GameObjectModifier> _prefabModifiers;

        public override void Run(FeatureBehaviour fb, UnityTile tile)
        {
            int selpos = fb.Data.Points[0].Count / 2;
            var met = fb.Data.Points[0][selpos];

            GameObject prefab = _prefabs[Random.Range(0, _prefabs.Length)];
            var go = Instantiate(prefab);
            go.name = fb.Data.Data.Id.ToString();
            go.transform.position = met;
            go.transform.SetParent(fb.transform, false);

            var bd = go.AddComponent<FeatureBehaviour>();
            bd.Init(fb.Data);

            var tm = go.GetComponent<IFeaturePropertySettable>();
            if (tm != null)
            {
                tm.Set(fb.Data.Properties);
            }

            /// Runs modifiers on each prefab
            foreach (GameObjectModifier mod in _prefabModifiers.Where(x => x.Active))
            {
                mod.Run(bd, tile);
            }

            /// Scale tree based on its diameter
            TreeModel tree = TreeModel.ParseData(fb.Data.Properties);
            float scale = _scale;
            float runningDiameter = tree.diameter ?? 1.0f;
            while (runningDiameter > 1)
            {
                scale += 0.02f;
                runningDiameter -= 12.0f;
            }
            go.transform.localScale *= scale * Random.Range(.9f, 1.1f);

            /// Rotate tree in different directions
            float rotationY = Random.Range(0, 360);
            go.transform.localEulerAngles = new Vector3(0, rotationY, 0);
        }
    }
}