using System;
using System.Collections.Generic;

namespace Valeo.Bot.Services.ValeoKeyboards
{
    public struct ValeoCommands
    {
        public string OriginalValue { get; private set; }
        public string Value { get; private set; }
        public RequestType RequestType { get; private set; }

        private ValeoCommands(string value)
        {
            OriginalValue = value;
            string[] parts = value.Split("|");

            if (parts.Length < 0 || parts.Length > 2)
            {
                throw new ArgumentException("Invalid command. Must be of 1-2 parts.");
            }

            if (parts.Length == 1)
            {
                RequestType = RequestType.Menu;
                Value = value;
            }
            else
            {
                Enum.TryParse(parts[1], out RequestType type);
                RequestType = type;
                Value = parts[0];
            }
        }
        public ValeoCommands(string value, RequestType requestType)
        {
            OriginalValue = "";
            RequestType = requestType;
            Value = value;
        }

        public static ValeoCommands Doctors { get { return new ValeoCommands("doctors", RequestType.Menu); } }
        public static ValeoCommands Back { get { return new ValeoCommands("back", RequestType.Menu); } }
        public static ValeoCommands Default { get { return new ValeoCommands("default", RequestType.Menu); } }
        public static ValeoCommands Usi { get { return new ValeoCommands("usi", RequestType.Menu); } }
        public static ValeoCommands Contacts { get { return new ValeoCommands("contacts", RequestType.Menu); } }
        public static ValeoCommands UsiInfo { get { return new ValeoCommands("usiinfo", RequestType.Menu); } }
        public static ValeoCommands About { get { return new ValeoCommands("about", RequestType.Menu); } }
        public static ValeoCommands DoctorsStatic { get { return new ValeoCommands("doctorsstatic", RequestType.Menu); } }
        public static ValeoCommands OurDoctors { get { return new ValeoCommands("ourdoctors", RequestType.OurDoctors); } }
        public static ValeoCommands Feedback { get { return new ValeoCommands("feedback", RequestType.Feedback); } }
        public static ValeoCommands Location { get { return new ValeoCommands("location", RequestType.Location); } }
        public static ValeoCommands Safonov { get { return new ValeoCommands("safonov", RequestType.Safonov); } }
        public static ValeoCommands Palivoda { get { return new ValeoCommands("palivoda", RequestType.Palivoda); } }
        public static ValeoCommands Makarchenko { get { return new ValeoCommands("makarchenko", RequestType.Makarchenko); } }
        public static ValeoCommands Kalita { get { return new ValeoCommands("kalita", RequestType.Kalita); } }
        public static ValeoCommands Leonova { get { return new ValeoCommands("leonova", RequestType.Leonova); } }

        public static implicit operator ValeoCommands(string command)
        {
            return new ValeoCommands(command);
        }
        public static implicit operator string(ValeoCommands command)
        {
            return command.Value;
        }
        public static bool operator ==(ValeoCommands command1, ValeoCommands command2)
        {
            return command1.Value == command2.Value;
        }
        public static bool operator !=(ValeoCommands command1, ValeoCommands command2)
        {
            return command1.Value != command2.Value;
        }
        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is ValeoCommands res)
            {
                return res.Value == Value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }

    public enum RequestType
    {
        Default,
        Menu,
        Doctors,
        Times,
        Save,
        Contacts,
        UsiInfo,
        About,
        DoctorsStatic,
        OurDoctors,
        Feedback,
        Location,
        Safonov,
        Palivoda,
        Makarchenko,
        Kalita,
        Leonova
    }

    public class Test
    {

        public Test()
        {
            string inputCommand = "doctors";
            string inputEnum = ValeoCommands.Doctors;

            string res1 = ApplyCommand(inputCommand);
            ValeoCommands res2 = AppluEnum(inputEnum);
            bool res3 = ValeoCommands.Doctors == ValeoCommands.Default;
            bool res4 = ValeoCommands.Doctors != ValeoCommands.Doctors;
        }
        public string ApplyCommand(ValeoCommands param)
        {
            return param;
        }
        public ValeoCommands AppluEnum(string param)
        {
            return param;
        }
    }
}