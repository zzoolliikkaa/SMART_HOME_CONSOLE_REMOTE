using Smart_Home_Console_Remote;

Menu MainMenu = new Menu();
Menu DeviceMenu = new Menu();
List<SmartDevice> DeviceList = new List<SmartDevice>();
int choice = 0;

MainMenu.AddCaption("SMART HOME CONSOLE REMOTE");
MainMenu.AddOption("List devices");
MainMenu.AddOption("Add device");
MainMenu.AddOption("Toggle power");
MainMenu.AddOption("Device actions");
MainMenu.AddOption("Self-test all");
MainMenu.AddOption("Exit");

DeviceMenu.AddOption("Light bulb");
DeviceMenu.AddOption("Thermostat");
DeviceMenu.AddOption("Smart plug");
DeviceMenu.AddOption("Back to main menu");

while (MainMenuOptions.Exit != (MainMenuOptions)choice)
{
    MainMenu.DisplayMenu();
    choice = MainMenu.ReadChoice();
    MainMenuOptions MainMenuSelected = (MainMenuOptions)choice;
    switch (MainMenuSelected)
    {
        case MainMenuOptions.ListDevices:
            if (DeviceList.Count == 0)
            {
                Console.WriteLine("No devices found.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Listing devices...");
                foreach (var device in DeviceList)
                {
                    Console.Write($"ID: {device.Id}, Name: {device.Name}, Type: {device.GetType().Name}, Status: {device.GetStatus()}");
                    if (device.GetType().Name == "LightBulbs")
                    {
                        Console.WriteLine($", Brightness: {((LightBulbs)device).Brightness}");
                    }
                    else if (device.GetType().Name == "Thermostat")
                    {
                        Console.WriteLine($", Target Temperature: {((Thermostat)device).TargetCelsius}");
                    }
                    else if (device.GetType().Name == "SmartPlug")
                    {
                        Console.WriteLine($", Current Load: {((SmartPlug)device).GetCurrentLoad()} W, Total Energy: {((SmartPlug)device).GetTotalEnergy()} Wh");
                    }
                }
                Console.ReadLine();
            }
            break;
        case MainMenuOptions.AddDevice:
            while (DeviceMenuOptions.BackToMainMenu != (DeviceMenuOptions)choice)
            {
                DeviceMenu.AddCaption("ADD DEVICE");
                DeviceMenu.DisplayMenu();
                choice = DeviceMenu.ReadChoice();
                DeviceMenuOptions DeviceMenSelected = (DeviceMenuOptions)choice;
                switch (DeviceMenSelected)
                {
                    case DeviceMenuOptions.LightBulb:
                        DeviceList.Add(new LightBulbs());
                        DeviceList.Last().Id = DeviceList.Count;
                        DeviceList.Last().Name = DeviceList.Last().ReadDeviceInfo();
                        break;
                    case DeviceMenuOptions.Thermostat:
                        DeviceList.Add(new Thermostat());
                        DeviceList.Last().Id = DeviceList.Count;
                        DeviceList.Last().Name = DeviceList.Last().ReadDeviceInfo();
                        break;
                    case DeviceMenuOptions.SmartPlug:
                        DeviceList.Add(new SmartPlug());
                        DeviceList.Last().Id = DeviceList.Count;
                        DeviceList.Last().Name = DeviceList.Last().ReadDeviceInfo();
                        break;
                    case DeviceMenuOptions.BackToMainMenu:
                        Console.WriteLine("Returning to main menu...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadLine();
                        break;
                }
            }
            break;
        case MainMenuOptions.TogglePower:
            {
                Console.Write("Enter device ID to toggle power:");
                int deviceId = int.TryParse(Console.ReadLine(), out int result) ? result : 0;
                var device = DeviceList.FirstOrDefault(d => d.Id == deviceId);
                if (device != null)
                {
                    if (device.powerStatus)
                    {
                        device.PowerOff();
                        Console.WriteLine($"Device {device.Name} powered off.");
                    }
                    else
                    {
                        device.PowerOn();
                        Console.WriteLine($"Device {device.Name} powered on.");
                    }
                }
                else
                {
                    Console.WriteLine("Device not found.");
                }
                Console.ReadLine();
            }
            break;
        case MainMenuOptions.DeviceActions:
            {
                Console.Write("Enter device ID to Device actions:");
                int deviceId = int.TryParse(Console.ReadLine(), out int result) ? result : 0;
                var device = DeviceList.FirstOrDefault(d => d.Id == deviceId);
                if (device != null)
                {
                    if (0 == device.GetStatus())
                    {
                        Console.WriteLine("Device is off. Please turn it on to update.");
                    }
                    else
                    {
                        if (device.GetType().Name == "LightBulbs")
                        {

                            Console.Write("Enter the value of the brightness (0–100):");
                            int brightness = int.TryParse(Console.ReadLine(), out int br) ? br : 0;
                            if (brightness < 0 || brightness > 100)
                            {
                                Console.WriteLine("Brightness must be between 0 and 100.");
                            }
                            else
                            {
                                ((LightBulbs)device).SetBrightness(brightness);
                                Console.WriteLine($"Device {device.Name} brightness set to {brightness}.");
                            }


                        }
                        else if (device.GetType().Name == "Thermostat")
                        {
                            Console.Write("Enter the value of the target temperature (10–30):");
                            double temp = double.TryParse(Console.ReadLine(), out double t) ? t : 0;
                            if (temp < 10 || temp > 30)
                            {
                                Console.WriteLine("Temperature must be between 10 and 30.");
                            }
                            else
                            {
                                ((Thermostat)device).SetTarget(temp);
                                Console.WriteLine($"Device {device.Name} target temperature set to {temp}.");
                            }

                        }
                        else if (device.GetType().Name == "SmartPlug")
                        {
                            ((SmartPlug)DeviceList.Last()).UpdateEnergy(1000, 2);
                            Console.WriteLine($"Total power consumtion: {((SmartPlug)device).GetTotalEnergy()} Wh");
                            Console.WriteLine($"Current power consumtion: {((SmartPlug)device).GetCurrentLoad()} W");
                            Console.Write("Do you want to RESET the Total energy (y/n) ? : ");
                            bool totalReset = Console.ReadLine()?.Trim().ToLower() == "y";
                            if (totalReset)
                            {
                                ((SmartPlug)device).ResetEnergy();
                            }

                        }
                    }
                }
                else
                {
                    Console.WriteLine("Device not found.");
                }
                Console.ReadLine();
            }
            break;
        case MainMenuOptions.SelfTestAll:
            {
                if (DeviceList.Count == 0)
                {
                    Console.WriteLine("No devices found.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Performing self-test on all devices...");
                    foreach (var device in DeviceList)
                    {
                        Console.WriteLine($"ID: {device.Id}, Name: {device.Name}, Type: {device.GetType().Name}, Self-test status: {device.SelfTest()}");
                    }
                    Console.ReadLine();
                }
            }
            break;
        case MainMenuOptions.Exit:
            Console.WriteLine("Exiting...");
            break;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
    //Console.ReadLine();
}

enum MainMenuOptions
{
    ListDevices = 1,
    AddDevice,
    TogglePower,
    DeviceActions,
    SelfTestAll,
    Exit
}
enum DeviceMenuOptions
{
    LightBulb = 1,
    Thermostat,
    SmartPlug,
    BackToMainMenu
}
