using System.Collections.Generic;
using UnityEngine;
using Diego;
using System.Runtime.CompilerServices;
using System;
public static class ItemsUniverse
{
    private static readonly Dictionary<int, Diego.Item> _data = new Dictionary<int, Diego.Item>();
    private static readonly Dictionary<int, Diego.Item> _required = new Dictionary<int, Item>(); 
    // runs at the start of the program
    static ItemsUniverse()
    {
        AddItem(
            10000,
            "whiskey",
            "Jack Daniel's Single-Barrel Tennessee Whiskey",
            "It escaped the supplies promptly; A sneaky father seems to have broken his vows.",
            ItemQuality.Useless
        );
        AddItem(
            10001,
            "teddy_bear",
            "Kuma",
            "A gentle reminder of childhood innocence, yet with no place here in times like these."
        );
        AddItem(
            10002,
            "flashlight",
            "Flashlight",
            "Amidst the destruction, the flashlight serves its valiant purpose well in recovering your essentials.",
            ItemQuality.Required
        );
        AddItem(
            10003,
            "canned_tuna",
            "Canned Tuna",
            "A classic snack enjoyed thoroughly by the father. He gingerly pries off the tin lid, and serves it to his children.",
            ItemQuality.Required
        );
        AddItem(
            10004,
            "first_aid_kit",
            "First Aid Kit",
            "Your daughter is injured... You address it with this First Aid Kit.",
            ItemQuality.Required
        );
        AddItem(
            10005,
            "keys",
            "Keys",
            "You aren't going anywhere... Your car is buried in rubble.",
            ItemQuality.Useless
        );
        AddItem(
            10006,
            "crowbar",
            "Crowbar",
            "The right man in the wrong place can make all the difference in the world.",
            ItemQuality.Required
        );
        AddItem(
            10007,
            "flare",
            "Flare",
            "The flare sparks a crimson red in the dead of the dark. You find your way around easily.",
            ItemQuality.Required
        );
        AddItem(
            10008,
            "plank",
            "Plank",
            "With one useless plank you hold comes hundreds more from the collapse of your home.",
            ItemQuality.Useless
        );
        AddItem(
            10009,
            "wrench",
            "Pipe Wrench",
            "Would you kindly toss that wrench back into the rubble? You're not the town plumber!",
            ItemQuality.Useless
        );
        AddItem(
            10010,
            "gauze",
            "Gauze",
            "Any wounds that come flying at you are stopped by a thick gauze wrapping.",
            ItemQuality.Useless
        );
        AddItem(
            10011,
            "long_pillow",
            "Suspiciously Long Pillow",
            "Shrek is love, Shrek is life.",
            ItemQuality.Useless
        );
        AddItem(
            10012,
            "fire_extinguisher",
            "Fire Extinguisher",
            "A fire immolates the debris. You fend it off with this handy Fire Extinguisher.",
            ItemQuality.Required
        );
        AddItem(
            10013,
            "radio",
            "Portable Radio",
            "The radio keeps the family updated on all happenings through the aftermath of the quake.",
            ItemQuality.Required         
        );
        AddItem(
            10014,
            "map",
            "Town Map",
            "The map helps you navigate through the destruction of the city to the evacuation point.",
            ItemQuality.Required
        );
        AddItem(
            10015,
            "cash",
            "Cash",
            "You'll need your cash to live through these trying times.",
            ItemQuality.Required
        );
        AddItem(
            10016,
            "home_documents",
            "House Documents",
            "These documents are proof of your home ownership and mortgage payments.",
            ItemQuality.Required
        );
        AddItem(
            10017,
            "sleeping_bag",
            "Sleeping Bag",
            "An uncomfortable but essential place to sleep in the night",
            ItemQuality.Required
        );
        AddItem(
            10018,
            "can_opener",
            "Can Opener",
            "It won't be too uncommon to find canned sardines around here.",
            ItemQuality.Required
        );
        AddItem(
            10019,
            "batteries",
            "Spare Batteries",
            "It's a good idea to keep your electronics charged.",
            ItemQuality.Required
        );
        AddItem(
            10020,
            "manifesto",
            "My Manifesto",
            "What- Why would you... Like, what is that, why would you write something like that?",
            ItemQuality.Useless
        );
        AddItem(
            10021,
            "burnout_revenge",
            "Burnout Revenge for the PS2",
            "There's no weed to smoke, no diet soda to fill your bellies with, no hanging out to do.",
            ItemQuality.Useless
        );
        AddItem(
            10022,
            "rubber_duck",
            "Rubber Duck",
            "Why?",
            ItemQuality.Useless
        );
        AddItem(
            10023,
            "pocket_fan",
            "Pocket Fan",
            "The flimsy pocket fan blows 1 kt of air at your forehead.",
            ItemQuality.Useless
        );
        AddItem(
            10024,
            "steak",
            "Steak",
            "There's now way to keep the steak from spoiling.",
            ItemQuality.Useless
        );
        AddItem(
            10025,
            "medicine",
            "Medicine",
            "Your wife has a health condition. Lucky of you to bring the medicine.",
            ItemQuality.Required
        );
    }

    public static bool TryGetValue(int id, out Diego.Item item)
    {
        bool result = _data.TryGetValue(id, out Item _item);
        item = _item;
        return result;
    }

    // Passive-aggressively ask the C# compiler to inline the function
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AddItem(int id, string _path, string _name, string _remarks, ItemQuality quality=ItemQuality.Useless)
    {
        _data.Add(id, Item.CreateWithSpritePath(
            id,
            _path,
            _name,
            _remarks
        ));
    }
}
