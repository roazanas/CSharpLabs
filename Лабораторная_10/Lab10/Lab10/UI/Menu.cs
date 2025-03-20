using System;

namespace Lab10.UI
{
    public class Menu
    {
        public static uint TabLevel = 0;
        public Menu? PastMenu { get; set; }
        public Menu? CurrentMenu { get; set; }
        public readonly List<MenuItem> menuItems;
        public Action ShowInfo = () => { };

        public Menu()
        {
            menuItems = new List<MenuItem>();
        }

        public Menu(List<MenuItem> menuItems)
        {
            this.menuItems = menuItems;
        }

        public void AddMenuItem(int itemNumber, string itemName, Action action, bool isSubmenu = false)
        {
            menuItems.Add(new MenuItem(itemNumber, itemName, action, isSubmenu));
        }

        public void DisplayMenu()
        {
            ShowInfo.Invoke();
            foreach (var item in menuItems)
            {
                string colorTag = item.IsBlocked ? "<DGRAY>" : "<WHITE>";
                if (item.ItemNumber != 0)
                {
                    ColorWriteManager.HandleColorStr($"<MAGENTA>{item.ItemNumber}{colorTag}. {item.ItemName}\n", TabLevel);
                }
                else
                {
                    ColorWriteManager.HandleColorStr($"<MAGENTA>{item.ItemNumber}{colorTag}. <DRED>{item.ItemName}\n", TabLevel);
                }
            }
            Console.WriteLine();
        }

        public int GetUserChoice()
        {
            while (true)
            {
                ColorWriteManager.HandleColorStr($"<WHITE>Выберите пункт меню: ");
                int choice = UserIO.ReadUserIntInArray(menuItems.Select(item => item.ItemNumber).ToArray());
                
                var selectedItem = menuItems.Find(item => item.ItemNumber == choice);
                if (selectedItem != null && selectedItem.IsBlocked)
                {
                    ColorWriteManager.HandleColorStr($"<WARNING>На данный момент пункт <MAGENTA>{choice} <WARNING>не доступен<RESET>\n");
                    continue;
                }
                
                return choice;
            }
        }

        public void GoToSubmenuOrAction(int choice)
        {
            var menuItem = menuItems.Find(item => item.ItemNumber == choice);
            if (menuItem != null)
            {
                TabLevel++;
                menuItem.Action.Invoke();
                if (!menuItem.IsSubmenu)
                {
                    TabLevel--;
                    Console.WriteLine();
                }
            }
        }

        public void BlockMenuItem(int itemNumber)
        {
            var item = menuItems.Find(x => x.ItemNumber == itemNumber);
            if (item != null)
            {
                item.IsBlocked = true;
            }
        }

        public void UnblockMenuItem(int itemNumber)
        {
            var item = menuItems.Find(x => x.ItemNumber == itemNumber);
            if (item != null)
            {
                item.IsBlocked = false;
            }
        }
    }

    public class MenuItem
    {
        public int ItemNumber { get; set; }
        public string ItemName { get; set; }
        public Action Action { get; set; }
        public bool IsSubmenu { get; set; }
        public bool IsBlocked { get; set; }

        public MenuItem(int itemNumber, string itemName, Action action, bool isSubmenu)
        {
            ItemNumber = itemNumber;
            ItemName = itemName;
            Action = action;
            IsSubmenu = isSubmenu;
            IsBlocked = false;
        }
    }
}
