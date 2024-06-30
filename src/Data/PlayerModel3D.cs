using Godot;
using Mojang.Api.Skins.Data;

namespace Mojang.Api.Skins.Godot.src.Data;
public class PlayerModel3D
{
    public Node3D PlayerModel { get; init; }
    public MeshInstance3D Body { get; init; }
    public MeshInstance3D BodyOut { get; init; }
    public MeshInstance3D Head { get; init; }
    public MeshInstance3D HeadOut { get; init; }
    public MeshInstance3D LeftArm { get; init; }
    public MeshInstance3D LeftArmOut { get; init; }
    public MeshInstance3D RightArm { get; init; }
    public MeshInstance3D RightArmOut { get; init; }
    public MeshInstance3D LeftLeg { get; init; }
    public MeshInstance3D LeftLegOut { get; init; }
    public MeshInstance3D RightLeg { get; init; }
    public MeshInstance3D RightLegOut { get; init; }

    private PlayerModel3D() { }

    public static PlayerModel3D CreateFromNode3D(Node3D playerModel)
    {
        return new PlayerModel3D()
        {
            PlayerModel = playerModel,
            Body = playerModel.GetNode<MeshInstance3D>("Armature/Skeleton3D/Body"),
            BodyOut = playerModel.GetNode<MeshInstance3D>("Armature/Skeleton3D/BodyOut"),
            Head = playerModel.GetNode<MeshInstance3D>("Armature/Skeleton3D/Head"),
            HeadOut = playerModel.GetNode<MeshInstance3D>("Armature/Skeleton3D/HeadOut"),
            LeftArm = playerModel.GetNode<MeshInstance3D>("Armature/Skeleton3D/LArm"),
            LeftArmOut = playerModel.GetNode<MeshInstance3D>("Armature/Skeleton3D/LArmOut"),
            RightArm = playerModel.GetNode<MeshInstance3D>("Armature/Skeleton3D/RArm"),
            RightArmOut = playerModel.GetNode<MeshInstance3D>("Armature/Skeleton3D/RArmOut"),
            LeftLeg = playerModel.GetNode<MeshInstance3D>("Armature/Skeleton3D/LLeg"),
            LeftLegOut = playerModel.GetNode<MeshInstance3D>("Armature/Skeleton3D/LLegOut"),
            RightLeg = playerModel.GetNode<MeshInstance3D>("Armature/Skeleton3D/RLeg"),
            RightLegOut = playerModel.GetNode<MeshInstance3D>("Armature/Skeleton3D/RLegOut")
        };
    }

    public void SetAccesoryMaterial(Material material)
    {
        BodyOut.SetSurfaceOverrideMaterial(0, material);
        HeadOut.SetSurfaceOverrideMaterial(0, material);
        LeftArmOut.SetSurfaceOverrideMaterial(0, material);
        RightArmOut.SetSurfaceOverrideMaterial(0, material);
        LeftLegOut.SetSurfaceOverrideMaterial(0, material);
        RightLegOut.SetSurfaceOverrideMaterial(0, material);
    }

    public void SetMaterial(Material material)
    {
        Body.SetSurfaceOverrideMaterial(0, material);
        Head.SetSurfaceOverrideMaterial(0, material);
        LeftArm.SetSurfaceOverrideMaterial(0, material);
        RightArm.SetSurfaceOverrideMaterial(0, material);
        LeftLeg.SetSurfaceOverrideMaterial(0, material);
        RightLeg.SetSurfaceOverrideMaterial(0, material);
    }

    public MeshInstance3D GetSkinMeshPart(SkinPart skinPart)
    {
        return skinPart switch
        {
            SkinPart.Head_FrontSide => Head,
            SkinPart.Body_FrontSide => Body,
            SkinPart.LeftArm_FrontSide => LeftArm,
            SkinPart.RightArm_FrontSide => RightArm,
            SkinPart.LeftLeg_FrontSide => LeftLeg,
            SkinPart.RightLeg_FrontSide => RightLeg,
            SkinPart.HeadAccesory_FrontSide => HeadOut,
            SkinPart.BodyAccesory_FrontSide => BodyOut,
            SkinPart.LeftArmAccesory_FrontSide => LeftArmOut,
            SkinPart.RightArmAccesory_FrontSide => RightArmOut,
            SkinPart.LeftLegAccesory_FrontSide => LeftLegOut,
            SkinPart.RightLegAccesory_FrontSide => RightLegOut,
            _ => null,
        };
    }
}
