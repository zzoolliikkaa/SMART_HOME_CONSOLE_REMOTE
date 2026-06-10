using Smart_Home_Console_Remote;
using Smart_Home_Console_Remote.Models.Interfaces;
using Smart_Home_Console_Remote.Services;

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
            ListDevices();
            break;
        case MainMenuOptions.AddDevice:
            AddDevice();
            break;
        case MainMenuOptions.TogglePower:
            TogglePower();
            break;
        case MainMenuOptions.DeviceActions:
            DeviceActions();
            break;
        case MainMenuOptions.SelfTestAll:
            SelfTestAll();
            break;
        case MainMenuOptions.Exit:
            Console.WriteLine("Exiting...");
            break;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}

void ListDevices()
{
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
            if (device is IDimmable dimmable)
            {
                Console.WriteLine($", Brightness: {dimmable.Brightness}");
            }
            else if (device is ITemperatureControl temperatureControl)
            {
                Console.WriteLine($", Target Temperature: {temperatureControl.TargetCelsius}");
            }
            else if (device is IMeasurableLoad measurableLoad)
            {
                Console.WriteLine($", Current Load: {measurableLoad.CurrentWatts} W, Total Energy: {measurableLoad.TotalWh} Wh");
            }
        }
        Console.ReadLine();
    }

}

void AddDevice()
{
    while (DeviceMenuOptions.BackToMainMenu != (DeviceMenuOptions)choice)
    {
        DeviceMenu.AddCaption("ADD DEVICE");
        DeviceMenu.DisplayMenu();
        choice = DeviceMenu.ReadChoice();
        DeviceMenuOptions DeviceMenSelected = (DeviceMenuOptions)choice;
        switch (DeviceMenSelected)
        {
            case DeviceMenuOptions.LightBulb:
                DeviceList.Add(new LightBulb());
                AddDeviceInfo();
                break;
            case DeviceMenuOptions.Thermostat:
                DeviceList.Add(new Thermostat());
                AddDeviceInfo(); break;
            case DeviceMenuOptions.SmartPlug:
                DeviceList.Add(new SmartPlug());
                AddDeviceInfo(); break;
            case DeviceMenuOptions.BackToMainMenu:
                Console.WriteLine("Returning to main menu...");
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                Console.ReadLine();
                break;
        }
    }

}
void AddDeviceInfo()
{
    DeviceList.Last().Id = DeviceList.Count;
    DeviceList.Last().Name = DeviceList.Last().ReadDeviceInfo();
}
SmartDevice ReadDeviceID()
{
    int deviceId = int.TryParse(Console.ReadLine(), out int result) ? result : 0;
    var device = DeviceList.FirstOrDefault(d => d.Id == deviceId);
    return device;
}
void TogglePower()
{
    Console.Write("Enter device ID to toggle power:");
    SmartDevice device = ReadDeviceID();
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
void DeviceActions()
{
    Console.Write("Enter device ID to Device actions:");
    SmartDevice device = ReadDeviceID();
    if (device != null)
    {
        if (0 == device.GetStatus())
        {
            Console.WriteLine("Device is off. Please turn it on to update.");
        }
        else
        {
            if (device is IDimmable dimmable)
            {

                Console.Write("Enter the value of the brightness (0–100):");
                int brightness = int.TryParse(Console.ReadLine(), out int br) ? br : 0;
                if (brightness < 0 || brightness > 100)
                {
                    Console.WriteLine("Brightness must be between 0 and 100.");
                }
                else
                {
                    dimmable.SetBrightness(brightness);
                    Console.WriteLine($"Device {device.Name} brightness set to {brightness}.");
                }


            }
            else if (device is ITemperatureControl temperatureControl)
            {
                Console.Write("Enter the value of the target temperature (10–30):");
                double temp = double.TryParse(Console.ReadLine(), out double t) ? t : 0;
                if (temp < 10 || temp > 30)
                {
                    Console.WriteLine("Temperature must be between 10 and 30.");
                }
                else
                {
                    temperatureControl.SetTarget(temp);
                    Console.WriteLine($"Device {device.Name} target temperature set to {temp}.");
                }

            }
            else if (device is IMeasurableLoad measurableLoad)
            {
                measurableLoad.UpdateEnergy(1000, 2);
                Console.WriteLine($"Total power consumtion: {measurableLoad.TotalWh} Wh");
                Console.WriteLine($"Current power consumtion: {measurableLoad.CurrentWatts} W");
                Console.Write("Do you want to RESET the Total energy (y/n) ? : ");
                bool totalReset = Console.ReadLine()?.Trim().ToLower() == "y";
                if (totalReset)
                {
                    measurableLoad.ResetEnergy();
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
void SelfTestAll()
{
    if (DeviceList.Count == 0)
    {
        Console.WriteLine("Device not found.");
    }
    else
    {
        Console.WriteLine("Performing self-test on all devices...");
        foreach (var device in DeviceList)
        {
            Console.WriteLine($"ID: {device.Id}, Name: {device.Name}, Type: {device.GetType().Name}, Self-test status: {device.SelfTest()}");
        }
    }
    Console.ReadLine();
}
