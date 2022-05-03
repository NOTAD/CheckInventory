using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;


namespace Plugin
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override string Author => "Notad";

        public override string Description => "Show Inv";

        public override string Name => "Check Bag";

        public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        public Plugin(Main game) : base(game)
        {
        }

        public override void Initialize()
        {

            Commands.ChatCommands.Add(new Command(
                //permissions: new List<string> { },
                cmd: this.CheckBag,
                "balo", "bag", "bb")
            {
                HelpText = "Check other players' backpacks. Aliases: bag or bb"
            });
        }

        private void CheckBag(CommandArgs args)
        {
            TSPlayer target = null;
            Item[] inventory = null;
            TSPlayer player = args.Player;
            //Item[] inventory = player.TPlayer.inventory;
            int argsCount = args.Parameters.Count;
            string str = "";

            if (argsCount != 0 && argsCount != 1)
            {
                args.Player.SendErrorMessage($"Error [c/55D284:/bag] [c/55D284:<player>]");
            }
            else if (argsCount == 0)
            {
                target = args.Player;
            }
            else if (argsCount == 1)
            {
                var players = TSPlayer.FindByNameOrID(args.Parameters[0]);
                if (players.Count == 0)
                {
                    args.Player.SendErrorMessage("The player does not exist!");
                }
                else if (players.Count > 1)
                {
                    args.Player.SendMultipleMatchError(players.Select(p => p.Name));
                }
                else
                {
                    target = players[0];
                }
            }

            inventory = target.TPlayer.inventory;
            str = $"{target.Name}" + "Blackpack：" + "\r\n";

            for (int i = 0; i < NetItem.InventorySlots; i++)
            {
                
                if (inventory[i] == null || inventory[i].netID == 0)
                {
                    str += "     ";
                    if (i == 9 || i == 19 || i == 29 || i == 39 || i == 49) { str += "  " + "\r\n"; }
                    
                    continue;
                }
                if (i == 9 || i == 19 || i == 29 || i == 39 || i == 49) { str += ItemTag(inventory[i]) + "\r\n"; }
                else
                {
                    str += ItemTag(inventory[i]) + "  ";
                }
            }
            
            if (argsCount == 0)
            {
                TShock.Utils.Broadcast(str + "\r\n", Microsoft.Xna.Framework.Color.Green);
            }
            else
            {
                args.Player.SendSuccessMessage(str + "\r\n");
                
            }

        }
        public string ItemTag(Item item)
        {
            int netID = item.netID;
            int stack = item.stack;
            int prefix = item.prefix;
            string options = stack > 1 ? "/s" + stack : prefix != 0 ? "/p" + prefix : "";
            return String.Format("[i{0}:{1}]", options, netID);
        }

    }
}