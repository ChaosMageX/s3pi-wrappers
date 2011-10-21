using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3piwrappers.Granny2;

namespace s3piwrappers
{
    public static class RigConversion
    {
        public static EAxoidRig GrannyToEAxoid(AbstractRig grRig, RigType grType)
        {
            int i, j;
            if (grRig == null)
            {
                return null;
            }
            WrappedGrannyData grData = grRig.GrannyData as WrappedGrannyData;
            if (grData == null)
            {
                // Missing granny2.dll
                return null;
            }
            Skeleton grSkeleton = grData.FileInfo.Skeleton;
            BoneList grBones = grSkeleton.Bones;
            Bone grBone;

            EAxoidRig eaRig = new EAxoidRig(0, null);
            EAxoidRig.Bone eaBone;
            for (i = 0; i < grBones.Count; i++)
            {
                grBone = grBones[i];
                eaBone = new EAxoidRig.Bone(0, null);
                eaBone.Position = grBone.LocalTransform.Position;
                eaBone.Orientation = grBone.LocalTransform.Orientation;
                eaBone.Scale = new Triple(0, null,
                    grBone.LocalTransform.ScaleShearX.X,
                    grBone.LocalTransform.ScaleShearY.Y,
                    grBone.LocalTransform.ScaleShearZ.Z);
                eaBone.Name = grBone.Name;
                eaBone.MirrorIndex = i;
                eaBone.ParentIndex = grBone.ParentIndex;
                eaBone.Int02 = 0x23;
                eaRig.Bones.Add(item: eaBone);
            }
            eaRig.Name = grSkeleton.Name;

            if (grType == RigType.Body)
            {
                BodyRig.IkChainList grIKChains = (grRig as BodyRig).IkChains;
                BodyRig.IkChain grIKChain;

                int lastInfo;
                EAxoidRig.IKChain eaIKChain;
                for (i = 0; i < grIKChains.Count; i++)
                {
                    grIKChain = grIKChains[i];
                    eaIKChain = new EAxoidRig.IKChain(0, null);
                    for (j = 0; j < grIKChain.Links.Count; j++)
                    {
                        eaIKChain.IkLinks.Add(item: grIKChain.Links[j].End);
                    }

                    lastInfo = -1;
                    for (j = 0; j < 10; j++)
                    {
                        if (grIKChain.InfoNodes[j].Enabled)
                            lastInfo = j;
                        eaIKChain.SetInfoNode(j, grIKChain.InfoNodes[j].Index);
                        //eaIKChain.SetInfoNode(j, lastInfo == j ? grIKChain.InfoNodes[j].Index : -1);
                    }
                    if (lastInfo == 9)
                    {
                        eaIKChain.SetInfoNode(10, grIKChain.InfoRoot);
                    }
                    else
                    {
                        eaIKChain.SetInfoNode(10, -1);
                        if (lastInfo == -1 || eaIKChain.GetInfoNode(lastInfo) != grIKChain.InfoRoot)
                        {
                            //TODO: Set Debugging message of erroneous mismatch of node indices
                        }
                    }

                    eaIKChain.IkPole = grIKChain.IkPole;
                    eaIKChain.SlotInfo = grIKChain.SlotInfo;
                    eaIKChain.SlotOffset = grIKChain.SlotOffset;
                    eaIKChain.Root = grIKChain.Root;

                    eaRig.IKChains.Add(item: eaIKChain);
                }
            }
            return eaRig;
        }

