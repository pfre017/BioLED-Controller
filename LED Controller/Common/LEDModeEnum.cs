using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED_Controller.Common
{
    public enum LEDModeEnum
    {
        Disabled,
        Constant,
        Trigger1,
        Follower,
        Pulse
    }

    public static class Extension
    {
        public static int LEDModeToInt(this LEDModeEnum Mode)
        {
            switch (Mode)
            {
                case LEDModeEnum.Disabled:
                    return 0;
                case LEDModeEnum.Constant:
                    return 1;
                case LEDModeEnum.Trigger1:
                    return 3;
                case LEDModeEnum.Follower:
                    return 3;
                case LEDModeEnum.Pulse:
                    return 3;
                default:
                    return 0;
            }
        }
    }
}
