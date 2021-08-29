using System.Collections.Generic;
using ProceduralToolkit.Buildings;
using UnityEngine;

namespace ProceduralToolkit.Samples.Buildings
{
    public abstract class ProceduralFacadeElement : ILayoutElement, IConstructible<CompoundMeshDraft>
    {
        protected const string WallDraftName = "Wall";
        protected const string GlassDraftName = "Glass";

        private const float SocleWindowWidth = 0.7f;
        private const float SocleWindowMinWidth = 0.9f;
        private const float SocleWindowHeight = 0.4f;
        private const float SocleWindowDepth = 0.1f;
        private const float SocleWindowHeightOffset = 0.1f;

        private const float EntranceDoorWidth = 1.8f;
        private const float EntranceDoorHeight = 2;
        private const float EntranceDoorThickness = 0.05f;

        private const float EntranceRoofDepth = 1;
        private const float EntranceRoofHeight = 0.15f;

        private const float EntranceWindowWidthOffset = 0.4f;
        private const float EntranceWindowHeightOffset = 0.3f;

        private const float WindowDepth = 0.1f;
        protected const float WindowWidthOffset = 0.5f;
        protected const float WindowBottomOffset = 1;
        protected const float WindowTopOffset = 0.3f;
        private const float WindowFrameWidth = 0.05f;
        private const float WindowSegmentMinWidth = 0.9f;
        private const float WindowsillWidthOffset = 0.1f;
        private const float WindowsillDepth = 0.15f;
        private const float WindowsillThickness = 0.05f;

        private const float BalconyHeight = 1;
        private const float BalconyDepth = 0.8f;
        private const float BalconyThickness = 0.1f;

        private const float AtticHoleWidth = 0.3f;
        private const float AtticHoleMinWidth = 0.5f;
        private const float AtticHoleHeight = 0.3f;
        private const float AtticHoleDepth = 0.5f;

        public abstract CompoundMeshDraft Construct(Vector2 parentLayoutOrigin);
        public Vector2 origin { get; set; }
        public float width { get; set; }
        public float height { get; set; }

        protected static MeshDraft Wall(Vector3 origin, float width, float height, Color wallColor)
        {
            return new MeshDraft {name = WallDraftName}
                .AddQuad(origin, Vector3.right * width, Vector3.up * height, true)
                .Paint(wallColor);
        }

        protected static MeshDraft PerforatedQuad(
            Vector3 min,
            Vector3 max,
            Vector3 innerMin,
            Vector3 innerMax)
        {
            var size = max - min;
            var widthVector = size.ToVector3XZ();
            var heightVector = size.ToVector3Y();
            var normal = Vector3.Cross(heightVector, widthVector).normalized;

            var innerSize = innerMax - innerMin;
            var innerHeight = innerSize.ToVector3Y();
            var innerWidth = innerSize.ToVector3XZ();

            var vertex0 = min;
            var vertex1 = min + heightVector;
            var vertex2 = max;
            var vertex3 = min + widthVector;
            var window0 = innerMin;
            var window1 = innerMin + innerHeight;
            var window2 = innerMax;
            var window3 = innerMin + innerWidth;

            return new MeshDraft
            {
                vertices = new List<Vector3>(8)
                {
                    vertex0,
                    vertex1,
                    vertex2,
                    vertex3,
                    window0,
                    window1,
                    window2,
                    window3
                },
                normals = new List<Vector3>(8) {normal, normal, normal, normal, normal, normal, normal, normal},
                triangles = new List<int>(24) {0, 1, 4, 4, 1, 5, 1, 2, 5, 5, 2, 6, 2, 3, 6, 6, 3, 7, 3, 0, 7, 7, 0, 4}
            };
        }

