namespace Controllers
{
    public class GameSceneController : SceneControllerBase
    {
        protected override void Start()
        {
            base.Start();
            if (!IsInitialized) return;
            
           InitScene();
        }

        protected override void InitScene()
        {
        }
    }
}