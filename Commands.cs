using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using starcraftbot.proxybot;
using System.Collections;

namespace StarCraftBot_net
{
    public static class Commands
    {
        public enum StarCraftCommand
        {
            none,
            attackMove,
            attackUnit,
            rightClick,
            rightClickUnit,
            train,
            build,
            buildAddon,
            research,
            upgrade,
            stop,
            holdPosition,
            patrol,
            follow,
            setRallyPosition,
            setRallyUnit,
            repair,
            morph,
            burrow,
            unburrow,
            siege,
            unsiege,
            cloak,
            decloak,
            lift,
            land,
            load,
            unload,
            unloadAll,
            unloadAllPosition,
            cancelConstruction,
            haltConstruction,
            cancelMorph,
            cancelTrain,
            cancelTrainSlot,
            cancelAddon,
            cancelResearch,
            cancelUpgrade,
            useTech,
            useTechPosition,
            useTechTarget,
        }
        public struct Command
        {
            public StarCraftCommand CommandID;
            public int UnitID;
            public int Arg0;
            public int Arg1;
            public int Arg2;

            public Command(StarCraftCommand pCommandID, int pUnitID, int pArg0, int pArg1, int pArg2)
            {
                this.CommandID = pCommandID;
                this.UnitID = pUnitID;
                this.Arg0 = pArg0;
                this.Arg1 = pArg1;
                this.Arg2 = pArg2;
            }

            public override string ToString()
            {
                return ":" + (int)CommandID + ";" + UnitID + ";" + Arg0 + ";" + Arg1 + ";" + Arg2;
            }
        }

        /// <summary> Tells the unit to attack move the specific location (in tile coordinates).
        /// 
        /// // static bool attackMove(Position position) = 0;
        /// </summary>
        public static void attackMove(this ProxyBot bot, int unitID, int x, int y)
        {
            bot.doCommand(StarCraftCommand.attackMove, unitID, x, y, 0);
        }
        /// <summary> Tells the unit to attack another unit.
        /// 
        /// // static bool attackUnit(Unit* target) = 0;
        /// </summary>
        public static void attackUnit(this ProxyBot bot, int unitID, int targetID)
        {
            bot.doCommand(StarCraftCommand.attackUnit, unitID, targetID, 0, 0);
        }

        /// <summary> Tells the unit to right click (move) to the specified location (in tile coordinates).
        /// 
        /// // static bool rightClick(Position position) = 0;
        /// </summary>
        public static void rightClick(this ProxyBot bot, int unitID, int x, int y)
        {
            bot.doCommand(StarCraftCommand.rightClick, unitID, x, y, 0);
        }

        /// <summary> Tells the unit to right click (move) on the specified target unit 
        /// (Includes resources).
        /// 
        /// // static bool rightClick(Unit* target) = 0;
        /// </summary>
        public static void rightClick(this ProxyBot bot, int unitID, int targetID)
        {
            bot.doCommand(StarCraftCommand.rightClickUnit, unitID, targetID, 0, 0);
        }

        /// <summary> Tells the building to train the specified unit type.
        /// 
        /// // static bool train(UnitType type) = 0;
        /// </summary>
        public static void train(this ProxyBot bot, int unitID, int typeID)
        {
            bot.doCommand(StarCraftCommand.train, unitID, typeID, 0, 0);
        }

        /// <summary> Tells a worker unit to construct a building at the specified location.
        /// 
        /// // static bool build(TilePosition position, UnitType type) = 0;
        /// </summary>
        public static void build(this ProxyBot bot, int unitID, int tx, int ty, int typeID)
        {
            bot.doCommand(StarCraftCommand.build, unitID, tx, ty, typeID);
        }

        /// <summary> Tells the building to build the specified add on.
        /// 
        /// // static bool buildAddon(UnitType type) = 0;
        /// </summary>
        public static void buildAddon(this ProxyBot bot, int unitID, int typeID)
        {
            bot.doCommand(StarCraftCommand.buildAddon, unitID, typeID, 0, 0);
        }

        /// <summary> Tells the building to research the specified tech type.
        /// 
        /// // static bool research(TechType tech) = 0;
        /// </summary>
        public static void research(this ProxyBot bot, int unitID, int techTypeID)
        {
            bot.doCommand(StarCraftCommand.research, unitID, techTypeID, 0, 0);
        }

        /// <summary> Tells the building to upgrade the specified upgrade type.
        /// 
        /// // static bool upgrade(UpgradeType upgrade) = 0;
        /// </summary>
        public static void upgrade(this ProxyBot bot, int unitID, int upgradeTypeID)
        {
            bot.doCommand(StarCraftCommand.upgrade, unitID, upgradeTypeID, 0, 0);
        }

