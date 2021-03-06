using ProceduralToolkit.Buildings;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProceduralToolkit.Samples.Buildings
{
    public class BuildingGeneratorReuse : MonoBehaviour
    {
        [SerializeField] [FormerlySerializedAs("facadePlanningStrategy")]
        private FacadePlanner facadePlanner;

        [SerializeField] [FormerlySerializedAs("facadeConstructionStrategy")]
        private FacadeConstructor facadeConstructor;

        [SerializeField] [FormerlySerializedAs("roofPlanningStrategy")]
        private RoofPlanner roofPlanner;

        [SerializeField] [FormerlySerializedAs("roofConstructionStrategy")]
        private RoofConstructor roofConstructor;

        [SerializeField] private int xCount = 10;

        [SerializeField] private int zCount = 10;

        [SerializeField] private float cellSize = 15;

        [SerializeField] private PolygonAsset foundationPolygon;

        [SerializeField] private BuildingGenerator.Config config = new BuildingGenerator.Config();

        private void Awake()
        {
            var generator = new BuildingGenerator();
            generator.SetFacadePlanner(facadePlanner);
            generator.SetFacadeConstructor(facadeConstructor);
            generator.SetRoofPlanner(roofPlanner);
            generator.SetRoofConstructor(roofConstructor);

            var xOffset = (xCount - 1) * 0.5f * cellSize;
            var zOffset = (zCount - 1) * 0.5f * cellSize;
            for (var z = 0; z < zCount; z++)
            for (var x = 0; x < xCount; x++)
            {
                config.roofConfig.type = RandomE.GetRandom(RoofType.Flat, RoofType.Hipped, RoofType.Gabled);
                var building = generator.Generate(foundationPolygon.vertices, config);
                building.position = new Vector3(x * cellSize - xOffset, 0, z * cellSize - zOffset);
            }
        }
    }
}