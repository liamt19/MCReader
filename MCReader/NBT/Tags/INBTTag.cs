using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MCReader.NBT.Tags
{
    public interface INBTTag
    {
        static TAG_End END = new TAG_End();

        public int PayloadSize();
        public object Data();
        public string Name();
        public int TagID();

        public void Read(NBTReader r, bool noName = false);

        public static INBTTag Create(byte id, object data = null)
        {
            if (id == (int)TagType.TAG_Byte)
            {
                return new TAG_Byte(data);
            }
            else if (id == (int)TagType.TAG_Short)
            {
                return new TAG_Short(data);
            }
            else if (id == (int)TagType.TAG_Int)
            {
                return new TAG_Int(data);
            }
            else if (id == (int)TagType.TAG_Long)
            {
                return new TAG_Long(data);
            }
            else if (id == (int)TagType.TAG_Float)
            {
                return new TAG_Float(data);
            }
            else if (id == (int)TagType.TAG_Double)
            {
                return new TAG_Double(data);
            }
            else if (id == (int)TagType.TAG_Byte_Array)
            {
                return new TAG_Byte_Array(data);
            }
            else if (id == (int)TagType.TAG_String)
            {
                return new TAG_String(data);
            }
            else if (id == (int)TagType.TAG_List)
            {
                return new TAG_List(data);
            }
            else if (id == (int)TagType.TAG_Compound)
            {
                return new TAG_Compound(data);
            }
            else if (id == (int)TagType.TAG_Int_Array)
            {
                return new TAG_Int_Array(data);
            }
            else if (id == (int)TagType.TAG_Long_Array)
            {
                return new TAG_Long_Array(data);
            }

            return new TAG_End(data);
        }

        public static byte IDOf(INBTTag tag)
        {
            if (!Enum.TryParse(tag.GetType().Name, out TagType ret))
            {
                Log("Casting " + tag.ToString() + " failed");
            }
            return (byte) ret;
        }


        private static void FormatNBT_Rec(List<INBTTag> list, StringBuilder sb, int level)
        {
            
            foreach (INBTTag tag in list)
            {
                if (tag.Name() == "sections")
                {
                    int a = 0;
                }
                sb.Append('\t', level);

                if (tag.TagID() == (int)TagType.TAG_List)
                {
                    var t = ((TAG_List)tag);
                    if (false)
                    {
                        if (t.Name() != null)
                        {
                            sb.Append("List: [");
                        }
                        else
                        {
                            sb.Append(t.Name() + ": [");
                        }

                        foreach (var item in (List<INBTTag>)t.Data())
                        {
                            sb.Append(item.Data() + ", ");
                        }
                        sb.Remove(sb.Length - 2, 2);
                        sb.Append("]");
                    }
                    sb.Append(t.ToStringLevel(level + 1));
                }
                else if (tag.TagID() == (int)TagType.TAG_Int_Array)
                {
                    var t = ((TAG_Int_Array)tag);
                    if (false)
                    {
                        if (t.Name() != null)
                        {
                            sb.Append(t.Name() + ": {");
                        }
                        else
                        {
                            sb.Append("Int array: [");
                        }

                        foreach (var item in (TAG_Int[])t.Data())
                        {
                            sb.Append(item.Data() + ", ");
                        }
                        sb.Remove(sb.Length - 2, 2);
                        sb.Append("]");
                    }

                    sb.Append(t.ToString());
                }
                else if (tag.TagID() == (int) TagType.TAG_Long_Array)
                {
                    var t = ((TAG_Long_Array)tag);
                    
                    if (false)
                    {
                        if (t.Name() != null)
                        {
                            sb.Append(t.Name() + ": [");
                        }
                        else
                        {
                            sb.Append("Long array: [");
                        }

                        foreach (var item in (TAG_Long[])t.Data())
                        {
                            sb.Append(item.Data() + ", ");
                        }
                        sb.Remove(sb.Length - 2, 2);
                        sb.Append("]");
                    }

                    sb.Append(t.ToString());
                }
                else if (tag.TagID() == (int)TagType.TAG_Compound)
                {
                    var t = ((TAG_Compound)tag); 
                    
                    if (t.Name() != null)
                    {
                        sb.Append(t.Name() + ": {" + Environment.NewLine);
                    }
                    else
                    {
                        sb.Append("Compound: {" + Environment.NewLine);
                    }

                    FormatNBT_Rec((List<INBTTag>)t.Data(), sb, level + 1);

                    sb.Append('\t', level);
                    sb.Append("}" + Environment.NewLine);
                }
                else
                {
                    sb.Append(tag.Name() + ": " + tag.Data().ToString());
                }

                sb.Append(Environment.NewLine);
            }
        }

        public static string FormatNBT(List<INBTTag> list)
        {
            StringBuilder sb = new StringBuilder();
            FormatNBT_Rec(list, sb, 0);
            return sb.ToString();
        }

    }

    public enum TagType
    {
        TAG_End = 0,
        TAG_Byte = 1,
        TAG_Short = 2,
        TAG_Int = 3,
        TAG_Long = 4,
        TAG_Float = 5,
        TAG_Double = 6,
        TAG_Byte_Array = 7,
        TAG_String = 8,
        TAG_List = 9,
        TAG_Compound = 10,
        TAG_Int_Array = 11,
        TAG_Long_Array = 12,
    }
}
