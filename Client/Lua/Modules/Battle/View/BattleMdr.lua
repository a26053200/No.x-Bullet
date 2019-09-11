---
--- Generated by Tools
--- Created by zheng.
--- DateTime: 2019-05-18-23:22:49
---

local BattleBehavior = require("Game.Modules.Battle.Behaviors.BattleBehavior")
local BornArea = require("Game.Modules.Battle.Behaviors.BornWave")
local AttachCamera = require("Game.Modules.Common.Camera.AttachCamera")
local MainHero = require("Game.Modules.Battle.Items.MainHero")
local BaseMediator = require("Game.Core.Ioc.BaseMediator")

---@class Game.Modules.Battle.View.BattleMdr : Game.Core.Ioc.BaseMediator
---@field battleModel Game.Modules.Battle.Model.BattleModel
---@field battleService Game.Modules.Battle.Service.BattleService
---@field mainHero Game.Modules.Battle.Items.MainHero
---@field attachCamera Game.Modules.Common.Camera.AttachCamera
---@field points table<number, UnityEngine.Vector3>
---@field battleBehavior Game.Modules.Battle.Behaviors.BattleBehavior
---@field bornAreas table<number, Game.Modules.Battle.Behaviors.BornArea>
---@field ecsWorld Game.ECSWorld
local BattleMdr = class("BattleMdr",BaseMediator)

function BattleMdr:OnInit()

    World.root = GameObject.New("[World]")
    self.ecsWorld = GameObject.Find("BootStrap"):GetComponent(typeof(Game.ECSWorld))
    self.ecsWorld:Launch()

    --local pointsObj = self.scene.currSubScene:GetRootObjByName("Points")
    --self.points = {}
    --for i = 1, 6 do
    --    self.points[i] = pointsObj:FindChild("p" .. i).transform.position
    --end
    --World.points = self.points
    --local aStarObj = vmgr.scene.currSubScene:GetRootObjByName("A*")
    --World.grid = aStarObj:GetComponent(typeof(AStar.Grid))
    --
    --
    --self.mainHero = MainHero.New(AvatarConfig.Get("TestHero"))
    --self.mainHero.transform.position = self.points[1]
    --World.mainHero = self.mainHero
    --
    --self.attachCamera = AttachCamera.New(Camera.main)
    --self.attachCamera:Attach(self.mainHero.gameObject)
    --
    --local battleInfo = require("Game.Config.Excel.Battle_Test") ---@type BattleInfo
    --
    --self.battleBehavior = BattleBehavior.New(battleInfo, self.gameObject)
    --self.battleBehavior:CreateBattle()
    --
    --self:StartBattle()
end

function BattleMdr:StartBattle()
    --DelayCallback(1, Handler.New(function()
    --    self.mainHero:SetBehaviorEnable(true)
    --end, self))
end

return BattleMdr
