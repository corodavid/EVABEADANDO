using Connect4.Model;
using Connect4.Persistance;
using Moq;

namespace Connect4Test
{
    [TestClass]
    public class Connect4Test
    {
        #region Fields

        private GameTable? _mockedTable = null;
        private GameModel? _model = null;
        private IGameTimer? _firstPlayerTimer = null;
        private IGameTimer? _secondPlayerTimer = null;
        private Mock<IGameTableDataAccess>? _mock = null;

        #endregion

        [TestInitialize]
        public void Initialize()
        {
            _mockedTable = new GameTable(6);
            _firstPlayerTimer = new MockedGameTimer();
            _secondPlayerTimer = new MockedGameTimer();
            _model = new GameModel(_mockedTable, _firstPlayerTimer, _secondPlayerTimer);
            _mock = new Mock<IGameTableDataAccess>();
            _model = new GameModel(_mock.Object, _mockedTable);

            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult((_firstPlayerTimer.RemainingTime, _secondPlayerTimer.RemainingTime, _mockedTable)));
        }


        [TestMethod]
        public void GameModelNewGameTest()
        {
            _model!.NewGame(7);
            _mockedTable = new(7);
            Assert.AreEqual(7, _model.TableSize);
            _model.StartGame();
            Assert.AreEqual(GameState.RUNNING, _model.State);
            _model.Pause();
            Assert.AreEqual(GameState.PAUSED, _model.State);
        }

        [TestMethod]
        public void GameModelInsertionSucceedsWhenSuccessfullFailsIfIsFull()
        {
            _model!.StartGame();
            for (int i = 0; i < _model.TableSize; i++)
            {
                _model.Round(0);
                Assert.AreNotEqual(FieldStatus.NONE, _model[_model.TableSize - i - 1, 0]);
            }
            Assert.IsFalse(_model.TryInsert(0));
        }
        [TestMethod]
        public void GameModelFirstPlayerAllWinsWork()
        {
            _model!.StartGame();
            for (int i = 0; i < 4; i++)
            {
                _model.Round(i);
                _model.Round(i);
            }
            Assert.AreEqual(GameState.FIRST, _model.State);
            _model.NewGame(_model.TableSize);
            _model.StartGame();
            for (int i = 0; i < 4; i++)
            {
                _model.Round(0);
                _model.Round(i + 1);
            }
            _mockedTable = new(6);
            Assert.AreEqual(GameState.FIRST, _model.State);
            _model.NewGame();
            _model.StartGame();
            for (int i = 0; i < 3; i++)
            {
                _model.Round(i);
                _model.Round(3);
            }
            _model.Round(3);
            Assert.AreEqual(GameState.FIRST, _model.State);
        }

        [TestMethod]
        public void GameTimerTimeTest()
        {
            _model!.StartGame();

            int time = _model.RemainingTime.Item1;
            Assert.IsTrue(_firstPlayerTimer!.IsRunning);

            while (_model.State == GameState.RUNNING)
            {
                _firstPlayerTimer.RaiseTicked();
                time--;

                Assert.AreEqual(time, _model.RemainingTime.Item1);
                Assert.AreEqual(GameState.RUNNING, _model.State);
            }

            Assert.AreEqual(0, _model.RemainingTime.Item1);
            Assert.AreEqual(GameState.SECOND, _model.State);

        }

        [TestMethod]
        public async Task GameModelLoadTest()
        {
            _model!.NewGame();

            await _model.LoadAsync(String.Empty);

            for (int i = 0; i < _mockedTable!.Size; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                    Assert.AreEqual(_mockedTable[i, j], _model[i, j]);
                }
            }

            _mock!.Verify(dataAccess => dataAccess.LoadAsync(String.Empty), Times.Once());
        }

        [TestMethod]
        public void GameModelRoundTesting()
        {
            _model!.StartGame();
            for (int i = 0; i < _mockedTable!.Size; i++)
            {
                _model.Round(i);
                Assert.AreEqual(_mockedTable[_mockedTable.Size - 1, i], _model[_model.TableSize - 1, i]);
            }
            Assert.AreEqual(GameState.RUNNING, _model.State);
            _model.NewGame();
            _model.StartGame();
            for (int i = 0; i < _mockedTable.Size; i++)
            {
                _model.Round(i);
                Assert.AreEqual(_mockedTable[_mockedTable.Size - 1, i], _model[_model.TableSize - 1, i]);
            }
            Assert.AreEqual(GameState.RUNNING, _model.State);

            _model.NewGame();
            _model.StartGame();

        }


    }
}