/**
 * AIModule implementation for communicating with a remote java process (ProxyBot).
 *
 * Uses the winsock library for sockets, include "wsock32.lib" in the linker inputs
 *
 * Note: I suck at C++, there is most likely better ways for doing string concatentation, tokenizing, etc.
 *
 * Note: this implementation uses a blocking socket. On each frame, an update message is
 *       sent to the ProxyBot, then the socket waits for a command message from the 
 *		 ProxyBot. The process blocks while waiting for a response, so the ProxyBot
 *		 should immediately respond to updates.
 */
#include "ExampleAIModule.h"
using namespace BWAPI;

#include <winsock.h>
#include <stdio.h>
#include <sstream>
#include <string>

#pragma comment(lib, "ws2_32.lib")
#pragma comment(lib, "Netapi32.lib")

/** port to connect to on the java side */
#define PORTNUM 13337

/** socket identifier */
int proxyBotSocket;

/** mapping of unit objects to a unique ID, sent is sent to the java process */
std::map<Unit*, int> unitMap;
std::map<int, Unit*> unitIDMap;
std::map<int, UnitType> typeMap;
std::map<int, TechType> techMap;
std::map<int, UpgradeType> upgradeMap;

/** used to assign unit object IDs */
int unitIDCounter = 1;

/** show output */
bool log = true;

// function prototypes
int initSocket();
std::string toString(int value);
std::string toString(bool value);
void handleCommand(int command, int unitID, int arg0, int arg1, int arg2);

/**
 * Called at the start of a match. 
 *
 * Establishes a connection with the proxybot and sends the following messages:
 * - An initial ACK
 * - Data about the unit types
 * - Data about the starting locations
 * - Map tile information
 * - Tech types
 * - Upgrade types
 */
