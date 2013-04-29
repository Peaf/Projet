using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project
{
    public class Inventaire
    {
        public Item[] tablObjects;
        public int place_object;
        public bool added = false;

        public Inventaire()
        {
            this.tablObjects = new Item[60];
        }
        public void Initialize()
        {
            for (int i = 0; i < tablObjects.Length; i++)
            {
                tablObjects[i] = new Item("","rien","",0, 1);
                tablObjects[i].place = i;
            }
        }

        public Item getItem(string s)
        {
            int i = 0;
            while (i < tablObjects.Length && tablObjects[i].name != s)
            {
                i++;
            }
            return tablObjects[i];
        }


        public void addItem(Item toAdd)
        {
            int i = 0;
            while (i < tablObjects.Length && tablObjects[i].name != "rien" && tablObjects[i].name != toAdd.name)
            {
                i++;
            }
            if(tablObjects[i].name == toAdd.name)
            tablObjects[i].total += toAdd.total;

            else if (i < tablObjects.Length)
            {
                toAdd.place = i;
                tablObjects[i] = toAdd;
                added = true;
            }

        }


        public void useItem(Item item)
        {
            switch (item.type)
            {
                case "Potion":
                    if (item.effect == "health")
                        Game1.player.health += item.stat;
                    if (item.effect == "mana")
                        Game1.player.mana += item.stat;
                    break;

                case "Weapon":
                    Game1.player.Degat += item.stat;
                    break;
                case "Armor":
                    Game1.player.Armor += item.stat;
                    break;
            }
            if (item.total > 1)
                item.total--;
            else removeItem(item);

        }



        public void removeItem(Item toRemove)
        {
            int i = 0;
            while (i < tablObjects.Length && tablObjects[i].name != toRemove.name)
            {
                i++;
            }
            if (i < tablObjects.Length)
            {
                tablObjects[i].type = "";
                tablObjects[i].name = "rien";
            }
        }
    }
}