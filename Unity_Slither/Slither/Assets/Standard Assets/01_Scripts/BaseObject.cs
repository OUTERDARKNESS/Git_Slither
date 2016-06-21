using System;
using System.Collections.Generic;

[Serializable()]
public class BaseObject
{
    #region Conversions
    /// <summary>
    /// A simple/shallow XML serialization.  This will only serialize whatever is exposed as "public".
    /// </summary>
    /// <returns>XML string representation of the object.</returns>
    public override string ToString()
    {
        System.Xml.Serialization.XmlSerializer XMLSer = new System.Xml.Serialization.XmlSerializer(this.GetType());
        System.IO.StringWriter sw = new System.IO.StringWriter();
        XMLSer.Serialize(sw, this);
        return sw.ToString();
    }

    public System.Xml.XmlDocument ToXmlDocument()
    {
        System.Xml.XmlDocument Ret = new System.Xml.XmlDocument();
        Ret.LoadXml(this.ToString());
        return Ret;
    }

    public static T FromXmlDocument<T>(System.Xml.XmlDocument XmlDoc) where T : BaseObject
    {
        System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));

        return (T)xs.Deserialize(new System.Xml.XmlNodeReader(XmlDoc));
    }

    public static T FromString<T>(string XmlString) where T : BaseObject
    {
        try
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
            return (T)xs.Deserialize(new System.IO.StringReader(XmlString));
        }
        catch
        {
            // Indicate a failure via a null return, not via exceptions, much cleaner.
        }

        return null;
    }
    #endregion

}
