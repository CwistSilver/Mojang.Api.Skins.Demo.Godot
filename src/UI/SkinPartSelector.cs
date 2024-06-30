using Godot;
using Mojang.Api.Skins.Data;
using Mojang.Api.Skins.Godot.src.Nodes;

namespace Mojang.Api.Skins.Godot.src.UI;
public partial class SkinPartSelector : Control
{
    private static readonly SkinPart[] _availableSkinParts = new SkinPart[]
    {
        SkinPart.Head_FrontSide,
        SkinPart.Body_FrontSide,
        SkinPart.LeftArm_FrontSide,
        SkinPart.RightArm_FrontSide,
        SkinPart.LeftLeg_FrontSide,
        SkinPart.RightLeg_FrontSide,

        SkinPart.HeadAccesory_FrontSide,
        SkinPart.BodyAccesory_FrontSide,
        SkinPart.LeftArmAccesory_FrontSide,
        SkinPart.RightArmAccesory_FrontSide,
        SkinPart.LeftLegAccesory_FrontSide,
        SkinPart.RightLegAccesory_FrontSide
    };


    private PlayerNode _playerNode;

    private PlayerData _currentPlayerData;

    private ColorRect _elytraButtonBackground;
    private bool _showCape = true;
    private CapePart _showCapePart = CapePart.FrontSide;

    public void SetPlayerNode(PlayerNode playerNode)
    {
        _playerNode = playerNode;
    }

    public void SetTextures(PlayerData playerData)
    {
        _currentPlayerData = playerData;

        Load2DSkinParts(_currentPlayerData);

        if (_currentPlayerData.Skin.SkinType == SkinType.Classic)
        {
            _playerNode.ChangeTextures(_currentPlayerData);
            _playerNode.SlimPlayerModel.PlayerModel.Visible = false;
            _playerNode.ClassicPlayerModel.PlayerModel.Visible = true;
        }
        else
        {
            GD.PrintErr($"Slim player");
            _playerNode.ChangeTextures(_currentPlayerData);
            _playerNode.SlimPlayerModel.PlayerModel.Visible = true;
            _playerNode.ClassicPlayerModel.PlayerModel.Visible = false;
        }
    }

    private void InitElytra()
    {
        _elytraButtonBackground = (ColorRect)FindChild("ElytraButton-Background");

        var elytraButton = _elytraButtonBackground.GetNode<TextureButton>("ElytraButton");
        elytraButton.Pressed += ElytraButton_Pressed;
        elytraButton.MouseEntered += () => Button_MouseEntered(elytraButton);
        elytraButton.MouseExited += () => Button_MouseExited(elytraButton);
    }


    public override void _Ready()
    {
        _elytraButtonBackground = (ColorRect)FindChild("ElytraButton-Background");
        _elytraButtonBackground = (ColorRect)FindChild("ElytraButton-Background");
        var animationC = GetNode<Control>("AnimationControl");

        var elytraButton = _elytraButtonBackground.GetNode<TextureButton>("ElytraButton");
        elytraButton.Pressed += ElytraButton_Pressed;
        elytraButton.MouseEntered += () => Button_MouseEntered(elytraButton);
        elytraButton.MouseExited += () => Button_MouseExited(elytraButton);

        InitButtonEvents();
    }


    private void ElytraButton_Pressed()
    {
        if (_showCapePart == CapePart.FrontSide)
        {
            _showCapePart = CapePart.Elytra_FrontSide;
            _playerNode.Cape.Visible = false;

            if (_showCape && _currentPlayerData.Cape is not null)
                _playerNode.Elytra.Visible = true;

            _elytraButtonBackground.GetNode<ColorRect>("Selected-ColorRect").Visible = false;
        }
        else
        {
            _showCapePart = CapePart.FrontSide;
            if (_showCape && _currentPlayerData.Cape is not null)
            {
                _playerNode.Cape.Visible = true;
                _playerNode.Elytra.Visible = false;
            }

            _elytraButtonBackground.GetNode<ColorRect>("Selected-ColorRect").Visible = true;
        }
    }




    private void ChangeCapeVisibility(bool visibility)
    {
        if (visibility && _currentPlayerData.Cape is null) return;

        if (_showCapePart == CapePart.FrontSide)
            _playerNode.Cape.Visible = visibility;
        else
            _playerNode.Elytra.Visible = visibility;
    }

    private void Load2DSkinParts(PlayerData playerData)
    {
        foreach (var skinPart in _availableSkinParts)
            SetBodyPart(playerData, skinPart);

        if (playerData.Cape is null)
        {
            var capeButton = GetTextureButton(CapePart.FrontSide);
            capeButton.TextureNormal = null;
            ChangeCapeVisibility(false);
        }
        else if (_showCape)
        {
            ChangeCapeVisibility(true);
            SetCapePart(_currentPlayerData, CapePart.FrontSide);
        }
    }

