using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Input;
using s3pi.Interfaces;
using s3piwrappers.RigEditor.Bones;
using s3piwrappers.RigEditor.Geometry;
using s3piwrappers.RigEditor.ViewModels;
using Quaternion = s3piwrappers.RigEditor.Geometry.Quaternion;

namespace s3piwrappers.RigEditor.Commands
{
    public class BoneOps
    {

        public static ICommand CloneHierarchyCommand { get; private set; }
        public static ICommand CloneCommand { get; private set; }
        public static ICommand AddCommand { get; private set; }
        public static ICommand DeleteHierarchyCommand { get; private set; }
        public static ICommand DeleteCommand { get; private set; }
        public static ICommand SetParentCommand { get; private set; }
        public static ICommand UnparentCommand { get; private set; }
        public static ICommand FindReplaceCommand { get; private set; }
        public static ICommand PrefixCommand { get; private set; }
        public static ICommand SuffixCommand { get; private set; }
        public static ICommand RotationMatrixInputCommand { get; private set; }

        static BoneOps()
        {
            DeleteCommand = new UserCommand<BoneViewModel>(IsABone, ExecuteDeleteBone);
            DeleteHierarchyCommand = new UserCommand<BoneViewModel>(IsAParent, ExecuteDeleteHierarchy);
            AddCommand = new UserCommand<BoneViewModel>(IsABone, ExecuteAddBone);
            CloneCommand = new UserCommand<BoneViewModel>(IsABone, ExecuteClone);
            CloneHierarchyCommand = new UserCommand<BoneViewModel>(IsAParent, ExecuteCloneHierarchy);
            SetParentCommand = new UserCommand<BoneViewModel>(HasNonDescendants, ExecuteSetParent);
            UnparentCommand = new UserCommand<BoneViewModel>(IsAChild, ExecuteUnparent);
            FindReplaceCommand = new UserCommand<BoneViewModel>(IsAParent, ExecuteFindReplace);
            PrefixCommand = new UserCommand<BoneViewModel>(IsAParent, ExecutePrefix);
            SuffixCommand = new UserCommand<BoneViewModel>(IsAParent, ExecuteSuffix);
            RotationMatrixInputCommand = new UserCommand<BoneViewModel>(IsABone, ExecuteRotationMatrixInput);

        }
        private static bool IsABone(BoneViewModel bone)
        {
            return bone != null;
        }
        private static bool IsAChild(BoneViewModel bone)
        {
            return IsABone(bone) && bone.Parent is BoneViewModel;
        }
        private static bool IsAParent(BoneViewModel bone)
        {
            return IsABone(bone) && bone.Children.Count > 0;
        }
        private static bool HasNonDescendants(BoneViewModel bone)
        {
            if(IsABone(bone))
            {
                var descendants = bone.Manager.GetDescendants(bone.Bone);
                return bone.Manager.Bones.Any(x => x!=bone.Bone && !descendants.Contains(x));

            }
            return false;
        }
        private static void ExecuteRotationMatrixInput(BoneViewModel bone)
        {
            var q = bone.Bone.Orientation;
            var m = new Matrix(new Quaternion(q.A, q.B, q.C, q.D));
            var dialog = new MatrixInputDialog(m, "Rotation Matrix Input");
            var result = dialog.ShowDialog() ?? false;
            if(result)
            {
                bone.Rotation = new EulerAngle(dialog.Value);
            }

        }
        private static void SuffixRecursive(BoneViewModel bone, String suffix)
        {
            bone.BoneName = bone.BoneName + suffix;
            foreach (var child in bone.Children)
            {
                SuffixRecursive(child, suffix);
            }
        }
        private static void PrefixRecursive(BoneViewModel bone, String prefix)
        {
            bone.BoneName = prefix + bone.BoneName;
            foreach (var child in bone.Children)
            {
                PrefixRecursive(child, prefix);
            }
        }
        private static void FindReplaceRecursive(BoneViewModel bone, String find, String replace)
        {
            bone.BoneName = bone.BoneName.Replace(find, replace);
            foreach (var child in bone.Children)
            {
                FindReplaceRecursive(child, find, replace);
            }
        }
        private static void ExecuteFindReplace(BoneViewModel target)
        {
            var dialog = new FindReplaceDialog("Find and Replace in Hierarchy...");
            var result = dialog.ShowDialog() ?? false;
            if (result)
            {
                FindReplaceRecursive(target, dialog.Find, dialog.Replace);
            }
        }
        private static void ExecutePrefix(BoneViewModel target)
        {
            var dialog = new StringInputDialog("Prefix Names in Hierarchy...");
            var result = dialog.ShowDialog() ?? false;
            if (result)
            {
                PrefixRecursive(target, dialog.Value);
            }
        }
        private static void ExecuteSuffix(BoneViewModel target)
        {
            var dialog = new StringInputDialog("Suffix Names in Hierarchy...");
            var result = dialog.ShowDialog() ?? false;
            if (result)
            {
                SuffixRecursive(target, dialog.Value);
            }
        }
        private static void ExecuteUnparent(BoneViewModel target)
        {
            target.Manager.SetParent(target.Bone, null);
        }
        private static void ExecuteSetParent(BoneViewModel target)
        {
            var descendants = target.Manager.GetDescendants(target.Bone).OrderBy(x=>x.Name);
            var choices = target.Manager.Bones.Where(x => x != target.Bone && !descendants.Contains(x)).ToList();
            choices.Sort((x, y) => x.Name.CompareTo(y.Name));
            var dialog = new BoneSelectDialog(choices,"Select a New Parent...");
            var result = dialog.ShowDialog()??false;
            if(result)
            {
                target.Manager.SetParent(target.Bone,dialog.SelectedBone);
            }
        }
        private static void ExecuteDeleteBone(BoneViewModel target)
        {
            foreach(var bone in target.Manager.GetChildren(target.Bone))
            {
                target.Manager.SetParent(bone,target.Parent is BoneViewModel? ((BoneViewModel)target.Parent).Bone:null);
            }
            target.Manager.DeleteBone(target.Bone, false);

        }
        private static void ExecuteDeleteHierarchy(BoneViewModel target)
        {
            target.Manager.DeleteBone(target.Bone, true);
        }

