using Godot;
using System;

namespace UI
{
    public partial class InGameMenu : CanvasLayer
    {
        public override void _Notification(int what)
        {
            if (what == NotificationPaused)
            {
                Visible = true;
            }
            else if (what == NotificationUnpaused)
            {
                Visible = false;
            }
        }
    }
}