    private void InitButtonEvents()
    {
        foreach (var skinPart in _availableSkinParts)
        {
            var button = GetTextureButton(skinPart);
            button.ButtonDown += () => OnTextureButtonPress(button, skinPart);
            button.MouseEntered += () => Button_MouseEntered(button);
            button.MouseExited += () => Button_MouseExited(button);
        }

        var capeButton = GetTextureButton(CapePart.FrontSide);
        capeButton.ButtonDown += () => OnCapeTextureButtonPress(capeButton, CapePart.FrontSide);
        capeButton.MouseEntered += () => Button_MouseEntered(capeButton);
        capeButton.MouseExited += () => Button_MouseExited(capeButton);
    }

    private static void Button_MouseExited(TextureButton sender)
    {
        var hoverColorRect = sender.GetParent().GetNode<ColorRect>("Hover-ColorRect");
        hoverColorRect.Visible = false;
    }

    private static void Button_MouseEntered(TextureButton sender)
    {
        var hoverColorRect = sender.GetParent().GetNode<ColorRect>("Hover-ColorRect");
        hoverColorRect.Visible = true;
    }

    private void OnCapeTextureButtonPress(TextureButton sender, CapePart capePart)
    {
        if (_currentPlayerData is null) return;

        var background = GetBackground(capePart);
        if (!_showCape)
        {
            background.Color = new Color(1f, 1f, 1f, 0.19f);


            if (_currentPlayerData.Cape is not null)
                ChangeCapeVisibility(true);


            SetCapePart(_currentPlayerData, capePart);

            _showCape = true;
        }
        else
        {
            background.Color = new Color(0f, 0f, 0f, 0.19f);
            sender.TextureNormal = null;

            ChangeCapeVisibility(false);
            _showCape = false;
        }
    }

    private void OnTextureButtonPress(TextureButton sender, SkinPart skinPart)
    {
        if (_currentPlayerData is null) return;

        if (sender.TextureNormal is not null)
        {
            sender.TextureNormal = null;
            var background = GetBackground(skinPart);
            background.Color = new Color(0f, 0f, 0f, 0.19f);

            _playerNode.ClassicPlayerModel.GetSkinMeshPart(skinPart).Visible = false;
            _playerNode.SlimPlayerModel.GetSkinMeshPart(skinPart).Visible = false;
        }
        else
        {
            var background = GetBackground(skinPart);
            background.Color = new Color(1f, 1f, 1f, 0.19f);

            _playerNode.ClassicPlayerModel.GetSkinMeshPart(skinPart).Visible = true;
            _playerNode.SlimPlayerModel.GetSkinMeshPart(skinPart).Visible = true;

            SetBodyPart(_currentPlayerData, skinPart);
        }
    }

    private TextureButton GetTextureButton(SkinPart skinPart)
    {
        var bodyBackground = GetBackground(skinPart);
        return bodyBackground.GetNode<TextureButton>(skinPart.ToString());
    }

    private TextureButton GetTextureButton(CapePart capePart)
    {
        var bodyBackground = GetBackground(capePart);
        return bodyBackground.GetNode<TextureButton>(capePart.ToString());
    }

    private ColorRect GetBackground(SkinPart skinPart)
    {
        var partName = skinPart.ToString();
        Node bodyControl;
        if (partName.Contains("Accesory"))
            bodyControl = GetNode("PlayerAccesory-SkinParts");
        else
            bodyControl = GetNode("Player-SkinParts");

        var background = $"{skinPart}-Background";
        return bodyControl.GetNode<ColorRect>(background);
    }

    private ColorRect GetBackground(CapePart capePart)
    {
        var bodyControl = GetNode("Player-Cape");

        var background = $"{capePart}-Background";
        return bodyControl.GetNode<ColorRect>(background);
    }

    private void SetBodyPart(PlayerData playerData, SkinPart skinPart)
    {
        if (!_playerNode.ClassicPlayerModel.GetSkinMeshPart(skinPart).Visible)
            return;

        var bodyControls = GetTextureButton(skinPart);

        var image = new Image();
        var skinPartData = playerData.Skin.GetSkinPart(skinPart);
        var error = image.LoadPngFromBuffer(skinPartData.TextureBytes);

        if (error == Error.Ok)
        {
            var texture = (Texture2D)ImageTexture.CreateFromImage(image);
            bodyControls.TextureNormal = texture;
        }
        else
            GD.PrintErr($"Image for body part {skinPart} for player '{playerData}' could not be loaded!");
    }

    private void SetCapePart(PlayerData playerData, CapePart capePart)
    {
        if (playerData.Cape is null || !_playerNode.Cape.Visible)
            return;

        var bodyControls = GetTextureButton(capePart);

        var image = new Image();
        var capePartData = playerData.Cape.GetCapePart(capePart);
        var error = image.LoadPngFromBuffer(capePartData.TextureBytes);

        if (error == Error.Ok)
        {
            var texture = (Texture2D)ImageTexture.CreateFromImage(image);
            bodyControls.TextureNormal = texture;
        }
        else
            GD.PrintErr($"Image for cape part {capePart} for player '{playerData}' could not be loaded!");
    }
}