        protected static CompoundMeshDraft Window(
            Vector3 min,
            float width,
            float height,
            float widthOffset,
            float bottomOffset,
            float topOffset,
            Color wallColor,
            Color frameColor,
            Color glassColor,
            bool hasWindowsill = false)
        {
            var widthVector = Vector3.right * width;
            var heightVector = Vector3.up * height;
            var max = min + widthVector + heightVector;
            var frameMin = min + Vector3.right * widthOffset + Vector3.up * bottomOffset;
            var frameMax = max - Vector3.right * widthOffset - Vector3.up * topOffset;
            var frameWidth = Vector3.right * (width - widthOffset * 2);
            var frameHeight = Vector3.up * (height - bottomOffset - topOffset);
            var frameDepth = Vector3.forward * WindowDepth;
            var frameSize = frameMax - frameMin;

            var frame = MeshDraft
                .PartialBox(frameWidth, frameDepth, frameHeight, Directions.All & ~Directions.ZAxis, false)
                .FlipFaces()
                .Move(frameMin + frameSize / 2 + frameDepth / 2);

            var wall = PerforatedQuad(min, max, frameMin, frameMax)
                .Add(frame)
                .Paint(wallColor);
            wall.name = WallDraftName;

            var windowpane = Windowpane(frameMin + frameDepth, frameMax + frameDepth, frameColor, glassColor);

            var compoundDraft = new CompoundMeshDraft().Add(wall).Add(windowpane);

            if (hasWindowsill)
            {
                var windowsillWidth = frameWidth + Vector3.right * WindowsillWidthOffset;
                var windowsillDepth = Vector3.forward * WindowsillDepth;
                var windowsillHeight = Vector3.up * WindowsillThickness;
                var windowsill = MeshDraft.PartialBox(windowsillWidth, windowsillDepth, windowsillHeight,
                        Directions.All & ~Directions.Forward, false)
                    .Move(frameMin + frameWidth / 2 + frameDepth - windowsillDepth / 2)
                    .Paint(frameColor);
                windowsill.name = WallDraftName;
                compoundDraft.Add(windowsill);
            }

            return compoundDraft;
        }

        private static CompoundMeshDraft Windowpane(Vector3 min, Vector3 max, Color frameColor, Color glassColor)
        {
            var frame = WindowpaneFrame(min, max, frameColor, out var frameDepth, out var windowMin,
                out var windowWidth, out var windowHeight);
            var glass = WindowpaneGlass(frameDepth, windowMin, windowWidth, windowHeight, glassColor);
            return new CompoundMeshDraft().Add(frame).Add(glass);
        }

        private static MeshDraft WindowpaneFrame(Vector3 min, Vector3 max, Color frameColor,
            out Vector3 frameDepth, out Vector3 windowMin, out Vector3 windowWidth, out Vector3 windowHeight)
        {
            var size = max - min;
            var widthVector = size.ToVector3XZ();
            var heightVector = size.ToVector3Y();
            var frame = WindowpaneFrameRods(min, widthVector, heightVector, out var frameWidth, out var frameHeight,
                out frameDepth, out var startPosition);

            windowMin = min + frameWidth + frameHeight;
            windowWidth = widthVector - frameWidth * 2;
            windowHeight = heightVector - frameHeight * 2;
            var windowMax = windowMin + windowWidth + windowHeight;

            frame.Add(WindowpaneOuterFrame(min, max, widthVector, frameDepth, startPosition, windowMin, windowWidth,
                windowHeight, windowMax));
            frame.Paint(frameColor);
            return frame;
        }

        private static MeshDraft WindowpaneFrameRods(Vector3 min, Vector3 widthVector, Vector3 heightVector,
            out Vector3 frameWidth, out Vector3 frameHeight, out Vector3 frameDepth, out Vector3 startPosition)
        {
            var frame = new MeshDraft {name = WallDraftName};

            var right = widthVector.normalized;
            var normal = Vector3.Cross(heightVector, right).normalized;

            var width = widthVector.magnitude;
            var rodCount = Mathf.FloorToInt(width / WindowSegmentMinWidth);
            var interval = width / (rodCount + 1);

            frameWidth = right * WindowFrameWidth / 2;
            frameHeight = Vector3.up * WindowFrameWidth / 2;
            frameDepth = -normal * WindowFrameWidth / 2;
            startPosition = min + heightVector / 2 + frameDepth / 2;
            for (var i = 0; i < rodCount; i++)
            {
                var rod = MeshDraft.PartialBox(frameWidth * 2, frameDepth, heightVector - frameHeight * 2,
                        Directions.Left | Directions.Back | Directions.Right, false)
                    .Move(startPosition + right * (i + 1) * interval);
                frame.Add(rod);
            }

            return frame;
        }

