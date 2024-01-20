using Godot;
using Mojang.Api.Skins.Data;
using Mojang.Api.Skins.Godot.src.Data;
using System;

namespace Mojang.Api.Skins.Godot.src.Nodes;
public partial class PlayerNode : Node3D
{
    [Export]
    public float RotationSpeed = 1.0f;

    [Export]
    private Node3D _classicPlayer;
    [Export]
    private Node3D _slimPlayer;
    [Export]
    private Node3D _cape;
    [Export]
    private Node3D _elytra;

    [Export]
    private ShaderMaterial _playerMaterial;
    [Export]
    private StandardMaterial3D _playerAccesoryMaterial;
    [Export]
    private StandardMaterial3D _capeMaterial;

    public PlayerModel3D ClassicPlayerModel { get; private set; }
    public PlayerModel3D SlimPlayerModel { get; private set; }
    public Node3D Cape => _cape;
    public Node3D Elytra => _elytra;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ClassicPlayerModel = PlayerModel3D.CreateFromNode3D(_classicPlayer);
        ClassicPlayerModel.SetMaterial(_playerMaterial);
        ClassicPlayerModel.SetAccesoryMaterial(_playerAccesoryMaterial);

        SlimPlayerModel = PlayerModel3D.CreateFromNode3D(_slimPlayer);
        SlimPlayerModel.SetMaterial(_playerMaterial);
        SlimPlayerModel.SetAccesoryMaterial(_playerAccesoryMaterial);

        StartAnimation("Walking");
        //StartAnimation("Breathing");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    float rotationSpeed = 20f;
    public override void _Process(double delta)
    {
        //float rotationDelta = rotationSpeed * (float)delta;

        //// Anwendung der Rotation
        //RotateY(Mathf.DegToRad(rotationDelta));
    }


    public bool StartAnimation(string animationName)
    {
        try
        {
            var classicAnimationPlayer = _classicPlayer.GetNode<AnimationPlayer>("MinecraftAnimations");
            classicAnimationPlayer.Play(animationName);

            var slimAnimationPlayer = _slimPlayer.GetNode<AnimationPlayer>("MinecraftAnimations");
            slimAnimationPlayer.Play(animationName);
            return true;
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Fehler beim Abspielen der Animation: {ex}");
            return false;
        }
    }

    public void ChangeTextures(PlayerData playerData)
    {
        ChangePlayerTexture(playerData);
        ChangeCapeTexture(playerData);

    }

    private void ChangeCapeTexture(PlayerData playerData)
    {
        if (playerData.Cape is null) return;

        var image = new Image();
        var error = image.LoadPngFromBuffer(playerData.Cape.TextureBytes);

        if (error == Error.Ok)
        {
            var texture = ImageTexture.CreateFromImage(image);
            _capeMaterial.AlbedoTexture = texture;
        }
        else
        {
            GD.PrintErr("Fehler beim Laden der Textur");
        }
    }

    private void ChangePlayerTexture(PlayerData playerData)
    {
        var image = new Image();
        var error = image.LoadPngFromBuffer(playerData.Skin.TextureBytes);

        if (error == Error.Ok)
        {
            var texture = ImageTexture.CreateFromImage(image);
            _playerAccesoryMaterial.AlbedoTexture = texture;
            _playerMaterial.SetShaderParameter("Texture2DParameter", texture);
        }
        else
        {
            GD.PrintErr("Fehler beim Laden der Textur");
        }
    }
}
