using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API;

namespace ZombieLimiter
{
    public class Config : IRocketPluginConfiguration
    {
        public bool Enabled;
        public int SleepTimeZombie;
        public int SleepTimeNoClear;
        public int MaxZombiesAllowed;

        public void LoadDefaults()
        {
            Enabled = true;
            SleepTimeZombie = 1500;
            SleepTimeNoClear = 3000;
            MaxZombiesAllowed = 25;
        }
    }
}
