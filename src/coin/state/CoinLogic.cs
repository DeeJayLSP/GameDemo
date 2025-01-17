namespace GameDemo;

using Chickensoft.LogicBlocks;
using Chickensoft.LogicBlocks.Generator;

public interface ICoinLogic : ILogicBlock<CoinLogic.IState> { }

[StateMachine]
public partial class CoinLogic : LogicBlock<CoinLogic.IState>, ICoinLogic {
  public override IState GetInitialState(IContext context) =>
    new State.Idle(context);

  public record Settings(double CollectionTimeInSeconds);

  public CoinLogic(ICoin coin, Settings settings, IAppRepo appRepo) {
    Set(coin);
    Set(settings);
    Set(appRepo);
  }
}
