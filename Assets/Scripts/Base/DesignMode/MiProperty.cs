using BXB.Core;


namespace BXB
{
    public class MiProperty : MiSingleton<MiProperty>
    {
        //MiBaseClass.MiBaseClass BaseClass => MiBaseClass.MiBaseClass;

        MiAsyncManager AsyncManager => MiAsyncManager.Instance;
        MiInputManager InputManager => MiInputManager.Instance;

        //MiUIDialog Dialog => MiUIDialog;
    }
}