        public static Matrix4x4 Inverse(Transform transform)
        {
            float a41 = transform.Orientation.X;
            float a42 = transform.Orientation.Y;
            float a43 = transform.Orientation.Z;
            float a44 = transform.Orientation.W;
            // Orientation Quaternion to Matrix3x3 Conversion
            float a11 = (((a44 * a44) + (a41 * a41)) - (a42 * a42)) - (a43 * a43);
            float a12 = ((2f * a41) * a42) - ((2f * a43) * a44);
            float a13 = ((2f * a41) * a43) + ((2f * a42) * a44);
            float a21 = ((2f * a41) * a42) + ((2f * a43) * a44);
            float a22 = (((a44 * a44) - (a41 * a41)) + (a42 * a42)) - (a43 * a43);
            float a23 = ((2f * a42) * a43) - ((2f * a41) * a44);
            float a31 = ((2f * a43) * a41) - ((2f * a42) * a44);
            float a32 = ((2f * a43) * a42) + ((2f * a41) * a44);
            float a33 = (((a44 * a44) - (a41 * a41)) - (a42 * a42)) + (a43 * a43);
            // Orientation * ScaleShear
            Matrix4x4 result = new Matrix4x4(0, null);
            result.M0.X = a11 * transform.ScaleShearX.X + a12 * transform.ScaleShearY.X + a13 * transform.ScaleShearZ.X;
            result.M0.Y = a11 * transform.ScaleShearX.Y + a12 * transform.ScaleShearY.Y + a13 * transform.ScaleShearZ.Y;
            result.M0.Z = a11 * transform.ScaleShearX.Z + a12 * transform.ScaleShearY.Z + a13 * transform.ScaleShearZ.Z;
            result.M1.X = a21 * transform.ScaleShearX.X + a22 * transform.ScaleShearY.X + a23 * transform.ScaleShearZ.X;
            result.M1.Y = a21 * transform.ScaleShearX.Y + a22 * transform.ScaleShearY.Y + a23 * transform.ScaleShearZ.Y;
            result.M1.Z = a21 * transform.ScaleShearX.Z + a22 * transform.ScaleShearY.Z + a23 * transform.ScaleShearZ.Z;
            result.M2.X = a31 * transform.ScaleShearX.X + a32 * transform.ScaleShearY.X + a33 * transform.ScaleShearZ.X;
            result.M2.Y = a31 * transform.ScaleShearX.Y + a32 * transform.ScaleShearY.Y + a33 * transform.ScaleShearZ.Y;
            result.M2.Z = a31 * transform.ScaleShearX.Z + a32 * transform.ScaleShearY.Z + a33 * transform.ScaleShearZ.Z;
            result.M3.X = transform.Position.X;
            result.M3.Y = transform.Position.Y;
            result.M3.Z = transform.Position.Z;
            // Inversion
            Matrix4x4 output = new Matrix4x4(0, null);
            a11 = (result.M2.Z * result.M3.W) - (result.M2.W * result.M3.Z);
            a12 = (result.M2.Y * result.M3.W) - (result.M2.W * result.M3.Y);
            a13 = (result.M2.Y * result.M3.Z) - (result.M2.Z * result.M3.Y);
            float a14 = (result.M2.X * result.M3.W) - (result.M2.W * result.M3.X);
            a21 = (result.M2.X * result.M3.Z) - (result.M2.Z * result.M3.X);
            a22 = (result.M2.X * result.M3.Y) - (result.M2.Y * result.M3.X);
            a23 = ((result.M1.Y * a11) - (result.M1.Z * a12)) + (result.M1.W * a13);
            float a24 = -(((result.M1.X * a11) - (result.M1.Z * a14)) + (result.M1.W * a21));
            a31 = ((result.M1.X * a12) - (result.M1.Y * a14)) + (result.M1.W * a22);
            a32 = -(((result.M1.X * a13) - (result.M1.Y * a21)) + (result.M1.Z * a22));
            a33 = 1f / ((((result.M0.X * a23) + (result.M0.Y * a24)) + (result.M0.Z * a31)) + (result.M0.W * a32));
            output.M0.X = a23 * a33;
            output.M1.X = a24 * a33;
            output.M2.X = a31 * a33;
            output.M3.X = a32 * a33;
            output.M0.Y = -(((result.M0.Y * a11) - (result.M0.Z * a12)) + (result.M0.W * a13)) * a33;
            output.M1.Y = (((result.M0.X * a11) - (result.M0.Z * a14)) + (result.M0.W * a21)) * a33;
            output.M2.Y = -(((result.M0.X * a12) - (result.M0.Y * a14)) + (result.M0.W * a22)) * a33;
            output.M3.Y = (((result.M0.X * a13) - (result.M0.Y * a21)) + (result.M0.Z * a22)) * a33;
            a11 = (result.M1.Z * result.M3.W) - (result.M1.W * result.M3.Z);
            a12 = (result.M1.Y * result.M3.W) - (result.M1.W * result.M3.Y);
            a13 = (result.M1.Y * result.M3.Z) - (result.M1.Z * result.M3.Y);
            a14 = (result.M1.X * result.M3.W) - (result.M1.W * result.M3.X);
            a21 = (result.M1.X * result.M3.Z) - (result.M1.Z * result.M3.X);
            a22 = (result.M1.X * result.M3.Y) - (result.M1.Y * result.M3.X);
            output.M0.Z = (((result.M0.Y * a11) - (result.M0.Z * a12)) + (result.M0.W * a13)) * a33;
            output.M1.Z = -(((result.M0.X * a11) - (result.M0.Z * a14)) + (result.M0.W * a21)) * a33;
            output.M2.Z = (((result.M0.X * a12) - (result.M0.Y * a14)) + (result.M0.W * a22)) * a33;
            output.M3.Z = -(((result.M0.X * a13) - (result.M0.Y * a21)) + (result.M0.Z * a22)) * a33;
            a11 = (result.M1.Z * result.M2.W) - (result.M1.W * result.M2.Z);
            a12 = (result.M1.Y * result.M2.W) - (result.M1.W * result.M2.Y);
            a13 = (result.M1.Y * result.M2.Z) - (result.M1.Z * result.M2.Y);
            a14 = (result.M1.X * result.M2.W) - (result.M1.W * result.M2.X);
            a21 = (result.M1.X * result.M2.Z) - (result.M1.Z * result.M2.X);
            a22 = (result.M1.X * result.M2.Y) - (result.M1.Y * result.M2.X);
            output.M0.W = -(((result.M0.Y * a11) - (result.M0.Z * a12)) + (result.M0.W * a13)) * a33;
            output.M1.W = (((result.M0.X * a11) - (result.M0.Z * a14)) + (result.M0.W * a21)) * a33;
            output.M2.W = -(((result.M0.X * a12) - (result.M0.Y * a14)) + (result.M0.W * a22)) * a33;
            output.M3.W = (((result.M0.X * a13) - (result.M0.Y * a21)) + (result.M0.Z * a22)) * a33;
            return output;
        }

