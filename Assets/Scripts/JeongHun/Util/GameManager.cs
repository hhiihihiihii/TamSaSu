using ASTAR;
using DesignPattern;
using Unity.VisualScripting;

namespace Manager
{
    public class GameManager : SingleTon<GameManager>
    {
        public AStarGrid grid;
        public Console consoleManager;

        public bool debugMod = false;
        
        public void Awake() {
            consoleManager = new Console();
            grid = this.transform.GetComponent<AStarGrid>();
            if(grid == null) consoleManager.ErrorPrint(Type.GAMEMANAGER, "AStarGrid Load 실패함");
            

        }
        
        
    }
}