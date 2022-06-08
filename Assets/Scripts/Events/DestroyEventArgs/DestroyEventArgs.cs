using System;

namespace Assets.Scripts.Events.DestroyEventArgs
{
    public class DestroyEventArgs : EventArgs
    {
        public readonly int ObjectToDestroyId;

        public DestroyEventArgs(int objectToDestroyId)
        {
            ObjectToDestroyId = objectToDestroyId;
        }
    }
}
