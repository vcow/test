namespace Models
{
    public class GameModel
    {
        private static GameModel _instance;

        private GameModel()
        {
        }

        public static GameModel Instance
        {
            get { return _instance ?? (_instance = new GameModel()); }
        }
    }
}