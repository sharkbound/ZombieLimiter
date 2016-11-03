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
        public bool LogKills;
        public int SecondsBetweenZombieClears;
        public int MaxZombiesAllowed;

        public void LoadDefaults()
        {
            Enabled = true;
            LogKills = true;
            SecondsBetweenZombieClears = 5;
            MaxZombiesAllowed = 15;
        }
    }
}
