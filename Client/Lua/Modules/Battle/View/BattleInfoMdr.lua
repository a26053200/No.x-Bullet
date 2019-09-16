---
--- Generated by Tools
--- Created by zheng.
--- DateTime: 2019-09-17-00:16:13
---

local BaseMediator = require("Game.Core.Ioc.BaseMediator")
---@class Game.Modules.Battle.View.BattleInfoMdr : Game.Core.Ioc.BaseMediator
---@field battleModel Game.Modules.Battle.Model.BattleModel
---@field battleService Game.Modules.Battle.Service.BattleService
---@field scoreText UnityEngine.UI.Text
local BattleInfoMdr = class("BattleInfoMdr",BaseMediator)

function BattleInfoMdr:OnInit()
    self.scoreText = self.gameObject:GetText("Score/Text")
end


function BattleInfoMdr:RegisterListeners()

end

function BattleInfoMdr:Update()
    self.scoreText.text = Game.ECSWorld.Instance.score .. ""
end

return BattleInfoMdr