        /// <summary> Orders the unit to stop moving. The unit will chase enemies that enter its vision.
        /// 
        /// // static bool stop() = 0;
        /// </summary>
        public static void stop(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.stop, unitID, 0, 0, 0);
        }

        /// <summary> Orders the unit to hold position. The unit will not chase enemies that enter its vision.
        /// 
        /// // static bool holdPosition() = 0;
        /// </summary>
        public static void holdPosition(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.holdPosition, unitID, 0, 0, 0);
        }

        /// <summary> Orders the unit to patrol between its current location and the specified location.
        /// 
        /// // static bool patrol(Position position) = 0;
        /// </summary>
        public static void patrol(this ProxyBot bot, int unitID, int x, int y)
        {
            bot.doCommand(StarCraftCommand.patrol, unitID, x, y, 0);
        }

        /// <summary> Orders a unit to follow a target unit.
        /// 
        /// // static bool follow(Unit* target) = 0;
        /// </summary>
        public static void follow(this ProxyBot bot, int unitID, int targetID)
        {
            bot.doCommand(StarCraftCommand.follow, unitID, targetID, 0, 0);
        }

        /// <summary> Sets the rally location for a building. 
        /// 
        /// // static bool setRallyPosition(Position target) = 0;
        /// </summary>
        public static void setRallyPosition(this ProxyBot bot, int unitID, int x, int y)
        {
            bot.doCommand(StarCraftCommand.setRallyPosition, unitID, x, y, 0);
        }

        /// <summary> Sets the rally location for a building based on the target unit's current position.
        /// 
        /// // static bool setRallyUnit(Unit* target) = 0;
        /// </summary>
        public static void setRallyUnit(this ProxyBot bot, int unitID, int targetID)
        {
            bot.doCommand(StarCraftCommand.setRallyUnit, unitID, targetID, 0, 0);
        }

        /// <summary> Instructs an SCV to repair a target unit.
        /// 
        /// // static bool repair(Unit* target) = 0;
        /// </summary>
        public static void repair(this ProxyBot bot, int unitID, int targetID)
        {
            bot.doCommand(StarCraftCommand.repair, unitID, targetID, 0, 0);
        }

        /// <summary> Orders a zerg unit to morph to a different unit type.
        /// 
        /// // static bool morph(UnitType type) = 0;
        /// </summary>
        public static void morph(this ProxyBot bot, int unitID, int typeID)
        {
            bot.doCommand(StarCraftCommand.morph, unitID, typeID, 0, 0);
        }

        /// <summary> Tells a zerg unit to burrow. Burrow must be upgraded for non-lurker units.
        /// 
        /// // static bool burrow() = 0;
        /// </summary>
        public static void burrow(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.burrow, unitID, 0, 0, 0);
        }

        /// <summary> Tells a burrowed unit to unburrow.
        /// 
        /// // static bool unburrow() = 0;
        /// </summary>
        public static void unburrow(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.unburrow, unitID, 0, 0, 0);
        }

        /// <summary> Orders a siege tank to siege.
        /// 
        /// // static bool siege() = 0;
        /// </summary>
        public static void siege(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.siege, unitID, 0, 0, 0);
        }

        /// <summary> Orders a siege tank to un-siege.
        /// 
        /// // static bool unsiege() = 0;
        /// </summary>
        public static void unsiege(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.unsiege, unitID, 0, 0, 0);
        }

        /// <summary> Tells a unit to cloak. Works for ghost and wraiths. 
        /// 
        /// // static bool cloak() = 0;
        /// </summary>
        public static void cloak(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.cloak, unitID, 0, 0, 0);
        }

        /// <summary> Tells a unit to decloak, works for ghosts and wraiths.
        /// 
        /// // static bool decloak() = 0;
        /// </summary>
        public static void decloak(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.decloak, unitID, 0, 0, 0);
        }

        /// <summary> Commands a Terran building to lift off.
        /// 
        /// // static bool lift() = 0;
        /// </summary>
        public static void lift(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.lift, unitID, 0, 0, 0);
        }

        /// <summary> Commands a terran building to land at the specified location.
        /// 
        /// // static bool land(TilePosition position) = 0;
        /// </summary>
        public static void land(this ProxyBot bot, int unitID, int tx, int ty)
        {
            bot.doCommand(StarCraftCommand.land, unitID, tx, ty, 0);
        }

        /// <summary> Orders the transport unit to load the target unit.
        /// 
        /// // static bool load(Unit* target) = 0;
        /// </summary>
        public static void load(this ProxyBot bot, int unitID, int targetID)
        {
            bot.doCommand(StarCraftCommand.load, unitID, targetID, 0, 0);
        }

        /// <summary> Orders a transport unit to unload the target unit at the current transport location.
        /// 
        /// // static bool unload(Unit* target) = 0;
        /// </summary>
        public static void unload(this ProxyBot bot, int unitID, int targetID)
        {
            bot.doCommand(StarCraftCommand.unload, unitID, targetID, 0, 0);
        }

        /// <summary> Orders a transport to unload all units at the current location.
        /// 
        /// // static bool unloadAll() = 0;
        /// </summary>
        public static void unloadAll(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.unloadAll, unitID, 0, 0, 0);
        }

        /// <summary> Orders a unit to unload all units at the target location.
        /// 
        /// // static bool unloadAll(Position position) = 0;
        /// </summary>
        public static void unloadAll(this ProxyBot bot, int unitID, int x, int y)
        {
            bot.doCommand(StarCraftCommand.unloadAllPosition, unitID, x, y, 0);
        }

        /// <summary> Orders a being to stop being constructed.
        /// 
        /// // static bool cancelConstruction() = 0;
        /// </summary>
        public static void cancelConstruction(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.cancelConstruction, unitID, 0, 0, 0);
        }

        /// <summary> Tells an scv to pause construction on a building.
        /// 
        /// // static bool haltConstruction() = 0;
        /// </summary>
        public static void haltConstruction(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.haltConstruction, unitID, 0, 0, 0);
        }

        /// <summary> Orders a zerg unit to stop morphing.
        /// 
        /// // static bool cancelMorph() = 0;
        /// </summary>
        public static void cancelMorph(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.cancelMorph, unitID, 0, 0, 0);
        }

        /// <summary> Tells a building to remove the last unit from its training queue.
        /// 
        /// // static bool cancelTrain() = 0;
        /// </summary>
        public static void cancelTrain(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.cancelTrain, unitID, 0, 0, 0);
        }

        /// <summary> Tells a building to remove a specific unit from its queue.
        /// 
        /// // static bool cancelTrain(int slot) = 0;
        /// </summary>
        public static void cancelTrain(this ProxyBot bot, int unitID, int slot)
        {
            bot.doCommand(StarCraftCommand.cancelTrainSlot, unitID, slot, 0, 0);
        }

        /// <summary> Orders a Terran building to stop constructing an add on.
        /// 
        /// // static bool cancelAddon() = 0;
        /// </summary>
        public static void cancelAddon(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.cancelAddon, unitID, 0, 0, 0);
        }

        /// <summary> 
        /// Tells a building cancel a research in progress. 
        /// 
        /// // static bool cancelResearch() = 0;
        /// </summary>
        public static void cancelResearch(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.cancelResearch, unitID, 0, 0, 0);
        }

        /// <summary> 
        /// Tells a building cancel an upgrade  in progress. 
        /// 
        /// // static bool cancelUpgrade() = 0;
        /// </summary>
        public static void cancelUpgrade(this ProxyBot bot, int unitID)
        {
            bot.doCommand(StarCraftCommand.cancelUpgrade, unitID, 0, 0, 0);
        }

        /// <summary> Tells the unit to use the specified tech, (i.e. STEM PACKS)
        /// 
        /// // static bool useTech(TechType tech) = 0;
        /// </summary>
        public static void useTech(this ProxyBot bot, int unitID, int techTypeID)
        {
            bot.doCommand(StarCraftCommand.useTech, unitID, techTypeID, 0, 0);
        }

        /// <summary> Tells the unit to use tech at the target location.
        /// 
        /// Note: for AOE spells such as plague.
        /// 
        /// // static bool useTech(TechType tech, Position position) = 0;
        /// </summary>
        public static void useTech(this ProxyBot bot, int unitID, int techTypeID, int x, int y)
        {
            bot.doCommand(StarCraftCommand.useTechPosition, unitID, techTypeID, x, y);
        }

        /// <summary> Tells the unit to use tech on the target unit.
        /// 
        /// Note: for targeted spells such as irradiate.
        /// 
        /// // static bool useTech(TechType tech, Unit* target) = 0;
        /// </summary>
        public static void useTech(this ProxyBot bot, int unitID, int techTypeID, int targetID)
        {
            bot.doCommand(StarCraftCommand.useTechTarget, unitID, techTypeID, targetID, 0);
        }

        /// <summary> Adds a command to the command queue.
        /// 
        /// </summary>
        /// <param name="command">- the command to execture, see the Orders enumeration
        /// </param>
        /// <param name="unitID">- the unit to control
        /// </param>
        /// <param name="arg0">- the first command argument
        /// </param>
        /// <param name="arg1">- the second command argument
        /// </param>
        /// <param name="arg2">- the third command argument
        /// </param>
        private static void doCommand(this ProxyBot bot, StarCraftCommand command, int unitID, int arg0, int arg1, int arg2)
        {
            lock (bot.commandQueue.SyncRoot)
            {
                bot.commandQueue.Add(new Command(command, unitID, arg0, arg1, arg2));
            }
        }
    }
}