void ExampleAIModule::onStart()
{

  // TODO: only init if not connected
  proxyBotSocket = initSocket();

  // say hello to the java app
  if (proxyBotSocket == -1) {
	  Broodwar->sendText("Proxy Bot connection FAILURE!");
  }
  else {

	  // send the player and enemy IDs
	  std::string ack("What's up Proxy Bot");
	  ack += ":";
	  ack += toString(Broodwar->self()->getID());
	  ack += ":";
	  ack += toString(Broodwar->self()->getRace().getID());
	  ack += ":";
	  ack += toString(Broodwar->enemy()->getID());	
	  ack += ":";
	  ack += toString(Broodwar->enemy()->getRace().getID());	
	  ack += "\n";

	  send(proxyBotSocket,(char*)ack.c_str(), ack.size(), 0);
	  Broodwar->sendText("Connected to Proxy Bot!");
  }

  // Wait for bot options
  char buf[1024];
  int numBytes = recv(proxyBotSocket , buf , 1024 , 0);

  // cheats
  if (buf[0] == '1') Broodwar->enableFlag(Flag::UserInput);
  if (buf[1] == '1') Broodwar->enableFlag(Flag::CompleteMapInformation); // Note: Fog of War remains

  // unit type data
  std::string unitTypes("UnitTypes");

  std::set<UnitType> types = UnitTypes::allUnitTypes();
  for(std::set<UnitType>::iterator i=types.begin();i!=types.end();i++)
  {
	  int id = i->getID();
	  std::string race = i->getRace().getName();
	  std::string name = i->getName();
	  int minerals = i->mineralPrice();
	  int gas = i->gasPrice();
	  int hitPoints = i->maxHitPoints()/256;
	  int shields = i->maxShields();
	  int energy = i->maxEnergy();
	  int buildTime = i->buildTime();
	  bool canAttack = i->canAttack();
	  bool canMove = i->canMove();
	  int width = i->tileWidth();
	  int height = i->tileHeight();
	  int supplyRequired = i->supplyRequired();
	  int supplyProvided = i->supplyProvided();
	  int sightRange = i->sightRange();
	  int groundMaxRange = i->groundWeapon()->maxRange();
	  int groundMinRange = i->groundWeapon()->minRange();
	  int groundDamage = i->groundWeapon()->damageAmount();
	  int airRange = i->airWeapon()->maxRange();
	  int airDamage = i->airWeapon()->damageAmount();
	  bool isBuilding = i->isBuilding();
	  bool isFlyer = i->isFlyer();
	  bool isSpellCaster = i->isSpellcaster();
	  bool isWorker = i->isWorker();
	  int whatBuilds = i->whatBuilds().first->getID();

	  typeMap[id] = (*i);

	  // encode the type as a string
	  unitTypes += ":";
	  unitTypes += toString(id);
	  unitTypes += ";";
	  unitTypes += race;
	  unitTypes += ";";
	  unitTypes += name;
	  unitTypes += ";";
	  unitTypes += toString(minerals);
	  unitTypes += ";";
	  unitTypes += toString(gas);
	  unitTypes += ";";
	  unitTypes += toString(hitPoints);
	  unitTypes += ";";
	  unitTypes += toString(shields);
	  unitTypes += ";";
	  unitTypes += toString(energy);
	  unitTypes += ";";
	  unitTypes += toString(buildTime);
	  unitTypes += ";";
	  unitTypes += toString(canAttack);
	  unitTypes += ";";
	  unitTypes += toString(canMove);
	  unitTypes += ";";
	  unitTypes += toString(width);
	  unitTypes += ";";
	  unitTypes += toString(height);
	  unitTypes += ";";
	  unitTypes += toString(supplyRequired);
	  unitTypes += ";";
	  unitTypes += toString(supplyProvided);
	  unitTypes += ";";
	  unitTypes += toString(sightRange);
	  unitTypes += ";";
	  unitTypes += toString(groundMaxRange);
	  unitTypes += ";";
	  unitTypes += toString(groundMinRange);
	  unitTypes += ";";
	  unitTypes += toString(groundDamage);
	  unitTypes += ";";
	  unitTypes += toString(airRange);
	  unitTypes += ";";
	  unitTypes += toString(airDamage);
	  unitTypes += ";";
	  unitTypes += toString(isBuilding);
	  unitTypes += ";";
	  unitTypes += toString(isFlyer);
	  unitTypes += ";";
	  unitTypes += toString(isSpellCaster);
	  unitTypes += ";";
	  unitTypes += toString(isWorker);
	  unitTypes += ";";
	  unitTypes += toString(whatBuilds);
  }

  unitTypes += "\n";
  char *utBuf = (char*)unitTypes.c_str();
  send(proxyBotSocket, utBuf, unitTypes.size(), 0);

  // starting locations
  std::string locations("Locations");

  std::set<TilePosition> startSpots = Broodwar->getStartLocations();
  for(std::set<TilePosition>::iterator i=startSpots.begin();i!=startSpots.end();i++)
  {
	  int x = i->x();
	  int y = i->y();

	  locations += ":";
	  locations += toString(x);
	  locations += ";";
	  locations += toString(y);
  }

  locations += "\n";
  char *slBuf = (char*)locations.c_str();
  send(proxyBotSocket, slBuf, locations.size(), 0);

  // Get the map data
  std::string mapName = Broodwar->mapName();
  int mapWidth = Broodwar->mapWidth();
  int mapHeight = Broodwar->mapHeight();

  std::string mapData(mapName); 

  mapData += ":";
  mapData += toString(mapWidth);
  mapData += ":";
  mapData += toString(mapHeight);
  mapData += ":";

  for (int y=0; y<mapHeight; y++) {	
	  for (int x=0; x<mapWidth; x++) {

	    // char 0: hieght
		int h = Broodwar->groundHeight(4*x, 4*y);
		mapData += toString(h);

		// char 1: buildable
		if (Broodwar->buildable(x, y)) {
			mapData += toString(1);
		}
		else { 
			mapData += toString(0);
		}

		// char 2: walkable
		if (Broodwar->walkable(4*x, 4*y)) {
			mapData += toString(1);		
		}
		else {
            mapData += toString(0);
		}
	  }
  }

  mapData += "\n";
  char *sbuf = (char*)mapData.c_str();
  send(proxyBotSocket, sbuf, mapData.size(), 0);

  // tech types
  std::string techTypes("TechTypes"); 

  std::set<TechType> tektypes = TechTypes::allTechTypes();
  for(std::set<TechType>::iterator i=tektypes.begin();i!=tektypes.end();i++)
  {
	  int id = i->getID();
	  std::string name = i->getName();
	  int whatResearchesID = i->whatResearches()->getID(); // unit type id of what researches it
	  int mins = i->mineralPrice();
	  int gas = i->gasPrice();
	  
	  techMap[id] = (*i);

	  techTypes += ":";
	  techTypes += toString(id);
	  techTypes += ";";
	  techTypes += name;
	  techTypes += ";";
	  techTypes += toString(whatResearchesID);
	  techTypes += ";";
	  techTypes += toString(mins);
	  techTypes += ";";
	  techTypes += toString(gas);
  }

  techTypes += "\n";
  char *ttbuf = (char*)techTypes.c_str();
  send(proxyBotSocket, ttbuf, techTypes.size(), 0);

    // upgrade types
  std::string upgradeTypes("UpgradeTypes"); 

  std::set<UpgradeType> upTypes = UpgradeTypes::allUpgradeTypes();
  for(std::set<UpgradeType>::iterator i=upTypes.begin();i!=upTypes.end();i++)
  {
	  int id = i->getID();
	  std::string name = i->getName();
	  int whatUpgradesID = i->whatUpgrades()->getID(); // unit type id of what researches it
	  int repeats = i->maxRepeats();
	  int minBase = i->mineralPriceBase();
	  int minFactor = i->mineralPriceFactor();
	  int gasBase = i->gasPriceFactor();
	  int gasFactor = i->gasPriceFactor();
	  
	  upgradeMap[id] = (*i);

	  upgradeTypes += ":";
	  upgradeTypes += toString(id);
	  upgradeTypes += ";";
	  upgradeTypes += name;
	  upgradeTypes += ";";
	  upgradeTypes += toString(whatUpgradesID);
	  upgradeTypes += ";";
	  upgradeTypes += toString(repeats);
	  upgradeTypes += ";";
	  upgradeTypes += toString(minBase);
	  upgradeTypes += ";";
	  upgradeTypes += toString(minFactor);
	  upgradeTypes += ";";
	  upgradeTypes += toString(gasBase);
	  upgradeTypes += ";";
	  upgradeTypes += toString(gasFactor);
  }

  upgradeTypes += "\n";
  char *utbuf = (char*)upgradeTypes.c_str();
  send(proxyBotSocket, utbuf, upgradeTypes.size(), 0);
}

