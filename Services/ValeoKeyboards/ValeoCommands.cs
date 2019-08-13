using System;
using System.Collections.Generic;

namespace Valeo.Bot.Services.ValeoKeyboards
{
    public struct ValeoCommands
    {
        private static readonly Dictionary<string, Func<ValeoCommands>> _values = new Dictionary<string, Func<ValeoCommands>>()
        { { "doctors", () => Doctors }, { "back", () => Back }, { "default", () => Default }, { "usi", () => Usi }
        };
        public string Value { get; private set; }
        public RequestType RequestType { get; private set; }

        private ValeoCommands(string value, RequestType requestType = RequestType.LoadInfo)
        {
            RequestType = requestType;
            Value = value;
        }

        public static ValeoCommands Doctors { get { return new ValeoCommands("doctors", RequestType.Menu); } }
        public static ValeoCommands Back { get { return new ValeoCommands("back", RequestType.Menu); } }
        public static ValeoCommands Default { get { return new ValeoCommands("default", RequestType.Menu); } }
        public static ValeoCommands Usi { get { return new ValeoCommands("usi", RequestType.Menu); } }

        public static implicit operator ValeoCommands(string command)
        {
            return _values[command]();
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
        Menu,
        LoadInfo
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