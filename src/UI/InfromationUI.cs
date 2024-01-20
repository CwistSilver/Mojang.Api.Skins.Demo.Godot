using Godot;
using Mojang.Api.Skins.Data;
using Mojang.Api.Skins.Godot.src.Nodes;
using Mojang.Api.Skins;
using System;
using System.Text;
using System.Threading.Tasks;
using Mojang.Api.Skins.Godot.src.Data;

public partial class InfromationUI : Control
{
    [Export]
    private PlayerNode _playerNode;

    private PlayerData _currentPlayerData;
    private LineEdit _playerNameLineEdit;
    private Label _detailsLable;
    private Button _loadPlayerButton;
    private ColorRect _elytraButtonBackground;
    private OptionButton _animationOptionButton;
    private TextureButton _headTextureButton;
    private SkinsClient _client;
    private SkinPartSelector _skinPartSelector;

    public override void _Ready()
    {
        _client = new SkinsClient();
        _skinPartSelector = GetNode<SkinPartSelector>("SkinPartSelector");
        _skinPartSelector.SetPlayerNode(_playerNode);

        _headTextureButton = (TextureButton)FindChild("HeadTexture");
        _loadPlayerButton = (Button)FindChild("PlayerLoadButton");
        _playerNameLineEdit = (LineEdit)FindChild("PlayerNameLineEdit");
        _elytraButtonBackground = (ColorRect)FindChild("ElytraButton-Background");
        _elytraButtonBackground = (ColorRect)FindChild("ElytraButton-Background");
        var animationC = GetNode<Control>("AnimationControl");
        _animationOptionButton = animationC.GetNode<OptionButton>("AnimationOptionButton");
        _animationOptionButton.ItemSelected += _animationOptionButton_ItemSelected;
        _animationOptionButton.AddItem("Walking", 1);
        _animationOptionButton.AddItem("Breathing", 2);


        _loadPlayerButton.Pressed += LoadPlayerButton_Pressed;

        _detailsLable = (Label)FindChild("DetailsLabel");

        _playerNode.Ready += () => ShowDefaultSkin();
    }


    private void _animationOptionButton_ItemSelected(long index)
    {
        _playerNode.StartAnimation(_animationOptionButton.GetItemText((int)index));
    }


    private void Button_MouseExited(TextureButton sender)
    {
        var hoverColorRect = sender.GetParent().GetNode<ColorRect>("Hover-ColorRect");
        hoverColorRect.Visible = false;
    }

    private void Button_MouseEntered(TextureButton sender)
    {
        var hoverColorRect = sender.GetParent().GetNode<ColorRect>("Hover-ColorRect");
        hoverColorRect.Visible = true;
    }


    private void SetHeadTexture()
    {
        var image = new Image();
        var headFrontTexture = _currentPlayerData.Skin.GetSkinPart(SkinPart.Head_FrontSide);
        var headAccesoryFrontTexture = _currentPlayerData.Skin.GetSkinPart(SkinPart.HeadAccesory_FrontSide);
        var fullHeadTexture = headFrontTexture.Combine(headAccesoryFrontTexture);
        var error = image.LoadPngFromBuffer(fullHeadTexture.TextureBytes);

        if (error == Error.Ok)
        {
            var texture = (Texture2D)ImageTexture.CreateFromImage(image);
            _headTextureButton.TextureNormal = texture;
        }
        else
            GD.PrintErr($"Image for head for player '{_currentPlayerData}' could not be loaded!");
    }

    private void ShowDefaultSkin()
    {
        var defaultSkinBytes = Convert.FromBase64String(DefaultSkins.DefaultSkinBase64);
        var defaultCapeBytes = Convert.FromBase64String(DefaultSkins.DefaultCapeBase64);
        _currentPlayerData = _client.GetLocal(defaultSkinBytes, defaultCapeBytes);
        _currentPlayerData.Name = "Steve";

        _skinPartSelector.SetTextures(_currentPlayerData);
        ShowSkinInfromation();
        SetHeadTexture();
     
        _playerNode.ChangeTextures(_currentPlayerData);
        _playerNode.SlimPlayerModel.PlayerModel.Visible = false;
        _playerNode.ClassicPlayerModel.PlayerModel.Visible = true;
    }

    private void ShowSkinInfromation()
    {
        var detailsText = new StringBuilder();
        detailsText.AppendLine($"Player Name: {_currentPlayerData.Name}");
        detailsText.AppendLine($"Player UUID: {_currentPlayerData.UUID}");
        detailsText.AppendLine($"Player SkinType: {_currentPlayerData.Skin.SkinType}");

        if (_currentPlayerData.Cape is not null)
            detailsText.AppendLine($"Cape Name: {_currentPlayerData.Cape.CapeName}");
        _detailsLable.Text = detailsText.ToString();
        _detailsLable.AddThemeColorOverride("font_color", Colors.WhiteSmoke);
    }

    private void ShowError(Exception exception)
    {
        _detailsLable.Text = exception.Message;
        _detailsLable.AddThemeColorOverride("font_color", Colors.Red);
    }

    private async void LoadPlayerButton_Pressed()
    {
        if (string.IsNullOrEmpty(_playerNameLineEdit.Text))
            return;

        _loadPlayerButton.Disabled = true;
        await LoadPlayer();
        _loadPlayerButton.Disabled = false;

    }

    private async Task LoadPlayer()
    {
        GD.Print($"Loading Player '{_playerNameLineEdit.Text}'");

        try
        {
            _currentPlayerData = await _client.GetAsync(_playerNameLineEdit.Text);
            GD.Print($"Finished Loading player '{_currentPlayerData}'. {_currentPlayerData.Skin.TextureBytes.Length} bytes");
            ShowSkinInfromation();
            SetHeadTexture();
            _skinPartSelector.SetTextures(_currentPlayerData);
        }
        catch (Exception ex)
        {
            ShowDefaultSkin();
            ShowError(ex);
            return;
        }
    }
}