/**
 * Returns the unit based on the unit ID
 */
Unit* getUnit(int unitID)
{
	return unitIDMap[unitID];
}

/** 
 * Returns the unit type from its identifier
 */
UnitType getUnitType(int type) 
{
	return typeMap[type];
}

/** 
 * Returns the tech type from its identifier
 */
TechType getTechType(int type) 
{
	return techMap[type];
}

/** 
 * Returns the upgrade type from its identifier
 */
UpgradeType getUpgradeType(int type)
{
	return upgradeMap[type];
}

/**
 * Utility function for constructing a Position.
 *
 * Note: positions are in pixel coordinates, while the inputs are given in tile coordinates
 */
Position getPosition(int x, int y)
{
	return BWAPI::Position(32*x, 32*y);
}

/**
 * Utility function for constructing a TilePosition.
 *
 * Note: not sure if this is correct, is there a way to get a tile position
 *       object from the api rather than create a new one?
 */
TilePosition getTilePosition(int x, int y)
{
	return BWAPI::TilePosition(x, y);
}

/**
 * Runs every frame 
 *
 * Sends the unit status to the ProxyBot, then waits for a command message.
 */
void ExampleAIModule::onFrame()
{
	// check if the Proxy Bot is connected
	if (proxyBotSocket == -1) {
		return;
	}
 
	// dont run every frame
	if (Broodwar->getFrameCount()%3 != 0) {
		return;
	}

	// figure out what units and upgrades the bot can produce
	bool unitProduction[230];
	for (int i=0; i<230; i++) unitProduction[i] = false;

	std::set<Unit*> selfUnits = Broodwar->self()->getUnits();
	std::set<UnitType> types = UnitTypes::allUnitTypes();
	for(std::set<UnitType>::iterator i=types.begin();i!=types.end();i++)
	{
		for(std::set<Unit*>::iterator j=selfUnits.begin();j!=selfUnits.end();j++)
		{
			if (Broodwar->canMake((*j), (*i))) {
				unitProduction[(*i).getID()] = true;
				break;
			}
		}
	}

	bool upgradeProduction[63];
	for (int i=0; i<63; i++) upgradeProduction[i] = false;

	std::set<UpgradeType> upTypes = UpgradeTypes::allUpgradeTypes();
	for(std::set<UpgradeType>::iterator i=upTypes.begin();i!=upTypes.end();i++)
	{
		for(std::set<Unit*>::iterator j=selfUnits.begin();j!=selfUnits.end();j++)
		{
			if (Broodwar->canUpgrade((*j), (*i))) {
				upgradeProduction[(*i).getID()] = true;
				break;
			}
		}
	}

	bool techProduction[47];
	for (int i=0; i<47; i++) techProduction[i] = false;

	std::set<TechType> tektypes = TechTypes::allTechTypes();
	for(std::set<TechType>::iterator i=tektypes.begin();i!=tektypes.end();i++)
	{
		for(std::set<Unit*>::iterator j=selfUnits.begin();j!=selfUnits.end();j++)
		{
			if (Broodwar->canResearch((*j), (*i))) {
				techProduction[(*i).getID()] = true;
				break;
			}
		}
	}

	std::string canProduce("");
	for (int i=0; i<230; i++) {
		if (unitProduction[i]) canProduce += "1";
		else canProduce += "0";
	}

	std::string canUpgrade("");
	for (int i=0; i<63; i++) {
		if (upgradeProduction[i]) canUpgrade += "1";
		else canUpgrade += "0";
	}

	std::string canTech("");
	for (int i=0; i<47; i++) {
		if (techProduction[i]) canTech += "1";
		else canTech += "0";
	}

	// send the unit status's to the Proxy Bot
	std::string status("status");
	std::set<Unit*> myUnits = Broodwar->getAllUnits();

	// also send current resources
	int minerals = Broodwar->self()->minerals();
	int gas = Broodwar->self()->gas();
	int supplyUsed = Broodwar->self()->supplyUsed();
	int supplyTotal = Broodwar->self()->supplyTotal();

	status += ";";
	status += toString(minerals);
	status += ";";
	status += toString(gas);
	status += ";";
	status += toString(supplyUsed);
	status += ";";
	status += toString(supplyTotal);
	status += ";";
	status += canProduce;
	status += ";";
	status += canTech;
	status += ";";
	status += canUpgrade;

	for(std::set<Unit*>::iterator i=myUnits.begin();i!=myUnits.end();i++)
	{
		// get the unit ID
		int unitID = unitMap[*i];
		if (unitID == 0) {

			// assign an ID if there is not one currently associated with the unit
			unitID = unitIDCounter++; 

			unitMap[*i] = unitID;
			unitIDMap[unitID] = *i;
		}

		status += ":";
		status += toString(unitID);
		status += ";";
		status += toString((*i)->getPlayer()->getID());
		status += ";";
		status += toString((*i)->getType().getID());
		status += ";";
		status += toString((*i)->getPosition().x()/32);
		status += ";";
		status += toString((*i)->getPosition().y()/32);
		status += ";";
		status += toString((*i)->getHitPoints()/256);
		status += ";";
		status += toString((*i)->getShields()/256);
		status += ";";
		status += toString((*i)->getEnergy()/256);
		status += ";";
		status += toString((*i)->getOrderTimer());
		status += ";";
		status += toString((*i)->getOrder().getID());
		status += ";";
		status += toString((*i)->getResources());
	} 

	status += "\n";
	char *sbuf = (char*)status.c_str();
	send(proxyBotSocket, sbuf, status.size(), 0);
	
	// process commands
	char buf[1024];
	int numBytes = recv(proxyBotSocket , buf , 1024 , 0);

	char *message = new char[numBytes + 1];
	message[numBytes] = 0;
	for (int i=0; i<numBytes; i++) 
	{
		message[i] = buf[i];
	}

	// tokenize the commands
	char* token = strtok(message, ":");
	token = strtok(NULL, ":");			// eat the command part of the message

    int commandCount = 0;
    char* commands[100];

	while (token != NULL) 
	{
		commands[commandCount] = token;
		commandCount++;
		token = strtok(NULL, ":");
	}

	// tokenize the arguments
	for (int i=0; i<commandCount; i++) 
	{
		char* command = strtok(commands[i], ";");
		char* unitID = strtok(NULL, ";");
		char* arg0 = strtok(NULL, ";");
		char* arg1 = strtok(NULL, ";");
		char* arg2 = strtok(NULL, ";");

		handleCommand(atoi(command), atoi(unitID), atoi(arg0), atoi(arg1), atoi(arg2));
	}
}

