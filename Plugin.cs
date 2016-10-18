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

        protected override void Load()
        {
            Instance = this;
            Logger.Log("ZombieLimiter has loaded!");
            new Thread(() => StartThreadDelayed(10000)).Start();
        }

        protected override void Unload()
        {
            Logger.Log("ZombieLimiter has Unloaded!");
            StopThread();
        }

        public void StartThreadDelayed(int delay)
        {
            Thread.Sleep(delay);
            thread = new Thread(() => KillZombiesThread());
            thread.Start();
        }

        public void StopThread()
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }

        void KillZombiesThread()
        {
            Random rand = new Random();
            int randomValue = 0;
            bool DidAClear = false;

            try
            {
                while (true)
                {
                    if (ZombieManager.tickingZombies.Count > GetAllowedZombies())
                    {
                        Logger.LogWarning("Starting to kill zombies, current zombie count: " + ZombieManager.tickingZombies.Count.ToString());

                        while (ZombieManager.tickingZombies.Count > GetAllowedZombies())
                        {
                            randomValue = rand.Next(ZombieManager.tickingZombies.Count);
                            Zombie zom = ZombieManager.tickingZombies[randomValue];

                            ZombieManager.sendZombieDead(zom, UnityEngine.Vector3.zero);

                            Thread.Sleep(GetSleepTimeZombie());
                        }

                        DidAClear = true;
                    }

                    if (DidAClear)
                    {
                        Logger.LogWarning("Finished a zombie clear!");
                        DidAClear = false;
                    }

                    Thread.Sleep(GetSleepTimeNoClear());
                }
            }
            catch
            {
                Logger.LogWarning("The thread has had a error or crashed! Restarting the thread!");
                StartThreadDelayed(5000);
            }
        }

        int GetAllowedZombies()
        {
            return Configuration.Instance.MaxZombiesAllowed;
        }

        int GetSleepTimeZombie()
        {
            return Configuration.Instance.SleepTimeZombie;
        }

        int GetSleepTimeNoClear()
        {
            return Configuration.Instance.SleepTimeNoClear;
        }

        [RocketCommand("zstop", "", "", Rocket.API.AllowedCaller.Both)]
        public void CommandStopThread(IRocketPlayer caller, string[] parameters)
        {
            if (thread == null)
            {
                sendMsg(caller, "Zombie clearing thread isnt running currently!");
                return;
            }

            thread.Abort();
            thread = null;

            sendMsg(caller, "Stopped the zombie clearing thread!");
        }

        [RocketCommand("zstart", "", "", Rocket.API.AllowedCaller.Both)]
        public void CommandStartThread(IRocketPlayer caller, string[] parameters)
        {

            if (thread != null)
            {
                sendMsg(caller, "Zombie clearing thread is already running!");
                return;
            }

            thread = new Thread(() => KillZombiesThread());
            thread.Start();

            sendMsg(caller, "Started the zombie clearing thread!");
        }

        [RocketCommand("zlist", "", "", Rocket.API.AllowedCaller.Both)]
        public void CommandZList(IRocketPlayer caller, string[] parameters)
        {
            sendMsg(caller, "Current zombies: " + ZombieManager.tickingZombies.Count.ToString());
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