        private static MeshDraft WindowpaneOuterFrame(Vector3 min, Vector3 max, Vector3 widthVector, Vector3 frameDepth,
            Vector3 startPosition,
            Vector3 windowMin, Vector3 windowWidth, Vector3 windowHeight, Vector3 windowMax)
        {
            var outerFrame = new MeshDraft();
            outerFrame.Add(PerforatedQuad(min, max, windowMin, windowMax));
            var box = MeshDraft.PartialBox(windowWidth, frameDepth, windowHeight, Directions.All & ~Directions.ZAxis,
                    false)
                .FlipFaces()
                .Move(startPosition + widthVector / 2);
            outerFrame.Add(box);
            return outerFrame;
        }

        private static MeshDraft WindowpaneGlass(Vector3 frameDepth, Vector3 windowMin, Vector3 windowWidth,
            Vector3 windowHeight, Color glassColor)
        {
            return new MeshDraft {name = GlassDraftName}
                .AddQuad(windowMin + frameDepth, windowWidth, windowHeight, true)
                .Paint(glassColor);
        }

        protected static CompoundMeshDraft Balcony(
            Vector3 origin,
            float width,
            float height,
            Color wallColor,
            Color frameColor,
            Color glassColor)
        {
            var widthVector = Vector3.right * width;
            var heightVector = Vector3.up * height;

            var compoundDraft = new CompoundMeshDraft();
            compoundDraft.Add(Balcony(origin, width, wallColor));

            var innerHeightOffset = Vector3.up * BalconyThickness;

            var windowHeightOffset = Vector3.up * WindowBottomOffset;
            var windowWidth = Vector3.right * (width - WindowWidthOffset * 2);
            var windowHeight = Vector3.up * (height - WindowBottomOffset - WindowTopOffset);
            var windowDepth = Vector3.forward * WindowDepth;

            var rodCount = Mathf.FloorToInt(windowWidth.magnitude / WindowSegmentMinWidth);
            var doorWidth = Vector3.right * windowWidth.magnitude / (rodCount + 1);
            var doorHeight = windowHeightOffset + windowHeight;

            var outerFrameOrigin = origin + Vector3.right * WindowWidthOffset + innerHeightOffset;
            var outerFrame = new List<Vector3>
            {
                outerFrameOrigin,
                outerFrameOrigin + doorWidth,
                outerFrameOrigin + doorWidth + windowHeightOffset,
                outerFrameOrigin + windowWidth + windowHeightOffset,
                outerFrameOrigin + windowWidth + doorHeight,
                outerFrameOrigin + doorHeight
            };

            compoundDraft.Add(BalconyWallPanel(origin, widthVector, heightVector, windowDepth, outerFrame, wallColor));

            var windowpaneMin1 = outerFrame[0] + windowDepth;
            var windowpaneMin2 = outerFrame[2] + windowDepth;
            compoundDraft.Add(Windowpane(windowpaneMin1, windowpaneMin1 + doorWidth + doorHeight, frameColor,
                glassColor));
            compoundDraft.Add(Windowpane(windowpaneMin2, windowpaneMin2 + windowWidth - doorWidth + windowHeight,
                frameColor, glassColor));

            return compoundDraft;
        }

        private static MeshDraft Balcony(Vector3 origin, float width, Color wallColor)
        {
            var widthVector = Vector3.right * width;
            var balconyHeight = Vector3.up * BalconyHeight;
            var balconyDepth = Vector3.forward * BalconyDepth;

            var balconyOuter = BalconyOuter(origin, widthVector, balconyHeight, balconyDepth, out var balconyCenter);
            var balconyInner = BalconyInner(widthVector, balconyHeight, balconyDepth, balconyCenter,
                out var innerWidthOffset, out var innerWidth, out var innerDepth);

            var balconyBorder = BalconyBorder(origin, widthVector, balconyHeight, balconyDepth, innerWidthOffset,
                innerWidth, innerDepth);

            return new MeshDraft {name = WallDraftName}
                .Add(balconyOuter)
                .Add(balconyInner)
                .Add(balconyBorder)
                .Paint(wallColor);
        }