void handleCommand(int command, int unitID, int arg0, int arg1, int arg2)
{
	if (command == 41) {
		Broodwar->sendText("Set game speed: %d", unitID);
		Broodwar->setLocalSpeed(unitID);
		return;
	}	

	// check that the unit ID is valid
	Unit* unit = unitIDMap[unitID];
	if (unit == NULL) {
		Broodwar->sendText("Issued command to invalid unit ID: %d", unitID);
		return;
	}

	// execute the command
	switch (command) {

	    // virtual bool attackMove(Position position) = 0;
		case 1:
			if (log) Broodwar->sendText("Unit:%d attackMove(%d, %d)",unitID, arg0, arg1);
			unit->attackMove(getPosition(arg0, arg1));
			break;
		// virtual bool attackUnit(Unit* target) = 0;
		case 2:
			if (getUnit(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d attackUnit(%d)", unitID, arg0);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d attackUnit(%d)", unitID, arg0);
				unit->attackUnit(getUnit(arg0));
			}
			break;
		// virtual bool rightClick(Position position) = 0;
		case 3:
			if (log) Broodwar->sendText("Unit:%d rightClick(%d, %d)",unitID, arg0, arg1);
			unit->rightClick(getPosition(arg0, arg1));
			break;
		// virtual bool rightClick(Unit* target) = 0;
		case 4:
			if (getUnit(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d rightClick(%d)", unitID, arg0);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d rightClick(%d)", unitID, arg0);
				unit->rightClick(getUnit(arg0));
			}
			break;
		// virtual bool train(UnitType type) = 0;
		case 5:
			if (getUnitType(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d train(%d)", unitID, arg0);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d train(%d)", unitID, arg0);
				unit->train(getUnitType(arg0));
			}
			break;
		// virtual bool build(TilePosition position, UnitType type) = 0;
		case 6:
			if (getUnitType(arg2) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d build(%d, %d, %d)", unitID, arg0, arg1, arg2);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d build(%d, %d, %d)", unitID, arg0, arg1, arg2);
				unit->build(getTilePosition(arg0, arg1), getUnitType(arg2));
			}
			break;
		// virtual bool buildAddon(UnitType type) = 0;
		case 7:
			if (getUnitType(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d buildAddon(%d)", unitID, arg0);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d buildAddon(%d)", unitID, arg0);
				unit->buildAddon(getUnitType(arg0));
			}
			break;
		// virtual bool research(TechType tech) = 0;
		case 8:
			if (getTechType(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d research(%d)", unitID, arg0);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d research(%d)", unitID, arg0);
				unit->research(getTechType(arg0));
			}
			break;
		// virtual bool upgrade(UpgradeType upgrade) = 0;
		case 9:
			if (getUpgradeType(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d upgrade(%d)", unitID, arg0);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d upgrade(%d)", unitID, arg0);
				unit->upgrade(getUpgradeType(arg0));
			}
			break;
		// virtual bool stop() = 0;
		case 10:
			if (log) Broodwar->sendText("Unit:%d stop()", unitID);
			unit->stop();
			break;
		// virtual bool holdPosition() = 0;
		case 11:
			if (log) Broodwar->sendText("Unit:%d holdPosition()", unitID);
			unit->holdPosition();
			break;
		// virtual bool patrol(Position position) = 0;
		case 12:
			if (log) Broodwar->sendText("Unit:%d patrol(%d, %d)", unitID, arg0, arg1);
			unit->patrol(getPosition(arg0, arg1));
			break;
		// virtual bool follow(Unit* target) = 0;
		case 13:
			if (getUnit(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d follow(%d)", unitID, arg0);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d follow(%d)", unitID, arg0);
				unit->follow(getUnit(arg0));
			}
			break;
		// virtual bool setRallyPosition(Position target) = 0;
		case 14:
			if (log) Broodwar->sendText("Unit:%d setRallyPosition(%d, %d)", unitID, arg0, arg1);
			unit->setRallyPosition(getPosition(arg0, arg1));
			break;
		// virtual bool setRallyUnit(Unit* target) = 0;
		case 15:
			if (getUnit(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d setRallyUnit(%d)", unitID, arg0);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d setRallyUnit(%d)", unitID, arg0);
				unit->setRallyUnit(getUnit(arg0));
			}
			break;
		// virtual bool repair(Unit* target) = 0;
		case 16:
			if (getUnit(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d repair(%d)", unitID, arg0);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d repair(%d)", unitID, arg0);
				unit->repair(getUnit(arg0));
			}
			break;
		// virtual bool morph(UnitType type) = 0;
		case 17:
			if (getUnitType(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d morph(%d)", unitID, arg0);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d morph(%d)", unitID, arg0);
				unit->morph(getUnitType(arg0));
			}
			break;
		// virtual bool burrow() = 0;
		case 18:
			if (log) Broodwar->sendText("Unit:%d burrow()", unitID);
			unit->burrow();
			break;
		// virtual bool unburrow() = 0;
		case 19:
			if (log) Broodwar->sendText("Unit:%d unburrow()", unitID);
			unit->unburrow();
			break;
		// virtual bool siege() = 0;
		case 20:
			if (log) Broodwar->sendText("Unit:%d siege()", unitID);
			unit->siege();
			break;
		// virtual bool unsiege() = 0;
		case 21:
			if (log) Broodwar->sendText("Unit:%d unsiege()", unitID);
			unit->unsiege();
			break;
		// virtual bool cloak() = 0;
		case 22:
			if (log) Broodwar->sendText("Unit:%d cloak()", unitID);
			unit->cloak();
			break;
		// virtual bool decloak() = 0;
		case 23:
			if (log) Broodwar->sendText("Unit:%d decloak()", unitID);
			unit->decloak();
			break;
		// virtual bool lift() = 0;
		case 24:
			if (log) Broodwar->sendText("Unit:%d lift()", unitID);
			unit->lift();
			break;
		// virtual bool land(TilePosition position) = 0;
		case 25:
			if (log) Broodwar->sendText("Unit:%d land(%d, %d)", unitID, arg0, arg1);
			unit->land(getTilePosition(arg0, arg1));
			break;
		// virtual bool load(Unit* target) = 0;
		case 26:
			if (getUnit(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d load(%d)", unitID, arg0);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d load(%d)", unitID, arg0);
				unit->load(getUnit(arg0));
			}
			break;
		// virtual bool unload(Unit* target) = 0;
		case 27:
			if (getUnit(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d unload(%d)", unitID, arg0);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d unload(%d)", unitID, arg0);
				unit->unload(getUnit(arg0));
			}
			break;
		// virtual bool unloadAll() = 0;
		case 28:
			if (log) Broodwar->sendText("Unit:%d unloadAll()", unitID);
			unit->unloadAll();
			break;
		// virtual bool unloadAll(Position position) = 0;
		case 29:
			if (log) Broodwar->sendText("Unit:%d unloadAll(%d, %d)", unitID, arg0, arg1);
			unit->unloadAll(getPosition(arg0, arg1));
			break;
		// virtual bool cancelConstruction() = 0;
		case 30:
			if (log) Broodwar->sendText("Unit:%d cancelConstruction()", unitID);
			unit->cancelConstruction();
			break;
		// virtual bool haltConstruction() = 0;
		case 31:
			if (log) Broodwar->sendText("Unit:%d haltConstruction()", unitID);
			unit->haltConstruction();
			break;
		// virtual bool cancelMorph() = 0;
		case 32:
			if (log) Broodwar->sendText("Unit:%d cancelMorph()", unitID);
			unit->cancelMorph();
			break;
		// virtual bool cancelTrain() = 0;
		case 33:
			if (log) Broodwar->sendText("Unit:%d cancelTrain()", unitID);
			unit->cancelTrain();
			break;
		// virtual bool cancelTrain(int slot) = 0;
		case 34:
			if (log) Broodwar->sendText("Unit:%d cancelTrain(%d)", unitID, arg0);
			unit->cancelTrain(arg0);
			break;
		// virtual bool cancelAddon() = 0;
		case 35:
			if (log) Broodwar->sendText("Unit:%d cancelAddon()", unitID);
			unit->cancelAddon();
			break;
		// virtual bool cancelResearch() = 0;
		case 36:
			if (log) Broodwar->sendText("Unit:%d cancelResearch()", unitID);
			unit->cancelResearch();
			break;
		// virtual bool cancelUpgrade() = 0;
		case 37:
			if (log) Broodwar->sendText("Unit:%d cancelUpgrade()", unitID);
			unit->cancelUpgrade();
			break;
		// virtual bool useTech(TechType tech) = 0;
		case 38:
			if (getTechType(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d useTech(%d)", unitID, arg0);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d useTech(%d)", unitID, arg0);
				unit->useTech(getTechType(arg0));
			}
			break;
		// virtual bool useTech(TechType tech, Position position) = 0;
		case 39:
			if (getTechType(arg0) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d useTech(%d, %d, %d)", unitID, arg0, arg1, arg2);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d useTech(%d, %d, %d)", unitID, arg0, arg1, arg2);
				unit->useTech(getTechType(arg0), getPosition(arg1, arg2));
			}
			break;
		// virtual bool useTech(TechType tech, Unit* target) = 0;
		case 40:
			if (getTechType(arg0) == NULL || getUnit(arg1) == NULL) {
				Broodwar->sendText("Invalid Command, Unit:%d useTech(%d, %d)", unitID, arg0, arg1);
			}
			else {
				if (log) Broodwar->sendText("Unit:%d useTech(%d, %d)", unitID, arg0, arg1);
				unit->useTech(getTechType(arg0), getUnit(arg1));
			}
			break;
		default:
			break;
	}
}

