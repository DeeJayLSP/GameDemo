namespace GameDemo.Tests;

using System.Threading.Tasks;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.GoDotTest;
using Godot;
using Moq;
using Shouldly;

public class DeathMenuTest : TestClass {
  private Mock<IButton> _mainMenuButton = default!;
  private Mock<IButton> _tryAgainButton = default!;
  private Mock<IAnimationPlayer> _animationPlayer = default!;
  private DeathMenu _menu = default!;

  public DeathMenuTest(Node testScene) : base(testScene) { }

  [Setup]
  public void Setup() {
    _mainMenuButton = new Mock<IButton>();
    _tryAgainButton = new Mock<IButton>();
    _animationPlayer = new Mock<IAnimationPlayer>();

    _menu = new DeathMenu {
      MainMenuButton = _mainMenuButton.Object,
      TryAgainButton = _tryAgainButton.Object,
      AnimationPlayer = _animationPlayer.Object
    };
  }

  [Test]
  public void Subscribes() {
    _menu.OnReady();
    _mainMenuButton.VerifyAdd(menu => menu.Pressed += _menu.OnMainMenuPressed);
    _tryAgainButton.VerifyAdd(menu => menu.Pressed += _menu.OnTryAgainPressed);

    _menu.OnExitTree();
    _mainMenuButton
      .VerifyRemove(menu => menu.Pressed -= _menu.OnMainMenuPressed);
    _tryAgainButton
      .VerifyRemove(menu => menu.Pressed -= _menu.OnTryAgainPressed);
  }

  [Test]
  public async Task SignalsMainMenuButtonPressed() {
    var signal = _menu.ToSignal(_menu, DeathMenu.SignalName.MainMenu);

    _menu.OnMainMenuPressed();

    await signal;

    signal.IsCompleted.ShouldBeTrue();
  }

  [Test]
  public async Task SignalsTryAgainButtonPressed() {
    var signal = _menu.ToSignal(_menu, DeathMenu.SignalName.TryAgain);

    _menu.OnTryAgainPressed();

    await signal;

    signal.IsCompleted.ShouldBeTrue();
  }

  [Test]
  public void Animates() {
    _animationPlayer
      .Setup(player => player.Play("splotch", -1, 1, false));

    _menu.Animate();

    _animationPlayer.VerifyAll();
  }
}
