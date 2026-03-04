# Beamable Battle Pass Example

The `battle-pass-example` repository contains a Unity project demonstrating a very simple battle pass implementation using Beamable services.

## Batlle Pass Content
In the Beamable Content Manager, you can find the content for this example called `batllepass` and it contains the 
following content item: `Season1Battlepass`

A battle pass content item contains the following elements which you can extent to your own needs:
- `Name`: The name of the battle pass
- `End Date`: The date the battle pass ends
- `Tiers`: A list of tiers that can be earned
  - `Level`: The level of the tier
  - `Rewards`: A list of rewards that can be earned for this tier
    - `Reward`: The content id of the reward
    - `Quantity`: The quantity of the reward

    
## BattlePass Microservice
The `BattlePassService` only has one method for now which you can extend to your own needs.
The method `IsBattlepassValid` returns true if the battle pass is valid within the EndDate and false otherwise.