        private static MeshDraft BalconyOuter(Vector3 origin, Vector3 widthVector, Vector3 balconyHeight,
            Vector3 balconyDepth,
            out Vector3 balconyCenter)
        {
            balconyCenter = origin + widthVector / 2 - balconyDepth / 2 + balconyHeight / 2;
            return MeshDraft.PartialBox(widthVector, balconyDepth, balconyHeight,
                    Directions.All & ~Directions.Up & ~Directions.Forward, false)
                .Move(balconyCenter);
        }

        private static MeshDraft BalconyInner(Vector3 widthVector, Vector3 balconyHeight, Vector3 balconyDepth,
            Vector3 balconyCenter,
            out Vector3 innerWidthOffset, out Vector3 innerWidth, out Vector3 innerDepth)
        {
            innerWidthOffset = Vector3.right * BalconyThickness;
            innerWidth = widthVector - innerWidthOffset * 2;
            var innerHeightOffset = Vector3.up * BalconyThickness;
            var innerHeight = balconyHeight - innerHeightOffset;
            var innerDepthOffset = Vector3.forward * BalconyThickness;
            innerDepth = balconyDepth - innerDepthOffset;
            return MeshDraft.PartialBox(innerWidth, innerDepth, innerHeight,
                    Directions.All & ~Directions.Up & ~Directions.Forward, false)
                .FlipFaces()
                .Move(balconyCenter + innerDepthOffset / 2 + innerHeightOffset / 2);
        }

        private static MeshDraft BalconyBorder(Vector3 origin, Vector3 widthVector, Vector3 balconyHeight,
            Vector3 balconyDepth,
            Vector3 innerWidthOffset, Vector3 innerWidth, Vector3 innerDepth)
        {
            var borderOrigin = origin + balconyHeight;
            var borderInnerOrigin = borderOrigin + innerWidthOffset;
            return new MeshDraft().AddTriangleStrip(new List<Vector3>
            {
                borderOrigin,
                borderInnerOrigin,
                borderOrigin - balconyDepth,
                borderInnerOrigin - innerDepth,
                borderOrigin - balconyDepth + widthVector,
                borderInnerOrigin - innerDepth + innerWidth,
                borderOrigin + widthVector,
                borderInnerOrigin + innerWidth
            });
        }

        private static MeshDraft BalconyWallPanel(Vector3 origin, Vector3 widthVector, Vector3 heightVector,
            Vector3 windowDepth,
            List<Vector3> outerFrame, Color wallColor)
        {
            var wall = new MeshDraft {name = WallDraftName}
                .AddTriangleStrip(new List<Vector3>
                {
                    outerFrame[0],
                    origin,
                    outerFrame[5],
                    origin + heightVector,
                    outerFrame[4],
                    origin + widthVector + heightVector,
                    outerFrame[3],
                    origin + widthVector,
                    outerFrame[2],
                    origin + widthVector,
                    outerFrame[1]
                });

            var innerFrame = new List<Vector3>();
            foreach (var vertex in outerFrame) innerFrame.Add(vertex + windowDepth);
            wall.AddFlatQuadBand(innerFrame, outerFrame, false);
            wall.Paint(wallColor);
            return wall;
        }

