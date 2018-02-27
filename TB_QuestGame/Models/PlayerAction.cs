using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB_QuestGame
{
    /// <summary>
    /// enum of all possible player actions
    /// </summary>
    public enum PlayerAction
    {
        None,
        MissionSetup,
        RandomName,
        LookAround,
        LookAt,
        PickUpItem,
        PickUpTreasure,
        PutDownItem,
        PutDownTreasure,
        Travel,
        PlayerInfo,
        EditPlayerInfo,
        PlayerInventory,
        PlayerTreasure,
        LocationsVisited,
        ListDestinations,
        ListItems,
        ListTreasures,
        Return,
        Exit
    }
}
