using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace StarCraftBot_net
{
    public static class Extensions
    {
        public static String ToStarcraftString(this Object obj)
        {
            System.String result = "";

            if (obj != null)
            {
                if (obj is System.Collections.ICollection)
                {
                    result = ((ICollection)obj).ToStarcraftString();
                }
                else
                    result = obj.ToString();
            }
            else
                result = "null";

            return result;
        }
        public static string ToStarcraftString(ICollection c)
	    {
		    StringBuilder s = new StringBuilder();
    		
		    if (c != null)
		    {
			    ArrayList l = new ArrayList(c);

			    bool isDictionary = (c is BitArray || c is Hashtable || c is IDictionary || c is NameValueCollection || (l.Count > 0 && l[0] is DictionaryEntry));
			    for (int index = 0; index < l.Count; index++) 
			    {
				    if (l[index] == null)
					    s.Append("null");
				    else if (!isDictionary)
					    s.Append(l[index]);
				    else
				    {
					    isDictionary = true;
					    if (c is System.Collections.Specialized.NameValueCollection)
						    s.Append(((System.Collections.Specialized.NameValueCollection)c).GetKey (index));
					    else
						    s.Append(((System.Collections.DictionaryEntry) l[index]).Key);
					    s.Append("=");
					    if (c is System.Collections.Specialized.NameValueCollection)
						    s.Append(((System.Collections.Specialized.NameValueCollection)c).GetValues(index)[0]);
					    else
						    s.Append(((System.Collections.DictionaryEntry) l[index]).Value);

				    }
				    if (index < l.Count - 1)
					    s.Append(", ");
			    }
    			
			    if(isDictionary)
			    {
				    if(c is System.Collections.ArrayList)
					    isDictionary = false;
			    }
			    if (isDictionary)
			    {
				    s.Insert(0, "{");
				    s.Append("}");
			    }
			    else 
			    {
				    s.Insert(0, "[");
				    s.Append("]");
			    }
		    }
		    else
			    s.Insert(0, "null");
		    return s.ToString();
	    }

    }
}
