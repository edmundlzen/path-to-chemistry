using UnityEngine;

namespace ProceduralToolkit.Samples
{
    public static class Armrests
    {
        public static MeshDraft Armrests0(float seatWidth, float seatDepth, Vector3 backCenter, float backHeight,
            float legWidth)
        {
            var draft = new MeshDraft();
            var armrestHeight = RandomE.Range(backHeight / 4, backHeight * 3 / 4, 3);
            var armrestLength = seatDepth - legWidth;

            var corner = backCenter + Vector3.left * (seatWidth / 2 - legWidth / 2) + Vector3.back * legWidth / 2;

            float offset = 0;
            if (RandomE.Chance(0.5f)) offset = RandomE.Range(legWidth / 2, legWidth, 2);
            var v0 = corner + Vector3.back * (armrestLength - legWidth / 2);
            var v1 = v0 + Vector3.up * (armrestHeight - legWidth / 2);
            var v2 = corner + Vector3.up * armrestHeight;
            var v3 = v2 + Vector3.back * (armrestLength + offset);

            var armrest = ChairGenerator.BeamDraft(v0, v1, legWidth);
            armrest.Add(ChairGenerator.BeamDraft(v2, v3, legWidth));
            draft.Add(armrest);
            armrest.Move(Vector3.right * (seatWidth - legWidth));
            draft.Add(armrest);
            return draft;
        }

        public static MeshDraft Armrests1(float seatWidth, float seatDepth, Vector3 backCenter, float backHeight,
            float legWidth)
        {
            var draft = new MeshDraft();
            var armrestHeight = RandomE.Range(backHeight / 4, backHeight * 3 / 4, 3);
            var armrestLength = RandomE.Range(seatDepth * 3 / 4, seatDepth, 2);
            legWidth = RandomE.Range(legWidth * 3 / 4, legWidth, 2);

            var corner = backCenter + Vector3.left * (seatWidth / 2 + legWidth / 2) +
                         Vector3.forward * legWidth / 2;

            float offset = 0;
            if (RandomE.Chance(0.5f)) offset = RandomE.Range(armrestLength / 4, armrestLength / 2, 2) - legWidth / 2;
            var v0 = corner + Vector3.back * (armrestLength - legWidth / 2 - offset) + Vector3.down * legWidth;
            var v1 = v0 + Vector3.up * (armrestHeight + legWidth / 2);
            var v2 = corner + Vector3.up * armrestHeight;
            var v3 = v2 + Vector3.back * armrestLength;

            var armrest = ChairGenerator.BeamDraft(v0, v1, legWidth);
            armrest.Add(ChairGenerator.BeamDraft(v2, v3, legWidth));
            draft.Add(armrest);
            armrest.Move(Vector3.right * (seatWidth + legWidth));
            draft.Add(armrest);
            return draft;
        }
    }
}