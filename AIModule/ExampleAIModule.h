#pragma once
#include <BWAPI.h>
#include <BWTA.h>
class ExampleAIModule : public BWAPI::AIModule
{
public:
  virtual void onStart();
  virtual void onFrame();
  virtual void onRemove(BWAPI::Unit* unit);
  virtual bool onSendText(std::string text);
  virtual void ExampleAIModule::onUnitShow(BWAPI::Unit* unit);
  virtual void ExampleAIModule::onUnitHide(BWAPI::Unit* unit);
  virtual void ExampleAIModule::onUnitMorph(BWAPI::Unit* unit);
  virtual void ExampleAIModule::onUnitDestroy(BWAPI::Unit* unit);
  virtual void ExampleAIModule::onUnitCreate(BWAPI::Unit* unit);

/*
  void showStats(); //not part of BWAPI::AIModule
  void showPlayers();
  void showForces();
  BWTA::Region* home;
  BWTA::Region* enemy_base;
  bool analyzed;
  std::map<BWAPI::Unit*,BWAPI::UnitType> buildings;
*/
};