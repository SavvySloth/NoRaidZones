**No Raid Zones** allows you to define zones where players can't use C4 or Drills.

## Usage

Simply stand in the cell (a place which is surrounded by the blue dots) you want to protect and type `/nrz add Example`. No Raid Zones will automatically create a cluster of all connected cells which are owned by you and protect them all.

## Permissions

* `noraidzones.use` -- Allows player to utilize plugin features

## Chat Commands

* `/nrz add <zone>` -- Adds a new zone
* `/nrz remove <zone>` -- Removes the specific zone
* `/nrz update <zone>` -- Updates the specific zone. This should be used if you add or remove stakes in a zone
* `/nrz list -- `Shows all your zones
* `/nrz help` -- Lists all available commands

## Configuration

**Add Extra Cell Layer** -- Add surrounding cells without a stake to the zone for extra protection.

**Auto Fill Stakes** -- Automatically fill all the stakes in the zone.

**Refund C4** -- Give back the C4 to the player which got automatically destroyed.

**Show Notification** -- Shows a notification to players which enter the zone.

```json
{
  "Settings": {
    "Add Extra Cell Layer": true,
    "Auto Fill Stakes": true,
    "Refund C4": false,
    "Show Notification": true
  }
}
```

## Localization

```json
{
  "Misc - No Permission": "You don't have the permission to use this command.",
  "Misc - Help": "Unknown command, type '{name}' to list all available commands.",
  "Misc - Syntax": "Syntax: {syntax}",
  "Misc - Commands": "Commands: \n{commands}",
  "Zone - Not Found": "Couldn't find the cell.",
  "Zone - Not Owned": "You don't own this cell.",
  "Zone - Already": "This cell is already part of another zone.",
  "Zone - Added": "You have created the zone '{name}'. Its {count} cells big.",
  "Zone - Removed": "You have removed the zone '{name}'.",
  "Zone - Unknown": "The zone '{name}' couldn't be found.",
  "Zone - List": "Zones:\n{names}",
  "Zone - Exists": "You already have a zone named '{name}'.",
  "Zone - None": "You don't have any zones yet.",
  "Zone - Updated": "You have updated the zone '{name}'. Its now {count} cells big.",
  "Zone - Raid": "The player '{player}' tried to raid '{name}'.",
  "Zone - Notification": "No Raid Zone",
  "Zone - Auto Removed": "You don't own this cell for '{name}' anymore and therefore got removed."
}
```

## Planed Features

* Anti-dynamite
* No raiding during defined times/dates
* No raiding stake (no need to be written in the stake optional)
* Invisible wall (Players can't enter any raid zone if they aren't written in the stake), don't know if this is going to be possible but I hope so! :)

## Credits

- **Lizzaran**, the original author of this plugin
- **Swat1801**, for helping maintain the plugin previously