/**
 * Cleans up the unitID maps.
 */
void ExampleAIModule::onRemove(BWAPI::Unit* unit)
{
	int key = unitMap.erase(unit);
	unitIDMap.erase(key);
}

bool ExampleAIModule::onSendText(std::string text)
{
  return true;
}
void ExampleAIModule::onUnitCreate(BWAPI::Unit* unit)
{
  if (!Broodwar->isReplay())
    Broodwar->printf("A %s [%x] has been created at (%d,%d)",unit->getType().getName().c_str(),unit,unit->getPosition().x(),unit->getPosition().y());
  else
  {
    /*if we are in a replay, then we will print out the build order
    (just of the buildings, not the units).*/
    if (unit->getType().isBuilding() && unit->getPlayer()->isNeutral()==false)
    {
      int seconds=Broodwar->getFrameCount()/24;
      int minutes=seconds/60;
      seconds%=60;
      Broodwar->printf("%.2d:%.2d: %s creates a %s",minutes,seconds,unit->getPlayer()->getName().c_str(),unit->getType().getName().c_str());
    }
  }
}
void ExampleAIModule::onUnitDestroy(BWAPI::Unit* unit)
{
  if (!Broodwar->isReplay())
    Broodwar->printf("A %s [%x] has been destroyed at (%d,%d)",unit->getType().getName().c_str(),unit,unit->getPosition().x(),unit->getPosition().y());
}
void ExampleAIModule::onUnitMorph(BWAPI::Unit* unit)
{
  if (!Broodwar->isReplay())
    Broodwar->printf("A %s [%x] has been morphed at (%d,%d)",unit->getType().getName().c_str(),unit,unit->getPosition().x(),unit->getPosition().y());
  else
  {
    /*if we are in a replay, then we will print out the build order
    (just of the buildings, not the units).*/
    if (unit->getType().isBuilding() && unit->getPlayer()->isNeutral()==false)
    {
      int seconds=Broodwar->getFrameCount()/24;
      int minutes=seconds/60;
      seconds%=60;
      Broodwar->printf("%.2d:%.2d: %s morphs a %s",minutes,seconds,unit->getPlayer()->getName().c_str(),unit->getType().getName().c_str());
    }
  }
}
void ExampleAIModule::onUnitShow(BWAPI::Unit* unit)
{
  if (!Broodwar->isReplay())
    Broodwar->printf("A %s [%x] has been spotted at (%d,%d)",unit->getType().getName().c_str(),unit,unit->getPosition().x(),unit->getPosition().y());
}
void ExampleAIModule::onUnitHide(BWAPI::Unit* unit)
{
  if (!Broodwar->isReplay())
    Broodwar->printf("A %s [%x] was last seen at (%d,%d)",unit->getType().getName().c_str(),unit,unit->getPosition().x(),unit->getPosition().y());
}

