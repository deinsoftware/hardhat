using System;

namespace dein.tools
{
    public static class Validation
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
                Exceptions.General(Ex.Message);
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
                Exceptions.General(Ex.Message);
            }
        }

        public static void Url(string uri, string error = null)
        {
            try
            {
                bool isUri = false;
                isUri = (!Uri.IsWellFormedUriString(uri, UriKind.Absolute));
                    
                Uri tmp;
                isUri = (!Uri.TryCreate(uri, UriKind.Absolute, out tmp));
                isUri = (tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps);
                if (!isUri)
                {
                    Message.Error(error);
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }
    }
}