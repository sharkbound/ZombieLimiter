using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SDG.Unturned;
using Rocket.API;
using Rocket.Core.Commands;
using Rocket.Unturned.Chat;

namespace ZombieLimiter
{
    public class Plugin : RocketPlugin<Config>
    {
        public static Plugin Instance;
        public static Thread thread;

        bool ClearsEnabled = true;

        DateTime LastClear = DateTime.Now;

        protected override void Load()
        {
            Instance = this;
            Logger.Log("ZombieLimiter has loaded!");
            ClearsEnabled = true;
        }

        protected override void Unload()
        {
            Logger.Log("ZombieLimiter has Unloaded!");
            ClearsEnabled = false;
        }

        public void Update()
        {
            if (ClearsEnabled)
            {
                KillZombies();
            }
        }

        Random rand = new Random();
        int randomValue = 0;
        void KillZombies()
        {
            try
            {
                if (ZombieManager.tickingZombies.Count > GetAllowedZombies())
                {
                    randomValue = rand.Next(ZombieManager.tickingZombies.Count);
                    Zombie zom = ZombieManager.tickingZombies[randomValue];

                    ZombieManager.sendZombieDead(zom, UnityEngine.Vector3.zero);
                    //zom.tellDead(UnityEngine.Vector3.zero);

                    if (Configuration.Instance.LogKills)
                    {
                        Logger.Log("Killed a zombie, current ticked zombie count: " + ZombieManager.tickingZombies.Count);
                    }
                } 
            }
            catch { }
        }

        int GetAllowedZombies()
        {
            return Configuration.Instance.MaxZombiesAllowed;
        }

        int GetTimeBetweenClears()
        {
            return Configuration.Instance.SecondsBetweenZombieClears;
        }

        [RocketCommand("zstop", "", "", Rocket.API.AllowedCaller.Both)]
        public void CommandStopThread(IRocketPlayer caller, string[] parameters)
        {
            ClearsEnabled = false;
            sendMsg(caller, "Disabled Clears!");
        }

        [RocketCommand("zstart", "", "", Rocket.API.AllowedCaller.Both)]
        public void CommandStartThread(IRocketPlayer caller, string[] parameters)
        {
            ClearsEnabled = true;
            sendMsg(caller, "Enabled Clears!");
        }

        [RocketCommand("zlist", "", "", Rocket.API.AllowedCaller.Both)]
        public void CommandZList(IRocketPlayer caller, string[] parameters)
        {
            sendMsg(caller, "Current Ticked zombies: " + ZombieManager.tickingZombies.Count.ToString());
        }

        void sendMsg(IRocketPlayer caller, string msg)
        {
            if (caller is ConsolePlayer)
                Logger.LogWarning(msg);
            else
                UnturnedChat.Say(caller, msg);
        }
    }

    public class test
    {
        [RocketCommand("zzzzz", "", "", Rocket.API.AllowedCaller.Both)]
        public void CommandStopThread(IRocketPlayer caller, string[] parameters)
        {
            Logger.Log("works!");
        }
    }
}