/**
 * Establishes a connection with the ProxyBot.
 *
 * Returns -1 if the connection fails
 */
int initSocket() 
{
      int sockfd;
      int size;
      struct hostent *h;
      struct sockaddr_in client_addr;
      char myname[256];
      WORD wVersionRequested;
      WSADATA wsaData;

      wVersionRequested = MAKEWORD( 1, 1 );
      WSAStartup( wVersionRequested, &wsaData );
      gethostname(myname, 256);      
      h=gethostbyname(myname);

      size = sizeof(client_addr);
      memset(&client_addr , 0 , sizeof(struct sockaddr_in));
      memcpy((char *)&client_addr.sin_addr , h -> h_addr ,h -> h_length);
     
	  client_addr.sin_family = AF_INET;
      client_addr.sin_port = htons(PORTNUM);
      client_addr.sin_addr =  *((struct in_addr*) h->h_addr) ;
      if ((sockfd = socket(AF_INET , SOCK_STREAM , 0)) == -1){
		  return -1;
      }

      if ((connect(sockfd , (struct sockaddr *)&client_addr , sizeof(client_addr))) == -1){
		  return -1;
	  }

	  return sockfd;
}

/**
 * Utiliity function for int to string conversion.
 */
std::string toString(int value) 
{
	std::stringstream ss;
	ss << value;
	return ss.str();
}

/**
 * Utiliity function for bool to string conversion.
 */
std::string toString(bool value) 
{
	if (value) return std::string("1");
	else return std::string("0");
}