        public static AbstractRig EAxoidToGranny(EAxoidRig eaRig, out RigType grType, string fileName = "")
        {
            int i, j;
            grType = RigType.Object;
            if (eaRig == null)
            {
                return null;
            }
            WrappedGrannyData grData = new WrappedGrannyData(0, null);
            BoneList grBones = grData.FileInfo.Skeleton.Bones;
            Bone grBone;

            EAxoidRig.BoneList eaBones = eaRig.Bones;
            EAxoidRig.Bone eaBone;
            for (i = 0; i < eaBones.Count; i++)
            {
                eaBone = eaBones[i];
                grBone = new Bone(0, null);
                grBone.Name = eaBone.Name;
                grBone.ParentIndex = eaBone.ParentIndex;
                grBone.LocalTransform.Position = eaBone.Position;
                grBone.LocalTransform.Orientation = eaBone.Orientation;
                grBone.LocalTransform.ScaleShearX = new Triple(0, null, eaBone.Scale.X, 0, 0);
                grBone.LocalTransform.ScaleShearY = new Triple(0, null, 0, eaBone.Scale.Y, 0);
                grBone.LocalTransform.ScaleShearZ = new Triple(0, null, 0, 0, eaBone.Scale.Z);
                grBone.InverseWorld4X4 = Inverse(grBone.LocalTransform);
                grBones.Add(item: grBone);
            }
            grData.FileInfo.FromFileName = fileName;
            grData.FileInfo.Model.Name = eaRig.Name;
            grData.FileInfo.Skeleton.Name = eaRig.Name;

            if (eaRig.IKChains.Count == 0)
            {
                return new ObjectRig(0, null, grData);
            }
            else
            {
                grType = RigType.Body;
                BodyRig grRig = new BodyRig(0, null, grData);

                EAxoidRig.IKChainList eaIKChains = eaRig.IKChains;
                EAxoidRig.IKChain eaIKChain;

                int lastInfo, count;
                uint unknown;
                BodyRig.IkChain grIKChain;
                for (i = 0; i < eaIKChains.Count; i++)
                {
                    eaIKChain = eaIKChains[i];
                    grIKChain = new BodyRig.IkChain(0, null);

                    //grIKChain.Unknown01 = 0;
                    /*grIKChain.Unknown02 = new byte[] { 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };/**/
                    grIKChain.IkPole = eaIKChain.IkPole;
                    grIKChain.IkPoleRoot = eaRig.GetBoneParent(eaIKChain.IkPole);
                    grIKChain.SlotInfo = eaIKChain.SlotInfo;
                    grIKChain.SlotInfoRoot = eaRig.GetBoneParent(eaIKChain.SlotInfo);
                    grIKChain.SlotOffset = eaIKChain.SlotOffset;
                    grIKChain.SlotOffsetRoot = eaRig.GetBoneParent(eaIKChain.SlotOffset);

                    lastInfo = -1;
                    for (j = 0; j < 10; j++)
                    {
                        if (eaIKChain.InfoNodes[j] != -1)
                        {
                            lastInfo = j;
                            grIKChain.InfoNodes[j].Enabled = true;
                        }
                        else
                        {
                            grIKChain.InfoNodes[j].Enabled = false;
                        }
                        grIKChain.InfoNodes[j].Index = eaIKChain.GetInfoNode(j);
                    }
                    if (lastInfo == -1)
                    {
                        grIKChain.InfoRoot = -1;
                    }
                    else
                    {
                        grIKChain.InfoRoot = eaIKChain.GetInfoNode(lastInfo);
                    }

                    grIKChain.Root = eaIKChain.Root;

                    count = eaIKChain.IkLinks.Count;
                    if (count == 1) 
                        unknown = 0x08;
                    else if (count == 3) 
                        unknown = 0x10;
                    else 
                        unknown = 0x00;
                    for (j = 0; j < count; j++)
                    {
                        lastInfo = eaIKChain.IkLinks[i];
                        grIKChain.Links.Add(item: new BodyRig.IkLink(0, null, unknown, lastInfo, eaRig.GetBoneParent(lastInfo)));
                        unknown += 8;
                    }

                    eaRig.IKChains.Add(item: eaIKChain);
                }
                return grRig;
            }
        }
    }
}
