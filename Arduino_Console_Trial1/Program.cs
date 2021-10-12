using System;
using Solid.Arduino;
using Solid.Arduino.Firmata;

/* Programmer: Craciun Mihai
 * Date: 10.05.2021
 * Purpose: Arduino Live Console
*/

namespace Arduino_Console_Trial1
{
    class Program
    {
        static void Main(string[] args)
        {
            ISerialConnection connection = GetConnection();
            if (connection == null)
                Console.WriteLine("Connection Falied. Make sure devide is powerd on and connected.");
            else
            {
                string ConsoleInput;
                using (var session = new ArduinoSession(connection))
                {
                OP: Console.Write("AC>");
                    ConsoleInput = Console.ReadLine();
                    switch(ConsoleInput)
                    {
                        case "ConInf":
                            ConectionInfo(session,connection);
                            goto OP;
                        case "Dwrite":
                            int PinNo;
                            bool PinVal;
                            Console.WriteLine("PinNo:");
                            PinNo = int.Parse(Console.ReadLine());
                            Console.WriteLine("PinVal:");
                            PinVal = bool.Parse(Console.ReadLine());
                            Dwrite(session,PinNo,PinVal);
                            goto OP;
                        case "Exit":
                            break;
                        case "Aread":
                            int PinANo;
                            Console.WriteLine("Analog Pin No:");
                            PinANo = int.Parse(Console.ReadLine());
                            ARead(session,PinANo);
                            goto OP;
                    }
                }
            }
        }
        private static ISerialConnection GetConnection()
        {
            Console.WriteLine("Searching for devices...");
            ISerialConnection connection = EnhancedSerialConnection.Find();
            if (connection == null)
                Console.WriteLine("Connection Falied. Make sure devide is powerd on and connected.");
            else
                Console.WriteLine("Connection Suceeded For more info do conn info.");
            return connection;
        }
        private static void ConectionInfo(IFirmataProtocol session,ISerialConnection connection)
        {
            if (connection != null)
            {
                Console.WriteLine($"Device connected to port {connection.PortName} at {connection.BaudRate} baud.");
                var firmware = session.GetFirmware();
                Console.WriteLine($"Firmware Version: {firmware.Name} version {firmware.MajorVersion}.{firmware.MinorVersion}");
                var protocolVersion = session.GetProtocolVersion();
                Console.WriteLine($"Firmata protocol version: {protocolVersion.Major}.{protocolVersion.Minor}");
            }
            else Console.WriteLine("Connection Fail");
        }
        private static void Dwrite(IFirmataProtocol session,int PinNo,bool PinVal)
        {
            // Made for Arduino Uno R3 (Change if needed)
            if (PinNo > 13)
            {
                Console.WriteLine("Invalid PinNo");
            }
            else if (PinVal != true & PinVal != false)
            {
                Console.WriteLine("Inavlid PinVal");
            }
            else
            {
                session.SetDigitalPinMode(PinNo, PinMode.DigitalOutput);
                session.SetDigitalPin(PinNo,PinVal);
                Console.WriteLine($"Pin {PinNo} has been set to {PinVal} value.");
            }
        }
        private static void ARead(IFirmataProtocol session,int PinANo)
        {
            if (PinANo < 13) Console.WriteLine("Invalid Pin Number");
            else if (PinANo > 19) Console.WriteLine("Invalid Pin Number");
            else
            {
                session.SetDigitalPinMode(PinANo, PinMode.AnalogInput);
                PinState Aval = session.GetPinState(PinANo);
                Console.WriteLine(Aval);
            }
        }
    }
}
