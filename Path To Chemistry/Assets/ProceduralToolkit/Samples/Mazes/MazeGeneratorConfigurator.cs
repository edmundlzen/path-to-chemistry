using System.Collections;
using ProceduralToolkit.Samples.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    ///     Configurator for MazeGenerator with UI controls
    /// </summary>
    public class MazeGeneratorConfigurator : ConfiguratorBase
    {
        private const int roomSize = 2;
        private const int wallSize = 1;
        private const float gradientSaturation = 0.7f;
        private const float gradientSaturationOffset = 0.1f;
        private const float gradientValue = 0.7f;
        private const float gradientValueOffset = 0.1f;
        private const float gradientLength = 30;
        public RectTransform leftPanel;
        public ToggleGroup algorithmsGroup;
        public RawImage mazeImage;

        [Space] public MazeGenerator.Config config = new MazeGenerator.Config();

        public bool useGradient = true;
        private ColorHSV mainColor;
        private MazeGenerator mazeGenerator;

        private Texture2D texture;

        private void Awake()
        {
            config.drawEdge = DrawEdge;

            var textureWidth = MazeGenerator.GetMapWidth(config.width, wallSize, roomSize);
            var textureHeight = MazeGenerator.GetMapHeight(config.height, wallSize, roomSize);
            texture = PTUtils.CreateTexture(textureWidth, textureHeight, Color.black);
            mazeImage.texture = texture;

            var header = InstantiateControl<TextControl>(algorithmsGroup.transform.parent);
            header.Initialize("Generator algorithm");
            header.transform.SetAsFirstSibling();

            InstantiateToggle(MazeGenerator.Algorithm.RandomTraversal, "Random traversal");
            InstantiateToggle(MazeGenerator.Algorithm.RandomDepthFirstTraversal, "Random depth-first traversal");

            InstantiateControl<ToggleControl>(leftPanel).Initialize("Use gradient", useGradient, value =>
            {
                useGradient = value;
                Generate();
            });

            InstantiateControl<ButtonControl>(leftPanel).Initialize("Generate new maze", Generate);

            Generate();
            SetupSkyboxAndPalette();
        }

        private void Update()
        {
            UpdateSkybox();
        }

        private void Generate()
        {
            StopAllCoroutines();

            texture.Clear(Color.black);
            texture.Apply();

            mazeGenerator = new MazeGenerator(config);

            GeneratePalette();
            mainColor = GetMainColorHSV();

            StartCoroutine(GenerateCoroutine());
        }

        private IEnumerator GenerateCoroutine()
        {
            while (mazeGenerator.Generate(200))
            {
                texture.Apply();
                yield return null;
            }
        }

        private void DrawEdge(Maze.Edge edge)
        {
            MazeGenerator.EdgeToRect(edge, wallSize, roomSize, out var position, out var width, out var height);

            Color color;
            if (useGradient)
            {
                var gradient01 = Mathf.Repeat(edge.origin.depth / gradientLength, 1);
                var gradient010 = Mathf.Abs((gradient01 - 0.5f) * 2);

                color = GetColor(gradient010);
            }
            else
            {
                color = GetColor(0.75f);
            }

            texture.DrawRect(position.x, position.y, width, height, color);
        }

        private Color GetColor(float gradientPosition)
        {
            var saturation = gradientPosition * gradientSaturation + gradientSaturationOffset;
            var value = gradientPosition * gradientValue + gradientValueOffset;
            return mainColor.WithSV(saturation, value).ToColor();
        }

        private void InstantiateToggle(MazeGenerator.Algorithm algorithm, string header)
        {
            var toggle = InstantiateControl<ToggleControl>(algorithmsGroup.transform);
            toggle.Initialize(
                header,
                algorithm == config.algorithm,
                isOn =>
                {
                    if (isOn)
                    {
                        config.algorithm = algorithm;
                        Generate();
                    }
                },
                algorithmsGroup);
        }
    }
}