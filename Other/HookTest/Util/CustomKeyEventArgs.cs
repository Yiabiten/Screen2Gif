using System;

namespace HookTest.Util
{
    public class CustomKeyEventArgs : EventArgs
    {
        public Keys Key { get; private set; }

        public bool Handled { get; private set; }

        public CustomKeyEventArgs(Keys key)
        {
            Key = key;
        }
    }
}
