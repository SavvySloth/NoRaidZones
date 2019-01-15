using System.Collections.Generic;
using Oxide.Core;
using UnityEngine;

namespace Oxide.Plugins {
    [Info ("NoRaidZones", "Uncle Myers", "2.0.1")]
    // Version 2.0.2 (ItemV2 patch) got updated by MrBlue, Thanks a lot!
    public class NoRaidZones : HurtworldPlugin {
        private Dictionary<int, string> protectedcells = new Dictionary<int, string> ();
        private List<string> blocked = new List<string>();

        void Loaded () {
            LoadCells ();
            blocked = Config.Get<List<string>> ("BlockedGuids");
            permission.RegisterPermission ("NoRaidZones.Admin", this);

            if ((bool) Config["FillStakes"]) {
                FillAmber ();
                timer.Repeat (14400f, 0, () => { FillAmber (); });
            }
        }

        protected override void LoadDefaultConfig () {
            if (Config["BlockedGuids"] == null) Config.Set ("BlockedGuids", new List<string> (new string[] { "e0ed5b104ae770a4ebe12a30576e6385", "972cbc350a69a14419b7c06a3baaa090" }));
            if (Config["FillStakes"] == null) Config.Set ("FillStakes", true);
            SaveConfig ();
        }

        protected override void LoadDefaultMessages () {
            var messages = new Dictionary<string, string> { 
                { "NRZUsage", "<color=orange>[NRZ]</color> > Usage: /NRZ add|remove|check." },
                    { "NRZAdded", "<color=orange>[NRZ]</color> > No Raid Zone added on this cell." },
                    { "HasNRZ", "<color=orange>[NRZ]</color> > This cell is a No Raid Zone." },
                    { "NRZExists", "<color=orange>[NRZ]</color> > No Raid Zone already exists in this cell." },
                    { "NRZRemoved", "<color=orange>[NRZ]</color> > No Raid Zone removed from this cell." },
                    { "NoStake", "<color=orange>[NRZ]</color> > You are not authorized on a stake in this cell." },
                    { "NoNRZ", "<color=orange>[NRZ]</color> > No No Raid Zone set in this cell." },
                    { "NRZAlert", "<color=orange>NO RAID ZONE!</color>" },
                    { "NoPermission", "<color=orange>[NRZ]</color> > You do not have the permissions to use this command. (Perm: {perm})" }
                };
            lang.RegisterMessages (messages, this);
        }

        string Msg (string msg, string SteamId = null) => lang.GetMessage (msg, this, SteamId);

        void FillAmber () {
            foreach (KeyValuePair<int, string> nrz in protectedcells) {
                OwnershipStakeServer stake = GetOwnerStake (StringToVector3 (nrz.Value));
                if (stake != null && !stake.IsDestroying) {
                    var stakeStorage = stake.GetComponent<Inventory> ();
                    if (stakeStorage == null) continue;
                    var amber = stakeStorage.GetSlot (0);
                    amber.StackSize = stakeStorage.GetSlotConf (0).StackRestriction;
                    amber.InvalidateStack ();
                }
            }
        }

        object OnEntityDeploy (EquipEventData data) {
            ItemObject item = data.Session.RootItem;
            if (item == null) return null;

            var guid = RuntimeHurtDB.Instance.GetGuid (item.Generator);
            if (guid == null) return null;

            if (blocked.Contains (guid)) {
                Vector3 pos = data.Session.Handler.RefCache.PlayerConstructionManager.ServerConstructionPrefab.Mover.GetGameObject ().transform.position;
                var session = data.Session.Handler.RefCache.InteractServer.OwnerIdentity.ConnectedSession;
                int cell = ConstructionUtilities.GetOwnershipCell (pos);
                if (protectedcells.ContainsKey (cell)) {
                    AlertManager.Instance.GenericTextNotificationServer (Msg ("NRZAlert", session.SteamId.ToString ()), session.Player);
                    return true;
                }
            }
            
            return null;
        }

        #region DataHandling
        void LoadCells () {
            protectedcells = Interface.GetMod ().DataFileSystem.ReadObject<Dictionary<int, string>> ("NoRaidZones");
        }
        void SaveCells () {
            Interface.Oxide.DataFileSystem.WriteObject ("NoRaidZones", protectedcells);
        }
        #endregion

        #region Helpers
        public OwnershipStakeServer GetOwnerStake (Vector3 vector3) => ConstructionManager.Instance.GetOwnerStake (vector3);

        bool HasStake (PlayerSession session) {
            OwnershipStakeServer stake = GetOwnerStake (session.WorldPlayerEntity.transform.position);
            if (stake == null)
                return false;
            if (!stake.AuthorizedPlayers.Contains (session.Identity))
                return false;
            return true;
        }

        public static Vector3 StringToVector3 (string sVector) {
            if (sVector.StartsWith ("(") && sVector.EndsWith (")"))
                sVector = sVector.Substring (1, sVector.Length - 2);
            string[] sArray = sVector.Split (',');
            Vector3 result = new Vector3 (float.Parse (sArray[0]), float.Parse (sArray[1]), float.Parse (sArray[2]));
            return result;
        }
        #endregion

        #region ChatCommand
        [ChatCommand ("nrz")]
        void nrzCommand (PlayerSession session, string command, string[] args) {
            if (!permission.UserHasPermission (session.SteamId.ToString (), "NoRaidZones.admin")) {
                Player.Message (session, Msg ("NoPermission", session.SteamId.ToString ()).Replace ("{perm}", "NoRaidZones.admin"));
                return;
            }

            Vector3 pos = session.WorldPlayerEntity.transform.position;
            int cell = ConstructionUtilities.GetOwnershipCell (pos);
            if (args.Length == 1) {
                switch (args[0]) {
                    case "add":
                        if (protectedcells.ContainsKey (cell))
                            Player.Message (session, Msg ("NRZExists", session.SteamId.ToString ()));
                        else if (!HasStake (session))
                            Player.Message (session, Msg ("NoStake", session.SteamId.ToString ()));
                        else {
                            protectedcells.Add (cell, GetOwnerStake (pos).gameObject.transform.position.ToString ());
                            Player.Message (session, Msg ("NRZAdded", session.SteamId.ToString ()));
                            SaveCells ();
                        }
                        break;

                    case "remove":
                        if (!protectedcells.ContainsKey (cell))
                            Player.Message (session, Msg ("NoNRZ", session.SteamId.ToString ()));
                        else {
                            protectedcells.Remove (cell);
                            Player.Message (session, Msg ("NRZRemoved", session.SteamId.ToString ()));
                            SaveCells();
                        }
                        break;
                    case "check":
                        if (protectedcells.ContainsKey (cell))
                            Player.Message (session, Msg ("HasNRZ", session.SteamId.ToString ()));
                        else
                            Player.Message (session, Msg ("NoNRZ", session.SteamId.ToString ()));
                        break;
                    default:
                        Player.Message (session, Msg ("NRZUsage", session.SteamId.ToString ()));
                        break;
                }
                return;
            } else {
                Player.Message (session, Msg ("NRZUsage", session.SteamId.ToString ()));
                return;
            }
        }
        #endregion
    }
}