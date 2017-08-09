using System;

namespace dein.tools
{
    class Validation
    {
        public static void Range(string val, int min, int max, string error = null)
        {
            try
            {
                Number(val);
                int value;
                Int32.TryParse(val, out value);
                if ( min > value || value > max)
                {
                    Message.Error(error);
                }
            }
            catch (Exception Ex)
            {
                Message.Critical(
                    msg: Ex.Message
                );
            }
        }

        public static void Number(string val, string error = null)
        {
            try
            {
                int value;
                bool isNumeric = int.TryParse(val, out value);
                Int32.TryParse(val, out value);
                if (!isNumeric)
                {
                    Message.Error(error);
                }
            }
            catch (Exception Ex)
            {
                Message.Critical(
                    msg: Ex.Message
                );
            }
        }
    }
}