        private const string NEW_BONE_NAME = "<New Bone>";
        private static void ExecuteAddBone(BoneViewModel target)
        {
            target.Manager.AddBone(new RigResource.RigResource.Bone(0, null, new Vertex(0, null), new s3pi.Interfaces.Quaternion(0,null,0f,0f,0f,1f), new Vertex(0, null), NEW_BONE_NAME, target.Manager.Bones.Count, -1, FNV32.GetHash(NEW_BONE_NAME),0x23), target.Bone);

        }
        private static void ExecuteClone(BoneViewModel target)
        {
            target.Manager.AddBone(new RigResource.RigResource.Bone(0, null, target.Bone), target.Parent is BoneViewModel ? ((BoneViewModel)target.Parent).Bone : null);

        }
        private static void ExecuteCloneHierarchy(BoneViewModel target)
        {
            CloneHierarchy(target.Manager, target.Bone, target.Manager.GetParent(target.Bone));
        }
        private static void CloneHierarchy(BoneManager manager, RigResource.RigResource.Bone bone, RigResource.RigResource.Bone dest)
        {
            var descendants = manager.GetDescendants(bone).ToList();
            var clones = new List<RigResource.RigResource.Bone>();
            var map = new Dictionary<RigResource.RigResource.Bone, RigResource.RigResource.Bone>();
            var root = new RigResource.RigResource.Bone(0, null, bone);
            map[bone] = root;
            manager.AddBone(root, dest);

            foreach (var descendant in descendants)
            {
                var clone = new RigResource.RigResource.Bone(0, null, descendant);
                map[descendant] = clone;
                clones.Add(clone);
                manager.AddBone(clone, manager.GetParent(descendant));
            }
            foreach (var c in clones)
            {
                var parent = manager.GetParent(c);
                if (map.ContainsKey(parent))
                {
                    manager.SetParent(c, map[parent]);
                }
            }
        }
    }
}
