**No Raid Zones** allows you to define zones where players can't use C4 or Drills.

## Usage

Simply stand in the cell (a square which is surrounded by the blue stripes) you want to protect and type `/nrz add`. No Raid Zones will automatically create a cluster of all connected cells which are owned by you and protect them all.

## Permissions

* `noraidzones.admin` -- Allows player to utilize plugin features

## Chat Commands

* `/nrz ` -- Lists all available commands
* `/nrz add <cell>` -- Apply protection to an cell.
* `/nrz remove <cell>` -- Disband protection on a cell.
* `/nrz check ` -- Check if an cells is protected or not.

## Configuration

**BlockedGuids** -- Blocked guiids by default C4 and RaidingDrill

**FillStakes** -- Automatically fill stakes in protected cells.

```json
{
  "BlockedGuids": [
    "e0ed5b104ae770a4ebe12a30576e6385",
    "972cbc350a69a14419b7c06a3baaa090"
  ],
  "FillStakes": true
}
```

## Localization

```json
{
  "NRZUsage": "<color=orange>[NRZ]</color> > Usage: /NRZ add|remove|check.",
  "NRZAdded": "<color=orange>[NRZ]</color> > No Raid Zone added on this cell.",
  "HasNRZ": "<color=orange>[NRZ]</color> > This cell is a No Raid Zone.",
  "NRZExists": "<color=orange>[NRZ]</color> > No Raid Zone already exists in this cell.",
  "NRZRemoved": "<color=orange>[NRZ]</color> > No Raid Zone removed from this cell.",
  "NoStake": "<color=orange>[NRZ]</color> > You are not authorized on a stake in this cell.",
  "NoNRZ": "<color=orange>[NRZ]</color> > No No Raid Zone set in this cell.",
  "NRZAlert": "<color=orange>NO RAID ZONE!</color>",
  "NoPermission": "<color=orange>[NRZ]</color> > You do not have the permissions to use this command. (Perm: {perm})"
}
```

## Planed Features

* No raiding during defined times/dates
* Invisible wall (Players can't enter any raid zone if they aren't written in the stake), don't know if this is going to be possible but I hope so! :)

## F.A.Q.

**Does this plugin still works for legacy?**  
No, all patches after version 1.3.0 won't work on legacy servers anymore. 

## Credits

- **Lizzaran**, the original author of this plugin
- **Swat1801**, for helping maintain the plugin previously
- **Mr. Blue**, for the lightweight V2 rewrite (version 2.0.1)