        protected static CompoundMeshDraft BalconyGlazed(Vector3 origin, float width, float height, Color wallColor,
            Color frameColor, Color glassColor, Color roofColor)
        {
            var widthVector = Vector3.right * width;
            var heightVector = Vector3.up * height;

            var balconyHeight = Vector3.up * BalconyHeight;
            var balconyDepth = Vector3.forward * BalconyDepth;

            var compoundDraft = new CompoundMeshDraft();

            var balcony = BalconyOuter(origin, widthVector, balconyHeight, balconyDepth, out var balconyCenter)
                .Paint(wallColor);
            balcony.name = WallDraftName;
            compoundDraft.Add(balcony);

            var roof = BalconyGlazedRoof(origin, widthVector, heightVector, balconyDepth, roofColor);
            compoundDraft.Add(roof);

            var glassHeight = new Vector3(0, height - BalconyHeight, 0);
            var glass0 = origin + balconyHeight;
            var glass1 = glass0 - balconyDepth;
            var glass2 = glass1 + widthVector;
            compoundDraft.Add(Windowpane(glass0, glass1 + glassHeight, frameColor, glassColor));
            compoundDraft.Add(Windowpane(glass1, glass2 + glassHeight, frameColor, glassColor));
            compoundDraft.Add(Windowpane(glass2, glass2 + balconyDepth + glassHeight, frameColor, glassColor));

            return compoundDraft;
        }

        private static MeshDraft BalconyGlazedRoof(Vector3 origin, Vector3 widthVector, Vector3 heightVector,
            Vector3 balconyDepth, Color roofColor)
        {
            var roof0 = origin + heightVector;
            var roof1 = roof0 + widthVector;
            var roof2 = roof1 - balconyDepth;
            var roof3 = roof0 - balconyDepth;
            var roof = new MeshDraft {name = WallDraftName}
                .AddQuad(roof0, roof1, roof2, roof3, Vector3.up)
                .Paint(roofColor);
            return roof;
        }

        protected static MeshDraft Entrance(Vector3 origin, float width, float height, Color wallColor, Color doorColor)
        {
            var widthVector = Vector3.right * width;
            var heightVector = Vector3.up * height;

            var doorWidth = Vector3.right * EntranceDoorWidth;
            var doorHeight = Vector3.up * EntranceDoorHeight;
            var doorThickness = Vector3.back * EntranceDoorThickness;
            var doorOrigin = origin + widthVector / 2 - doorWidth / 2;

            var draft = EntranceBracket(origin, widthVector, heightVector, doorOrigin, doorWidth, doorHeight)
                .Paint(wallColor);

            var doorFrame = MeshDraft
                .PartialBox(doorWidth, -doorThickness, doorHeight, Directions.All & ~Directions.ZAxis, false)
                .Move(doorOrigin + doorWidth / 2 + doorHeight / 2 + doorThickness / 2)
                .Paint(doorColor);
            draft.Add(doorFrame);

            var door = new MeshDraft().AddQuad(doorOrigin + doorThickness, doorWidth, doorHeight, true)
                .Paint(doorColor);
            draft.Add(door);
            return draft;
        }

        private static MeshDraft EntranceBracket(Vector3 origin, Vector3 width, Vector3 depth,
            Vector3 innerOrigin, Vector3 innerWidth, Vector3 innerDepth)
        {
            return new MeshDraft().AddTriangleStrip(new List<Vector3>
            {
                innerOrigin,
                origin,
                innerOrigin + innerDepth,
                origin + depth,
                innerOrigin + innerDepth + innerWidth,
                origin + depth + width,
                innerOrigin + innerWidth,
                origin + width
            });
        }

        protected static MeshDraft EntranceRoofed(Vector3 origin, float width, float height, Color wallColor,
            Color doorColor, Color roofColor)
        {
            var draft = Entrance(origin, width, height, wallColor, doorColor);
            var widthVector = Vector3.right * width;
            var depthVector = Vector3.forward * EntranceRoofDepth;

            var roof = MeshDraft.PartialBox(widthVector, depthVector, Vector3.up * EntranceRoofHeight,
                    Directions.All & ~Directions.Forward, false)
                .Move(origin + widthVector / 2 + Vector3.up * (height - EntranceRoofHeight / 2) - depthVector / 2)
                .Paint(roofColor);
            draft.Add(roof);
            return draft;
        }

        protected static CompoundMeshDraft EntranceWindow(Vector3 origin, float width, float height, Color wallColor,
            Color frameColor, Color glassColor)
        {
            var compoundDraft = Window(origin, width, height / 2, EntranceWindowWidthOffset, EntranceWindowHeightOffset,
                EntranceWindowHeightOffset, wallColor, frameColor, glassColor);
            compoundDraft.Add(Window(origin + Vector3.up * height / 2, width, height / 2, EntranceWindowWidthOffset,
                EntranceWindowHeightOffset, EntranceWindowHeightOffset, wallColor, frameColor, glassColor));
            return compoundDraft;
        }

        protected static CompoundMeshDraft SocleWindowed(Vector3 origin, float width, float height, Color wallColor,
            Color glassColor)
        {
            if (width < SocleWindowMinWidth) return new CompoundMeshDraft().Add(Wall(origin, width, height, wallColor));

            var widthVector = Vector3.right * width;
            var heightVector = Vector3.up * height;

            var windowWidth = Vector3.right * SocleWindowWidth;
            var windowHeigth = Vector3.up * SocleWindowHeight;
            var windowDepth = Vector3.forward * SocleWindowDepth;
            var windowOrigin = origin + widthVector / 2 - windowWidth / 2 + Vector3.up * SocleWindowHeightOffset;
            var windowMax = windowOrigin + windowWidth + windowHeigth;

            var frame = MeshDraft.PartialBox(windowWidth, -windowDepth, windowHeigth,
                    Directions.All & ~Directions.ZAxis, false)
                .Move(windowOrigin + windowWidth / 2 + windowHeigth / 2 + windowDepth / 2);

            var wall = PerforatedQuad(origin, origin + widthVector + heightVector, windowOrigin, windowMax)
                .Add(frame)
                .Paint(wallColor);
            wall.name = WallDraftName;

            var glass = new MeshDraft()
                .AddQuad(windowOrigin + windowDepth / 2, windowWidth, windowHeigth, true)
                .Paint(glassColor);
            glass.name = GlassDraftName;

            return new CompoundMeshDraft().Add(wall).Add(glass);
        }

        protected static CompoundMeshDraft AtticVented(Vector3 origin, float width, float height, Color wallColor,
            Color holeColor)
        {
            if (width < AtticHoleMinWidth) return new CompoundMeshDraft().Add(Wall(origin, width, height, wallColor));

            var widthVector = Vector3.right * width;
            var heightVector = Vector3.up * height;
            var center = origin + widthVector / 2 + heightVector / 2;

            var holeOrigin = center - Vector3.right * AtticHoleWidth / 2 - Vector3.up * AtticHoleHeight / 2;
            var holeWidth = Vector3.right * AtticHoleWidth;
            var holeHeight = Vector3.up * AtticHoleHeight;
            var holeDepth = Vector3.forward * AtticHoleDepth;

            var wall = PerforatedQuad(origin, origin + widthVector + heightVector, holeOrigin,
                    holeOrigin + holeWidth + holeHeight)
                .Paint(wallColor);

            var hole = MeshDraft.PartialBox(holeWidth, holeDepth, holeHeight, Directions.All & ~Directions.Back, false)
                .Move(center + holeDepth / 2)
                .FlipFaces()
                .Paint(holeColor);
            wall.Add(hole);
            wall.name = WallDraftName;
            return new CompoundMeshDraft().Add(wall);
        }
    }

    public class ProceduralWall : ProceduralFacadeElement
    {
        private readonly Color wallColor;

        public ProceduralWall(Color wallColor)
        {
            this.wallColor = wallColor;
        }

        public override CompoundMeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            return new CompoundMeshDraft().Add(Wall(parentLayoutOrigin + origin, width, height, wallColor));
        }
    }

    public class ProceduralWindow : ProceduralFacadeElement
    {
        private readonly Color frameColor;
        private readonly Color glassColor;
        private readonly Color wallColor;

        public ProceduralWindow(Color wallColor, Color frameColor, Color glassColor)
        {
            this.wallColor = wallColor;
            this.frameColor = frameColor;
            this.glassColor = glassColor;
        }

        public override CompoundMeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            return Window(parentLayoutOrigin + origin, width, height, WindowWidthOffset, WindowBottomOffset,
                WindowTopOffset,
                wallColor, frameColor, glassColor, true);
        }
    }

    public class ProceduralBalcony : ProceduralFacadeElement
    {
        private readonly Color frameColor;
        private readonly Color glassColor;
        private readonly Color wallColor;

        public ProceduralBalcony(Color wallColor, Color frameColor, Color glassColor)
        {
            this.wallColor = wallColor;
            this.frameColor = frameColor;
            this.glassColor = glassColor;
        }

        public override CompoundMeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            return Balcony(parentLayoutOrigin + origin, width, height, wallColor, frameColor, glassColor);
        }
    }

    public class ProceduralBalconyGlazed : ProceduralFacadeElement
    {
        private readonly Color frameColor;
        private readonly Color glassColor;
        private readonly Color roofColor;
        private readonly Color wallColor;

        public ProceduralBalconyGlazed(Color wallColor, Color frameColor, Color glassColor, Color roofColor)
        {
            this.wallColor = wallColor;
            this.frameColor = frameColor;
            this.glassColor = glassColor;
            this.roofColor = roofColor;
        }

        public override CompoundMeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            return BalconyGlazed(parentLayoutOrigin + origin, width, height, wallColor, frameColor, glassColor,
                roofColor);
        }
    }

    public class ProceduralEntrance : ProceduralFacadeElement
    {
        private readonly Color doorColor;
        private readonly Color wallColor;

        public ProceduralEntrance(Color wallColor, Color doorColor)
        {
            this.wallColor = wallColor;
            this.doorColor = doorColor;
        }

        public override CompoundMeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            var entranceDraft = Entrance(parentLayoutOrigin + origin, width, height, wallColor, doorColor);
            entranceDraft.name = WallDraftName;
            return new CompoundMeshDraft().Add(entranceDraft);
        }
    }

    public class ProceduralEntranceRoofed : ProceduralFacadeElement
    {
        private readonly Color doorColor;
        private readonly Color roofColor;
        private readonly Color wallColor;

        public ProceduralEntranceRoofed(Color wallColor, Color doorColor, Color roofColor)
        {
            this.wallColor = wallColor;
            this.doorColor = doorColor;
            this.roofColor = roofColor;
        }

        public override CompoundMeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            var entranceDraft =
                EntranceRoofed(parentLayoutOrigin + origin, width, height, wallColor, doorColor, roofColor);
            entranceDraft.name = WallDraftName;
            return new CompoundMeshDraft().Add(entranceDraft);
        }
    }

    public class ProceduralEntranceWindow : ProceduralFacadeElement
    {
        private readonly Color frameColor;
        private readonly Color glassColor;
        private readonly Color wallColor;

        public ProceduralEntranceWindow(Color wallColor, Color frameColor, Color glassColor)
        {
            this.wallColor = wallColor;
            this.frameColor = frameColor;
            this.glassColor = glassColor;
        }

        public override CompoundMeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            return EntranceWindow(parentLayoutOrigin + origin, width, height, wallColor, frameColor, glassColor);
        }
    }

    public class ProceduralSocle : ProceduralWall
    {
        public ProceduralSocle(Color wallColor) : base(wallColor)
        {
        }
    }

    public class ProceduralSocleWindowed : ProceduralFacadeElement
    {
        private readonly Color glassColor;
        private readonly Color wallColor;

        public ProceduralSocleWindowed(Color wallColor, Color glassColor)
        {
            this.wallColor = wallColor;
            this.glassColor = glassColor;
        }

        public override CompoundMeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            return SocleWindowed(parentLayoutOrigin + origin, width, height, wallColor, glassColor);
        }
    }

    public class ProceduralAtticVented : ProceduralFacadeElement
    {
        private readonly Color holeColor;
        private readonly Color wallColor;

        public ProceduralAtticVented(Color wallColor, Color holeColor)
        {
            this.wallColor = wallColor;
            this.holeColor = holeColor;
        }

        public override CompoundMeshDraft Construct(Vector2 parentLayoutOrigin)
        {
            return AtticVented(parentLayoutOrigin + origin, width, height, wallColor, holeColor);
        }
    }
}